using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Logic
{
    public abstract class Observer
    {
        public abstract bool handle(string text);
    }

    public class ConsoleLog : Observer
    {
        public override bool handle(string text)
        {
            bool result = false;

            try
            {
                Console.WriteLine(text);
                result = true;
            } catch(Exception exp)
            {
                Console.WriteLine("Ошибка записи в консоль!", exp);
            }

            return result;
        }
    }

    public class FileLog : Observer
    {
        private string fileName;

        public FileLog(string fileName)
        {
            this.fileName = fileName;
        }

        public override bool handle(string text)
        {
            bool result = false;

            string newText = DateTime.Now + ": " + text;
            try
            {

                using (FileStream fstream = new FileStream(this.fileName, FileMode.Append))
                {
                    // преобразуем строку в байты
                    byte[] array = System.Text.Encoding.Default.GetBytes(newText + '\n');
                    // запись массива байтов в файл
                    fstream.Write(array, 0, array.Length);
                    result = true;
                }
            } catch(Exception exp)
            {
                Console.WriteLine("Ошибка записи файла!", exp);
            }

            return result;
        }
    }

}
