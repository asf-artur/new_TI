using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic.Classes
{
    public class DataClass
    {
        //public List<ExpertClass> ExpertClasses;
        public Dictionary<string, ExpertClass> ExpertClasses;
        public DataTable DataTable;

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

        public DataClass()
        {
            ExpertClasses = new Dictionary<string, ExpertClass>();
            DataTable = new DataTable("Table1");
            Load();
            //GenerateDataTable();
        }

        public DataClass(Dictionary<string, ExpertClass> expertClasses)
        {
            ExpertClasses = expertClasses;
            DataTable = new DataTable("Table1");
            GenerateDataTable();
        }

        public void Save()
        {
            DataTable.WriteXml($"{DataTable.TableName}.xml", XmlWriteMode.WriteSchema);
        }

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
            var J = 0;
            foreach (var expertName in expertNames)
            {
                var expert = new ExpertClass(expertName);
                foreach (var termName in termNames)
                {
                    J = I;
                    foreach (var termValue in termValues)
                    {
                        var item = DataTable.Rows[I][termValue].ToString();
                        expert.ValuesDictionary[(termName, termValue)] = 0;
                        I++;
                    }

                    I = J;
                }

                ExpertClasses[expertName] = expert;
            }
        }

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
                    //ColumnName = $"column{columnNames.IndexOf(columnName)}",
                    Caption = columnName,
                    DataType = typeof(decimal),
                    ReadOnly = false
                };
                if (columnNames.GetRange(0, fixedColumns.Count).Contains(columnName))
                {
                    column.DataType = typeof(string);
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

        public void Calculation()
        {
            foreach (var VARIABLE in )
            {
                
            }
        }
    }
}
