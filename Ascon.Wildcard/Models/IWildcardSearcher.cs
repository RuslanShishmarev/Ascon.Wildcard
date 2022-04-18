using System.Collections.Generic;

namespace Ascon.Wildcard.Models
{
    public interface IWildcardSearcher
    {
        void AddWord(string word); // добавление слова в словарь
        IEnumerable<string> SearchWords(string pattern); // поиск слов, подходящих под шаблон
    }
}
