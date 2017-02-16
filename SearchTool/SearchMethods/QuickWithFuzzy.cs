using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.SearchMethods
{
    public class QuickWithFuzzy : ISearcherMethod
    {
        private int _lengthSourceText;
        private int _limit;

        public QuickWithFuzzy(int limit)
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

        private int CompareTwoWords(string dataBuffer, string searchText)
        {
            if (dataBuffer.Length == 1 || dataBuffer.Length == 0)
            {
                var compare = dataBuffer.CompareTo(searchText);
                return compare == 0 ? 0 : searchText.Length;
            }
            int halfLengthSearchText;
            if (dataBuffer.Length % 2 == 0)
                halfLengthSearchText = dataBuffer.Length / 2;
            else
            {
                halfLengthSearchText = dataBuffer.Length / 2 + 1;
            }
            var searchTextPart1 = dataBuffer.Substring(0, halfLengthSearchText);
            var searchTextPart2 = dataBuffer.Substring(halfLengthSearchText - 1);

            var diff1 = LevenshteinDistance(searchTextPart1, searchText);
            var diff2 = LevenshteinDistanceReverse(searchTextPart2, searchText);

            return MinCountChanges(diff1, diff2);

            //return diffRes <= _limit;
        }

        private int MinCountChanges(int[] DistanceSearchTextPart1, int[] DistanceSearchTextPart2)
        {
            int[] sumDistance = new int[DistanceSearchTextPart1.Length];
            for (int i = 0; i < DistanceSearchTextPart1.Length; i++)
            {
                sumDistance[i] = DistanceSearchTextPart1[i] + DistanceSearchTextPart2[i];
            }
            return sumDistance.Min();
        }

        private int[] LevenshteinDistance(string dataBufferPart1, string searchText)
        {
            if (dataBufferPart1 == null) throw new ArgumentNullException("string1");
            if (searchText == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[searchText.Length + 1, dataBufferPart1.Length + 1];
            int[] result = new int[searchText.Length + 1];
            result[0] = dataBufferPart1.Length;

            for (int i = 0; i <= searchText.Length; i++)
            {
                m[i, 0] = i;
            }
            for (int j = 0; j <= dataBufferPart1.Length; j++)
            {
                m[0, j] = j;
            }

            for (int i = 1; i <= dataBufferPart1.Length; i++)
            {
                for (int j = 1; j <= searchText.Length; j++)
                {
                    diff = (searchText[j - 1] == dataBufferPart1[i - 1]) ? 0 : 1;

                    m[j, i] = Math.Min(Math.Min(m[j, i - 1] + 1,
                        m[j - 1, i] + 1),
                        m[j - 1, i - 1] + diff);
                    if (i == dataBufferPart1.Length)
                    {
                        result[j] = m[j, i];
                    }
                }
            }

            return result;
        }

        private int[] LevenshteinDistanceReverse(string dataBufferPart2, string string2)
        {
            if (dataBufferPart2 == null) throw new ArgumentNullException("string1");
            if (string2 == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[string2.Length + 1, dataBufferPart2.Length];
            int[] result = new int[string2.Length + 1];
            result[string2.Length] = dataBufferPart2.Length - 1;
            result[0] = string2.Length - dataBufferPart2.Length + 1;


            int string1Length = dataBufferPart2.Length - 1;
            int string2Length = string2.Length;

            for (int i = string2Length; i >= 0; i--)
            {
                m[i, string1Length] = string2Length - i;

            }
            for (int j = string1Length, k = 0; j >= 0; j--, k++)
            {
                m[string2Length, j] = string1Length - j;
                m[0, j] = string2Length - k;

            }

            for (int i = dataBufferPart2.Length - 2; i >= 0; i--)
            {
                for (int j = string2.Length - 1; j > 0; j--)
                {
                    diff = (string2[j - 1] == dataBufferPart2[i]) ? 0 : 1;

                    m[j, i] = Math.Min(Math.Min(m[j, i + 1] + 1,
                        m[j + 1, i] + 1),
                        m[j + 1, i + 1] + diff);
                    if (i == 0)
                    {
                        result[j] = m[j, 0];
                    }
                }
            }

            return result;
        }
    }
}
