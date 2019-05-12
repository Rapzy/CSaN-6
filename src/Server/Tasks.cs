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
        public static byte[] SendBlock(string fileName, int offset, int length)
        {
            try
            {
                using (FileStream fsSource = File.OpenRead(@"C:\Files\Server\" + fileName))
                {
                    if (length > fsSource.Length | length < 0)
                        length = (int)fsSource.Length;
                    byte[] result = new byte[length];
                    try
                    {
                        fsSource.Seek(offset, SeekOrigin.Begin);
                        fsSource.Read(result, 0, length);
                        return result;
                    }
                    catch
                    {
                        Console.WriteLine("Ошибка чтения из файла");
                        return null;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден");
                return null;
            }
        }
    }
}
