using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyLogic.Classes
{
    public class ExpertClass
    {
        public Dictionary<(string TermName, string TermValue), decimal> ValuesDictionary;
        public string Name;

        public List<string> TermValues
        {
            get
            {
                var result = ValuesDictionary.Keys.Select(c => c.TermValue).Distinct().ToList();
                return result;
            }
        }

        public List<string> TermNames
        {
            get
            {
                var result = ValuesDictionary.Keys.Select(c => c.TermName).Distinct().ToList();
                return result;
            }
        }

        public ExpertClass(string name)
        {
            ValuesDictionary = new Dictionary<(string TermName, string TermValue), decimal>();
            Name = name;
        }

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
