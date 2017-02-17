using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.SearchMethods
{
    public class QuickWithFuzzy : FuzzySearch, ISearcherMethod
    {
        private int _lengthSourceText;
        private int _limit;

        public QuickWithFuzzy(int limit):base(limit)
        {
            _limit = limit;
        }

        public List<SearchResult> Search(Data text, string searchText)
        {
            var searchResult = new List<SearchResult>();
            _lengthSourceText = searchText.Length;
            searchResult = FasterQuick(text, searchText);
            foreach (var search in searchResult)
            {
                search.File = new File(text.Path);
                search.Position = text.Position + search.Position;
            }
            return searchResult;
        }


        // Строиться таблица смещений по которым будет перемещаться затем алгоритм в процессе поиска 
        private Dictionary<char, int> GenerateOffsetTable(string sourceText, int lenght, char[] uniqueSymbolsInSearchTexts)
        {
            var countUniqueSymbols = uniqueSymbolsInSearchTexts.Count();
            var offsetTable = new Dictionary<char, int>(countUniqueSymbols);

            // Увеличиваю на единицу чтобы заполнить этим числом изначально таблицу
            var lenghtSourceTextWithOne = lenght + 1;
            for (var i = 0; i < countUniqueSymbols; i++)
            {
                // Заполнение начальными значениями таблицу(словарь)
                offsetTable.Add(uniqueSymbolsInSearchTexts[i], lenghtSourceTextWithOne);
            }

            for (var i = 0; i < lenght; i++)
            {
                //var bias = lenght - i - _limit;
                //if (bias < 1)
                //    bias = 1;
                // Смещения считаются справа налево
                offsetTable[sourceText[i]] = lenght - i;
            }
            return offsetTable;
        }

        // Вычисляется максимальная позиция смещения 
        private int GetMaxPos(string sourceText, char[] uniqueSymbolsInSearchTexts)
        {
            // Ожидаемый сдвиг
            var expectedShift = 0;
            var maxExpectedShift = 0;
            var pos = 0;
            var countUniqueSymbols = uniqueSymbolsInSearchTexts.Count();
            var prePos = new Dictionary<char, int>(countUniqueSymbols);

            for (var i = 0; i < countUniqueSymbols; i++)
            {
                // Заполнение начальными значениями таблицу
                prePos[uniqueSymbolsInSearchTexts[i]] = -1;
            }

            for (var j = 0; j < _lengthSourceText; j++)
            {
                // Вычисление ожидаемого сдвига
                expectedShift = expectedShift + countUniqueSymbols - (j - prePos[sourceText[j]]);
                prePos[sourceText[j]] = j;

                if (expectedShift < maxExpectedShift) continue;
                maxExpectedShift = expectedShift;
                pos = j;
            }

            return pos;
        }

        // Вычисление следующей позиции j
        private int FollowingDisplacement(Dictionary<char, int> next, int j, int pos, string text, bool stepsmall)
        {
            int value;

            if (next.TryGetValue(text[j + pos], out value))
            {
                if ((j + value) < (text.Length - _lengthSourceText))
                    return j + value;
            }
            return j + 1;
        }

        private List<SearchResult> FasterQuick(Data data, string sourceText)
        {
            var foundResults = new List<SearchResult>();

            // Исходный текст, в котором будут искать шаблон
            var text = data.Buffer;
            var lengthText = text.Length;

            // Заполнение только уникальными символами изшаблона
            var uniqueSymbolsInSearchTexts = sourceText.Distinct().ToArray();
            Dictionary<char, int> next;
            var pos = GetMaxPos(sourceText, uniqueSymbolsInSearchTexts);

            // Таблица смещений отн-но найденной pos
            next = GenerateOffsetTable(sourceText, pos, uniqueSymbolsInSearchTexts);

            // Таблица смещений отн-но всего шаблона sourceText
           // shift = GenerateOffsetTable(sourceText, _lengthSourceText, uniqueSymbolsInSearchTexts);
            //var value = 0;
            var flag = false;
            bool compare;
            var j = 0;
            int step = 0;
            while (true)
            {
                compare = true;

                if (j > lengthText - _lengthSourceText)
                    step++;

                string dataBuffer = text.Substring(j, sourceText.Length - step);
                // Сравнение строк
                var compareTwoWords = CompareTwoWords(dataBuffer, sourceText);

                if (compareTwoWords > _limit)
                    compare = false;

                if (compare)
                {
                    foundResults.Add(new SearchResult { Position = j });
                    flag = true;
                }
                
                if (j == lengthText - pos)
                    break;
                j = FollowingDisplacement(next, j, pos, text, true);
            }
            return foundResults;
        }

    }
}
