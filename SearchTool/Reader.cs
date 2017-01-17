using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApplication1
{
    public class Reader : IReader
    {
        private FileStream fileStream;
        private BufferedStream buffStream;
        private int sizeBufferReader;
        private int sizeBufferWritter;
        //private Data data;
        private int currentNumberRecordedElements = 0;
        private int numberTimesRead;
        private File file;

        public Reader(int sizeBufferReader, int sizeBufferWritter, File f)
        {
            this.sizeBufferReader = sizeBufferReader;
            this.sizeBufferWritter = sizeBufferWritter;
            fileStream = new FileStream(f.Path, FileMode.Open);
            Initialize(fileStream, buffStream);
            file = new File(f.Path);
        }

        public void Initialize(Stream fin, BufferedStream buffStream)
        {
                this.buffStream = new BufferedStream(fin, sizeBufferReader);
                currentNumberRecordedElements = 0;
        }

        public bool Read(out Data dataOut)
        {
            numberTimesRead = sizeBufferWritter / sizeBufferReader;

            dataOut = new Data();
            dataOut.Path = file.Path;
            dataOut.Position = buffStream.Position;

            for (int i = 0; i < numberTimesRead; i++)
            {
                //if ((currentNumberRecordedElements + sizeBufferReader) > sizeBufferWritter)
                //{
                //    currentNumberRecordedElements = 0;
                //    dataOut = data;
                //    data.Recognition = true;
                //    return true;
                //}

                byte[] array;
                array = new byte[sizeBufferReader];
                int n;

                //if (data.Recognition == true)
                //{
                //    data.Position = buffStream.Position;

                //    data.Buffer = String.Empty;
                //    data.Recognition = false;
                //}
                n = buffStream.Read(array, 0, sizeBufferReader);

                if (n == 0)
                {
                    
                    return false;
                }

                dataOut.Buffer += System.Text.Encoding.Default.GetString(array).TrimEnd(new char[] { (char)0 });
                currentNumberRecordedElements = currentNumberRecordedElements + sizeBufferReader;
            }
            return true;

            //int offset = 0;

            //long length = buffStream.Length;
            //StringBuilder readTheText = new StringBuilder();
            //while (length > 0)
            //{

            //    byte[] array;
            //    if (sizeBufferReader <= length)
            //    {
            //        array = new byte[sizeBufferReader];
            //        int n = buffStream.Read(array,0, sizeBufferReader);
            //    }
            //    else
            //    {
            //        array = new byte[length];
            //        int n = buffStream.Read(array, 0, (int)length);
            //    }
            //    offset += sizeBufferReader;
            //    length -= sizeBufferReader;
            //    readTheText.Append(System.Text.Encoding.Default.GetString(array));
            //}

            //data.value = readTheText.ToString();
            //return data;
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            ////fin.Read()

            //    fstr_in = new StreamReader(fin);
            //    data.value = fstr_in.ReadLine();
            //return data; //поискать что-нибудь другое (именно для чтения файла)
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
