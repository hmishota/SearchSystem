using SearchTool.Interfaces;
using SearchTool.Models;
using System.Collections.Generic;

namespace SearchTool.SearchMethods
{
    public class SearcherMethodBoyer_Moore : ISearcherMethod
    {
        public char[] SymbolOfX; //Таблица символов искомой строки  
        public int[] ValueShift; //Таблица смещений для символов


        public List<SearchResult> Search(Data text, string searchText)
        {
            List<SearchResult> searchResult = new List<SearchResult>();
            searchResult = BM(text, searchText);
            foreach (var search in searchResult)
            {
                search.File = new File(text.Path);
                search.Position = text.Position + search.Position;
            }
            return searchResult;
        }

        private void ShiftBM(string x) // Функция - формирование смещений
        {
            int j; //Счетчик
            int k = 0; //Счетчик
            bool fl; //Флаг
            SymbolOfX = new char[x.Length]; //Инициализация
            ValueShift = new int[x.Length]; //Инициализация
            //Цикл по искомой строке без последнего символа
            for (int i = x.Length - 2; i >= 0; i--)
            {
                fl = false; //Флаг
                j = 0; //Обнуление
                while ((j < k + 1) && (fl == false))
                {
                    if (SymbolOfX[j] == x[i]) fl = true;
                    j++;
                }
                if (fl == false)
                {
                    SymbolOfX[k] = x[i];
                    ValueShift[k] = x.Length - i - 1;
                    k++;
                }
            }
        }
        //Функция поиска алгоритмом БМ
        private List<SearchResult> BM(Data data, string x)
        {
            List<SearchResult> foundResults = new List<SearchResult>();

            string s = data.Buffer;
            bool has, have; //Флаги
            int l, j, i; //Счетчики
            ShiftBM(x); //Вызов процедуры, формирующей таблицу смещений
            if (x.Length > s.Length) return foundResults;
            //Основной цикл по исходной строке
            for (i = 0; i < s.Length - x.Length + 1; i++)
            {
                j = x.Length - 1;
                have = true;
                //Проверка с последнего символа                
                while ((j >= 0) && (have == true))
                {
                    //Если не совпадает символ искомой и исходной
                    if (s[i + j] != x[j])
                    {
                        have = false;
                        //Если это последний символ
                        if (j == x.Length - 1)
                        {
                            l = 0;
                            has = false; //Флаг
                            //Поиск символа в таблице смещений
                            while ((l < x.Length) && (has == false))
                            {
                                //Если символ есть
                                if (s[i + j] == SymbolOfX[l])
                                {
                                    has = true; //Изменение флага
                                    i = i + ValueShift[l] - 1; //Сдвиг на величину
                                }
                                l++;
                            }
                            //Если не найден символ в таблице смещений
                            if (has == false)
                                //Сдвиг на величину искомой строки
                                i = i + x.Length - 1;
                        }
                    }
                    j--;
                }
                if (have == true)
                    foundResults.Add(new SearchResult { Position = i });

            }
            
            return foundResults; //Возвращение результата поиска
        }

    }
}
