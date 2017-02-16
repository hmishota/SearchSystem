using System;
using System.Collections.Generic;
using System.Linq;
using SearchTool.Interfaces;
using SearchTool.Models;

namespace SearchTool.SearchMethods
{
    public class FuzzySearchForEachSymbol : ISearcherMethod
    {
        private int _limit;

        public FuzzySearchForEachSymbol(int limit)
        {
            _limit = limit;
        }

        public List<SearchResult> Search(Data searchText, string source)
        {
            int n = searchText.Buffer.Length;
            int sourceLength = source.Length;
            List<SearchResult> listResults = new List<SearchResult>();

            string buffer = searchText.Buffer;
            for (int i = 0; i <= n - source.Length; i++) //проверить на концах
            {
                string str = buffer.Substring(i, sourceLength);
                var compareTwoWords = CompareTwoWords(str, source);
                if (compareTwoWords)
                {
                    var searchResult = new SearchResult()
                    {
                        File = new File(searchText.Path),
                        Position = searchText.Position + i
                    };
                    listResults.Add(searchResult);
                    i += sourceLength-1;
                }
            }
            return listResults;
        }

        private bool CompareTwoWords(string dataBuffer, string searchText)
        {
            if (dataBuffer.Length == 1 || dataBuffer.Length == 0)
            {
                var compare = dataBuffer.CompareTo(searchText);
                return compare == 0 ? true : false;
            }
            int halfLengthSearchText;
            if (dataBuffer.Length % 2 == 0)
                halfLengthSearchText = dataBuffer.Length / 2;
            else
            {
                halfLengthSearchText = dataBuffer.Length / 2 + 1;
            }
            var dataBufferPart1 = dataBuffer.Substring(0, halfLengthSearchText);
            var dataBufferPart2 = dataBuffer.Substring(halfLengthSearchText - 1);

            var diff1 = LevenshteinDistance(dataBufferPart1, searchText);
            var diff2 = LevenshteinDistanceReverse(dataBufferPart2, searchText);

            var diffRes = MinCountChanges(diff1, diff2);

            return diffRes <= _limit;
        }

        private int MinCountChanges(int[] distanceSearchTextPart1, int[] distanceSearchTextPart2)
        {
            int[] sumDistance = new int[distanceSearchTextPart1.Length];
            for (int i = 0; i < distanceSearchTextPart1.Length; i++)
            {
                sumDistance[i] = distanceSearchTextPart1[i] + distanceSearchTextPart2[i];
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
        
        private int[] LevenshteinDistanceReverse(string dataBufferPart2, string searchText)
        {
            if (dataBufferPart2 == null) throw new ArgumentNullException("string1");
            if (searchText == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[searchText.Length + 1, dataBufferPart2.Length];
            int[] result = new int[searchText.Length + 1];
            result[searchText.Length] = dataBufferPart2.Length - 1;
            result[0] = searchText.Length - dataBufferPart2.Length + 1;


            int string1Length = dataBufferPart2.Length - 1;
            int string2Length = searchText.Length;

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
                for (int j = searchText.Length - 1; j > 0; j--)
                {
                    diff = (searchText[j - 1] == dataBufferPart2[i]) ? 0 : 1;

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