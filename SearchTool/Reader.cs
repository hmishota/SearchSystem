using System;
using System.IO;
using System.Threading.Tasks;
using SearchTool.Interfaces;
using SearchTool.Models;
using System.Diagnostics;
using System.Collections.Concurrent;

namespace SearchTool
{

    public static class ResultTime
    {
        public static ConcurrentQueue<long> queryListRead;
        public static ConcurrentQueue<long> queryListSearch;

        static ResultTime()
        {
            queryListRead = new ConcurrentQueue<long>();
            queryListSearch = new ConcurrentQueue<long>();
        }
    }

    public class ThreadSafeReader : IReader
    {
        IReader _reader;

        public ThreadSafeReader(IReader reader)
        {
            _reader = reader;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public void InitVariables(int sizeBufferReader, int sizeBufferWritter, Models.File f)
        {
            _reader.InitVariables(sizeBufferReader, sizeBufferWritter, f);
        }

        public Task<Data> ReadAsync()
        {
            return _reader.ReadAsync();
        }
      
    }

    public interface IReadCounter
    {
        void ReaderAddTime();
    }

    public class ReadWithCounts : IReader, IReadCounter
    {
        private Reader _reader;

        private Stopwatch _getStopWatch;

        public ReadWithCounts()
        {
            _reader = new Reader();
            _getStopWatch = new Stopwatch();
        }

        public void InitVariables(int sizeBufferReader, int sizeBufferWritter, Models.File f)
        {
            _reader.InitVariables(sizeBufferReader, sizeBufferWritter, f);
        }

        public void ReaderAddTime()
        {
            ResultTime.queryListRead.Enqueue(_getStopWatch.ElapsedMilliseconds);
        }

        public async Task<Data> ReadAsync()
        {
            _getStopWatch.Start();
            var read= await _reader.ReadAsync();
            _getStopWatch.Stop();
            ReaderAddTime();
            return read;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }

    public class Reader : IReader
    {
        private FileStream fileStream;
        private BufferedStream buffStream;
        private int sizeBufferReader;
        private int sizeBufferWritter;
        private int currentNumberRecordedElements = 0;
        private int numberTimesRead;
        private Models.File file;

        public void InitVariables(int sizeBufferReader, int sizeBufferWritter, Models.File f)
        {
            this.sizeBufferReader = sizeBufferReader;
            this.sizeBufferWritter = sizeBufferWritter;
            fileStream = new FileStream(f.Path, FileMode.Open);
            Initialize(fileStream, buffStream);
            file = new Models.File(f.Path);
        }

        public void Initialize(Stream fin, BufferedStream buffStream)
        {
                this.buffStream = new BufferedStream(fin, sizeBufferReader);
                currentNumberRecordedElements = 0;
        }

        public async Task<Models.Data> ReadAsync()
        {
                var dataOut = new Models.Data();
            // Проверка на конец файла
            if (buffStream.Position == buffStream.Length)
                return null;

            numberTimesRead = sizeBufferWritter / sizeBufferReader;

            dataOut.Path = file.Path;
            dataOut.Position = buffStream.Position;

            for (int i = 0; i < numberTimesRead; i++)
            {
                byte[] array;
                array = new byte[sizeBufferReader];
                int n;
                // Чтение из файла размерностью sizeBufferReader
                n = await buffStream.ReadAsync(array, 0, sizeBufferReader);
                if (n == 0)
                {
                    return dataOut;
                }

                // Преобразование массива байтов в строку и удаление последних нулей
                dataOut.Buffer += System.Text.Encoding.Default.GetString(array).TrimEnd(new char[] { (char)0 });
                currentNumberRecordedElements = currentNumberRecordedElements + sizeBufferReader;
            }
            return dataOut;
        }

        public void Dispose()
        {
            if (buffStream != null)
            {
                buffStream.Dispose();
            }

        }
    }
}
