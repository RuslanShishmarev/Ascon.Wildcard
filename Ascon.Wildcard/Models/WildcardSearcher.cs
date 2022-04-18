using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascon.Wildcard.Models
{
    public class WildcardSearcher : IWildcardSearcher
    {
        public string Text { get; private set; }

        public bool WithRegister { get; private set; }

        public IEnumerable<string> UniqueWords { get; private set; }

        public WildcardSearcher(string text, bool withRegister)
        {
            Text = text;
            WithRegister = withRegister;
            UniqueWords = GetUniqueWords(text, withRegister);
        }

        public WildcardSearcher()
        {

        }

        public void AddWord(string word)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> SearchWords(string pattern)
        {
            return UniqueWords.Where(x => IsWordEqualPattern(pattern, x));
        }

        private bool IsWordEqualPattern(string pattern, string word)
        {
            string patternForRegex = pattern.Replace("*", "\\" + "w*").Replace("?", ".");

            Regex regex = new Regex(patternForRegex);
            Match match = regex.Match(word);

            return match.Success;
        }

        private IEnumerable<string> GetUniqueWords(string text, bool withRegister)
        {
            char[] separators = { '.', ',', ':', '!', '?' };

            Regex reg = new Regex("[^a-zA-Z0-9]");
            string txt = reg.Replace(text, " ");

            if (!withRegister) txt = txt.ToLower();

            return txt.Split(' ').OrderBy(x => x).Distinct();
        }
    }
}
