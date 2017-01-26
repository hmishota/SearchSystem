using System.IO;
using System.Threading.Tasks;
using SearchTool.Interfaces;

namespace SearchTool
{
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
                buffStream.Close();

            if (fileStream != null)
                fileStream.Close();
        }
    }
}
