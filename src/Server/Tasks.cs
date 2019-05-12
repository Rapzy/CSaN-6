using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
    public class Tasks
    {
        public static void SendBlock(string fileName, int offset, int length)
        {
            try
            {
                using (FileStream fsSource = File.OpenRead(@"C:\Files\" + fileName))
                {
                    if (length > fsSource.Length | length < 0)
                        length = (int)fsSource.Length;
                    byte[] array = new byte[length];
                    try
                    {
                        fsSource.Seek(offset, SeekOrigin.Begin);
                        fsSource.Read(array, 0, length);
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка чтения из файла");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
            }
        }
    }
}
