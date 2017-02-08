using System.IO;
using System.Threading.Tasks;
using SearchTool.Interfaces;
using SearchTool.Models;
using System.Diagnostics;

namespace SearchTool
{
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

        public long InitVariables(Stream stream, int sizeBufferReader, int sizeBufferWritter)
        {
            return _reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
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

        public long InitVariables(Stream stream, int sizeBufferReader, int sizeBufferWritter)
        {
            return _reader.InitVariables(stream, sizeBufferReader, sizeBufferWritter);
        }

        public void ReaderAddTime()
        {
            ResultTime.queryListRead.Enqueue(_getStopWatch.ElapsedMilliseconds);
        }

        public async Task<Data> ReadAsync()
        {
            _getStopWatch.Start();
            var read = await _reader.ReadAsync();
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
        private BufferedStream _buffStream;
        private int _sizeBufferReader;
        private int _sizeBufferWritter;
        private int _currentNumberRecordedElements = 0;
        private int _numberTimesRead;

        public long InitVariables(Stream stream, int sizeBufferReader, int sizeBufferWritter)
        {
            _sizeBufferReader = sizeBufferReader;
            _sizeBufferWritter = sizeBufferWritter;
            _buffStream = new BufferedStream(stream, sizeBufferReader);
            _currentNumberRecordedElements = 0;
            return _buffStream.Position;
        }

        public async Task<Models.Data> ReadAsync()
        {
            var dataOut = new Models.Data();
            // Проверка на конец файла
            if (_buffStream.Position == _buffStream.Length)
                return null;

            _numberTimesRead = _sizeBufferWritter / _sizeBufferReader;

            dataOut.Position = _buffStream.Position;
            byte[] array;
            int n;

            for (var i = 0; i < _numberTimesRead; i++)
            {
                array = new byte[_sizeBufferReader];
                // Чтение из файла размерностью sizeBufferReader
                n = await _buffStream.ReadAsync(array, 0, _sizeBufferReader);
                if (n == 0)
                {
                    return dataOut;
                }

                // Преобразование массива байтов в строку и удаление последних нулей
                dataOut.Buffer += System.Text.Encoding.Default.GetString(array).TrimEnd(new char[] { (char)0 });
                _currentNumberRecordedElements = _currentNumberRecordedElements + _sizeBufferReader;
            }
            return dataOut;
        }

        public void Dispose()
        {
            if (_buffStream != null)
            {
                _buffStream.Dispose();
            }

        }
    }
}
