﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuzzyLogic.Forms;

namespace FuzzyLogic.Classes
{
    public class DataClass
    {
        public static Form1 Form1;

        /// <summary>
        /// Словарь класс эксперта по имени эксперта
        /// </summary>
        public Dictionary<string, ExpertClass> ExpertClasses;

        /// <summary>
        /// Данные в виде таблицы для DataGridView
        /// </summary>
        public DataTable DataTable;

        public ExpertClass CalculatedExpertClass;

        /// <summary>
        /// Названия колонок
        /// </summary>
        public List<string> ColumnCaptions
        {
            get
            {
                var result = new List<string>();
                foreach (DataColumn column in DataTable.Columns)
                {
                    result.Add(column.Caption);
                }

                return result;
            }
        }

        public DataClass(string tableName)
        {
            ExpertClasses = new Dictionary<string, ExpertClass>();
            DataTable = new DataTable(tableName);
            Load();
            DataTable.RowChanging += DataTable_Row_Changing;
            //Calculation();
            //GenerateDataTable();
        }

        public DataClass(string tableName, Dictionary<string, ExpertClass> expertClasses)
        {
            ExpertClasses = expertClasses;
            DataTable = new DataTable(tableName);
            GenerateDataTable();
            DataTable.RowChanging += DataTable_Row_Changing;
        }

        public DataClass(string tableName, ExpertClass expertClass)
        {
            ExpertClasses = new Dictionary<string, ExpertClass>() { [expertClass.Name] = expertClass };
            DataTable = new DataTable(tableName);
            GenerateDataTable();
        }

        private void DataTable_Row_Changing(object sender, DataRowChangeEventArgs e)
        {
            var expertName = e.Row[0].ToString();
            var termName = e.Row[1].ToString();
            var temp = ExpertClasses[expertName].ValuesDictionary;
            var termValues = temp.Select(c => c.Key.TermValue).ToList();
            foreach (var termValue in termValues)
            {
                temp[(termName, termValue)] = Convert.ToDecimal(e.Row[termValue]);
            }
            
            Calculation();
            Form1.RefreshChartAndDataGrid();
        }

        public void Save()
        {
            DataTable.WriteXml($"{DataTable.TableName}.xml", XmlWriteMode.WriteSchema);
        }

        /// <summary>
        /// Загрузка DataTable и Экспертов
        /// </summary>
        public void Load()
        {
            DataTable.ReadXmlSchema($"{DataTable.TableName}.xml");
            DataTable.ReadXml($"{DataTable.TableName}.xml");
            var expertNames = Enumerable.Range(0, DataTable.Rows.Count)
                .Select(c => DataTable.Rows[c]["Эксперты"].ToString())
                .Distinct()
                .ToList();
            var termNames = Enumerable.Range(0, DataTable.Rows.Count)
                .Select(c => DataTable.Rows[c]["Термы"].ToString())
                .Distinct()
                .ToList();
            var termValues = Enumerable.Range(2, DataTable.Columns.Count-2)
                .Select(c => DataTable.Columns[c].ColumnName)
                .ToList();
            var I = 0;
            foreach (var expertName in expertNames)
            {
                var expert = new ExpertClass(expertName);
                foreach (var termName in termNames)
                {
                    foreach (var termValue in termValues)
                    {
                        var item = DataTable.Rows[I][termValue].ToString();
                        expert.ValuesDictionary[(termName, termValue)] = Convert.ToDecimal(item);
                    }

                    I++;
                }

                ExpertClasses[expertName] = expert;
            }
        }

        /// <summary>
        /// Генерация таблицы DataTable
        /// </summary>
        public void GenerateDataTable()
        {
            var fixedColumns = new List<(string ColumnName, List<string> Items)>()
            {
                ("Эксперты", ExpertClasses.Select(c => c.Value.Name).ToList()),
                ("Термы", ExpertClasses.Values.ElementAt(0)
                    .ValuesDictionary
                    .Keys.Select(c => c.TermName).Distinct().ToList()),
            };
            var unfixedColumnNames = ExpertClasses.Values.ElementAt(0)
                .ValuesDictionary
                .Keys.Select(c => c.TermValue).Distinct().ToList();

            var columnNames = fixedColumns.Select(c => c.ColumnName).ToList();
            foreach (var unfixedColumnName in unfixedColumnNames)
            {
                columnNames.Add(unfixedColumnName);
            }

            foreach (var columnName in columnNames)
            {
                var column = new DataColumn()
                {
                    ColumnName = columnName,
                    Caption = columnName,
                    DataType = typeof(decimal),
                    ReadOnly = false
                };
                if (columnNames.GetRange(0, fixedColumns.Count).Contains(columnName))
                {
                    column.DataType = typeof(string);
                    column.ReadOnly = true;
                }

                DataTable.Columns.Add(column);
            }

            foreach (var expertClass in ExpertClasses)
            {
                foreach (var termName in expertClass.Value.TermNames)
                {
                    var row = DataTable.NewRow();
                    var valuesByTermName = expertClass.Value.GetValuesByTermName(termName).Select(c => (object)c);
                    var items = new object[]
                    {
                        expertClass.Value.Name,
                        termName,
                    }.Union(valuesByTermName);
                    row.ItemArray = items.ToArray();
                    DataTable.Rows.Add(row);
                }
            }
        }

        /// <summary>
        /// Расчет значений экспертов для создания графика
        /// </summary>
        /// <returns></returns>
        public void Calculation()
        {
            var resultDictionary = new Dictionary<(string TermName, string TermValue), decimal>();
            var expertsDictionary = ExpertClass.GetByExpertTermValue(ExpertClasses);
            var allTermNames = expertsDictionary.Keys.Select(c => c.TermName).Distinct();
            var allTermValues = expertsDictionary.Keys.Select(c => c.TermValue).Distinct();

            foreach (var termValue in allTermValues)
            {
                foreach (var termName in allTermNames)
                {
                    var valueByTermNameAndTermValues = expertsDictionary.Where(c => c.Key.TermValue == termValue && c.Key.TermName == termName).ToList();
                    var valuesSum = valueByTermNameAndTermValues.Sum(c => c.Value);
                    resultDictionary[(termName, termValue)] = valuesSum;
                }
            }

            CalculatedExpertClass = new ExpertClass("Общая сумма", resultDictionary);
        }
    }
}
