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
            var originalFileName = ""; //Este lo recuperas del archivo que hay que cifrar.
            var nameOnFile = ""; //Esta va vacía, retorna el nombre con un código, que después me sirve para buscar sus permutaciones en mi registro

            //RECUERDA SIEMPRE LOS VAR, Pablo dijo que bajaría puntos por no usar var
            var newEncryption = new SDESMethods(originalFileName, ref nameOnFile, password);
            //acá llamas al archivo, lees 8 a 8 bits, y se los mandas a Ciphertext.
            //Ciphertext con los primeros 8 te dará un resultado, con los segundos 8 otro... esos los metes en otro archivo.
        }

        public static void Decrypted(string password, string Path)
        {
            var nameOnFile = ""; //Esta va vacía, retorna el nombre con un código, esta dentro de tu archivo a desencriptar. (porque tu la metiste allí)
            //Tiene este formato Junito.txt-1 //si quieres saber el nombre original le das split por -

            var newEncryption = new SDESMethods(nameOnFile, password); //Este constructor solo sirve para desencriptar porque se va directo a buscar la data al archivo.
            //acá llamas al archivo, lees 8 a 8 bits, y se los mandas a Decrypted(string eightBits) 
            //Decrypted con los primeros 8 te dará un resultado, con los segundos 8 otro... esos los metes en otro archivo. (Este es el archivo descifrado)

            //Supongo que la interfaz de Andoni hay que mejorarla despues de estos métodos (:
        }

    }
}
