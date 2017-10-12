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
                    writer.BaseStream.Position = writer.BaseStream.Length;
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
            return "";
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


        //methods for Encrypt or Decrypt a FILE

        public static void Encrypted(string password, string Path)
        {
            var nameOnFile = "";

            var newEncryption = new SDESMethods(Path.Split('\\')[Path.Split('\\').Length-1], ref nameOnFile, password);

            using (var file = new FileStream(Path, FileMode.Open))
            {
                using (var text = new BinaryReader(file))
                {
                    var name = Path.Split('\\');
                    name[name.Length - 1] = name[name.Length - 1].Split('.')[0] + ".cif";
                    var salida = string.Join("\\", name);
                    var eightBits="";

                    using (var fileToWrite = new FileStream(salida, FileMode.Create))
                    {
                        using (var toWrite = new BinaryWriter(fileToWrite, Encoding.ASCII))
                        {
                            toWrite.Write("::"+nameOnFile + "||");
                            for (int i = 0; i < text.BaseStream.Length; i++)
                            {
                                eightBits = Convert.ToString(text.ReadByte(), 2).PadLeft(8, '0');
                                toWrite.Write(Convert.ToByte(newEncryption.Ciphertext(eightBits), 2));
                            }
                        }
                    }
                }
            }
        }

        public static void Decrypted(string password, string Path)
        {
            try
            {
                var OriginalName = "";
                var NameOnFile = "";
                var eightBits = "";
                using (var fileForRead = new FileStream(Path, FileMode.Open))
                {
                    using(var text = new BinaryReader(fileForRead))
                    {
                        ReadNames(text, ref OriginalName, ref NameOnFile);
                        var name = Path.Split('\\');
                        name[name.Length - 1] = OriginalName;
                        var salida = string.Join("\\", name);

                        var newDecryption = new SDESMethods(NameOnFile, password);
                        using (var fileForWrite = new FileStream(salida, FileMode.Create))
                        {
                            using (var toWrite = new BinaryWriter(fileForWrite, Encoding.GetEncoding("iso-8859-1")))
                            {
                                text.BaseStream.Seek(NameOnFile.Length+5, SeekOrigin.Begin);
                                for (int i = NameOnFile.Length+5; i < text.BaseStream.Length; i++)
                                {
                                    eightBits = Convert.ToString(text.ReadByte(), 2).PadLeft(8, '0');
                                    toWrite.Write(Convert.ToByte(newDecryption.Decrypted(eightBits), 2));
                                }
                            }
                        }
                    }
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Dicho archivo no esta cifrado con SDES");
            }
        }

        private static void ReadNames(BinaryReader text, ref string OriginalName, ref string NameOnFile)
        {
            var chain = "";
            for (int i = 0; i < text.BaseStream.Length; i++)
            {
                var c = text.ReadByte();
                chain += Convert.ToChar(c).ToString();

                if (chain.Contains("||"))
                {
                    break;
                }
            }
            var Names = chain.Split(new[] { "||" }, StringSplitOptions.None)[0];
            OriginalName = Names.Split(new[] { "::" }, StringSplitOptions.None)[1];
            NameOnFile = OriginalName+"::"+Names.Split(new[] { "::" }, StringSplitOptions.None)[2];

        }

    }
}
