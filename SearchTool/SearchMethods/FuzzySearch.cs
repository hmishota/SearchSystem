using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections;

namespace SearchTool
{
    public class FuzzySearch : ISearcherMethod
    {
        private int _limit ;

        public FuzzySearch(int limit)
        {
            _limit = limit;
        }

        public List<SearchResult> Search(Data text, string S2)
        {
            Tokenizer tokenizer = new Tokenizer();
            var tokens = tokenizer.Parse(text);
            List<SearchResult> listResults = new List<SearchResult>();
            
            foreach (var token in tokens)
            {
                var compareTwoWords = CompareTwoWords(token.Key, S2);

                if (compareTwoWords <= _limit)
                {
                    listResults.AddRange(token.Value);
                }
            }

            return listResults;
        }

        private int CompareTwoWords(string dataBuffer, string searchText) 
        {
            if (dataBuffer.Length == 1 || dataBuffer.Length == 0)
            {
                var compare = dataBuffer.CompareTo(searchText);
                return compare == 0 ? 0 : searchText.Length;
            }
            int halfLengthSearchText;
            if (dataBuffer.Length%2 == 0)
                halfLengthSearchText = dataBuffer.Length/2;
            else
            {
                halfLengthSearchText = dataBuffer.Length/2 + 1;
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
