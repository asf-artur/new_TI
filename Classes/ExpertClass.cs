using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic.Classes
{
    public class ExpertClass
    {
        /// <summary>
        /// Словарь значений : 0 или 1 по паре (имя терма, значение терма)
        /// </summary>
        public Dictionary<(string TermName, string TermValue), decimal> ValuesDictionary;

        /// <summary>
        /// Имя эксперта
        /// </summary>
        public string Name;

        /// <summary>
        /// Значения термов
        /// </summary>
        public List<string> TermValues
        {
            get
            {
                var result = ValuesDictionary.Keys.Select(c => c.TermValue).Distinct().ToList();
                return result;
            }
        }

        /// <summary>
        /// Название терма
        /// </summary>
        public List<string> TermNames
        {
            get
            {
                var result = ValuesDictionary.Keys.Select(c => c.TermName).Distinct().ToList();
                return result;
            }
        }

        /// <summary>
        /// Создание эксперта с нуля
        /// </summary>
        /// <param name="name">Имя эксперта</param>
        public ExpertClass(string name)
        {
            ValuesDictionary = new Dictionary<(string TermName, string TermValue), decimal>();
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Имя эксперта</param>
        /// <param name="valuesDictionary">Значения</param>
        public ExpertClass(string name, Dictionary<(string TermName, string TermValue), decimal> valuesDictionary)
        {
            ValuesDictionary = valuesDictionary;
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expertClassesDictionary">Словарь класс эксперта по имени эксперта</param>
        /// <returns>Возращает словарь значений по именам экспертов</returns>
        public static Dictionary<(string ExpertName, string TermName, string TermValue), decimal> GetByExpertTermValue(Dictionary<string, ExpertClass> expertClassesDictionary)
        {
            var result = new Dictionary<(string ExpertName, string TermName, string TermValue), decimal>();
            var expertClasses = expertClassesDictionary.Values;
            foreach (var expert in expertClasses)
            {
                foreach (var termName in expert.TermNames)
                {
                    foreach (var termValue in expert.TermValues)
                    {
                        var value = expert.ValuesDictionary[(termName, termValue)];
                        result.Add((expert.Name, termName, termValue), value);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Загрузка имен термов, значений термов, количества экспертов
        /// </summary>
        /// TODO Сделать загрузку
        public void Load()
        {
            var termNames = new List<string>()
            {
                "Низкий",
                "Средний",
            };
            var termValues = new List<string>()
            {
                "[160,165)",
                "[165,170)",
            };

            var I = 0;
            foreach (var termName in termNames)
            {
                foreach (var termValue in termValues)
                {
                    ValuesDictionary[(termName, termValue)] = I;
                    I++;
                }
            }
        }

        /// <summary>
        /// Получение значений по имени терма
        /// </summary>
        /// <param name="termName">Имя терма</param>
        /// <returns>Значения</returns>
        public List<decimal> GetValuesByTermName(string termName)
        {
            var result = new List<decimal>();
            foreach (var termValue in TermValues)
            {
                result.Add(ValuesDictionary[(termName, termValue)]);
            }

            return result;
        }
    }
}
