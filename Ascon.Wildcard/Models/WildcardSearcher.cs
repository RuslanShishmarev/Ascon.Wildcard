using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ascon.Wildcard.Models
{
    public class WildcardSearcher : IWildcardSearcher
    {
        private (bool, string) USER_WORDS = (true, nameof(USER_WORDS));

        public bool IsReady => !string.IsNullOrEmpty(Text);

        public Dictionary<(bool, string), IEnumerable<string>> SearchedWords { get; private set; }

        public string Text { get; private set; }

        public bool WithRegister { get; private set; }

        public IEnumerable<string> UniqueWords { get; private set; }

        public WildcardSearcher(bool withRegister)
        {
            WithRegister = withRegister;
            SearchedWords = new Dictionary<(bool, string), IEnumerable<string>>();
        }

        public void SetIsWithRegister(bool withRegister)
        {
            WithRegister = withRegister;
        }

        public void SetText(string text)
        {
            Text = text;
            UniqueWords = GetUniqueWords(text);
        }

        public void AddWord(string word)
        {
            if(SearchedWords.ContainsKey(USER_WORDS)) 
            {
                var userWords = SearchedWords[USER_WORDS].Concat(new string[] { word });
                SearchedWords[USER_WORDS] = userWords;
                return;
            }

            SearchedWords[USER_WORDS] = new string[] { word };            
        }

        public IEnumerable<string> SearchWords(string pattern)
        {
            if (SearchedWords.ContainsKey((WithRegister, pattern))) 
            { 
                return SearchedWords[(WithRegister, pattern)]; 
            }
            else
            {
                var words = UniqueWords?.Where(x => IsWordEqualPattern(pattern, x));
                SearchedWords[(WithRegister, pattern)] = words;
                return words;
            }
        }

        public IEnumerable<string> GetUserWords()
        {
            return SearchedWords.ContainsKey(USER_WORDS) ? SearchedWords[USER_WORDS] : Array.Empty<string>();
        }

        public void Reset()
        {
            Text = string.Empty;
            UniqueWords = Array.Empty<string>();
            SearchedWords?.Clear();
        }

        private bool IsWordEqualPattern(string pattern, string word)
        {
            if (!pattern.Contains("*") && !pattern.Contains("?")) return pattern == word;
            if (!pattern.StartsWith("*")) pattern = "^" + pattern;

            string patternForRegex = pattern.Replace("*", "(\\" + "w*)").Replace("?", "(.)") + "$";            

            Regex regex = new Regex(@patternForRegex, !WithRegister ? RegexOptions.IgnoreCase : RegexOptions.None);
            Match match = regex.Match(word);

            return match.Success;
        }

        private IEnumerable<string> GetUniqueWords(string text)
        {
            Regex reg = new Regex("[^a-zA-Zа-яА-Я]");
            string txt = reg.Replace(text, " ");

            return txt.Split(' ').OrderBy(x => x).Where(x => x != string.Empty).Distinct();
        }
    }
}
