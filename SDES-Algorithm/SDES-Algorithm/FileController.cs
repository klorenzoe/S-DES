using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SDES_Algorithm
{
    class FileController
    {

        
        /// <summary>
        /// this method saves the permutations and switch boxes in other file, inside of debug.
        /// it is important that the code is in the beginning
        /// </summary>
        /// <param name="data"></param>
        public static void DataToDecrypth(string data)
        {
            using (var binnacle = Binnacle())
            {
                using (var writer = new StreamWriter(binnacle))
                {
                    writer.WriteLine(data);
                }
            }
        }

        private static FileStream Binnacle()
        {
            if (!File.Exists("Bitácora.txt"))
            {
                var fs = File.Create("Bitácora.txt");
                fs.Dispose();
            }
            return new FileStream("Bitácora.txt", FileMode.Open);
        }

        public static string GetDataToDecrypth(string NameInFile)
        {
            using (var binnacle = Binnacle())
            {
                using (var reader = new StreamReader(binnacle))
                {
                    var line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Split('|')[0] == NameInFile)
                        {
                            return line;
                        }
                    }

                }
            }
            return null;
        }

        public static string GetNewCode()
        {
            //if the code is not exist it creates it.
            if (!File.Exists("UltimoCodigo.txt"))
            {
                using (var code = new FileStream("UltimoCodigo.txt", FileMode.Create))
                {
                    using(var codeWriter = new StreamWriter(code))
                    {
                        codeWriter.Write("1");
                        return "1";
                    }
                }
            }
            else
            {
                //It take the last code used, and overwrite the new last code.
                int newCode = 0;
                using (var code = new FileStream("UltimoCodigo.txt", FileMode.Open))
                {
                    using (var codeReader = new StreamReader(code))
                    {
                        newCode = int.Parse(codeReader.ReadLine());
                    }
                }

                using (var code = new FileStream("UltimoCodigo.txt", FileMode.Create))
                {
                    using (var codeWriter = new StreamWriter(code))
                    {
                        codeWriter.Write(++newCode);
                        return newCode.ToString();
                    }
                }
            }
        }
    }
}
