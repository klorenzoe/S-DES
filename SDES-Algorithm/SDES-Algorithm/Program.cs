using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("SIMPLE DES Encryption");
                Console.WriteLine("***Para encriptar se le pedirá una llave, para desencriptar se le solicitará la que usó, si no, le dará un resultado erróneo");
                Console.WriteLine("\ningrese petición: ");
                try
                {
                    string[] Instructions = Console.ReadLine().Trim().Split();
                    var answer = "";

                    switch (Instructions[0])
                    {
                        case "-c":
                            answer = "";
                            if (Instructions[1] == "-f")
                            {
                                
                                do
                                {
                                    Console.WriteLine("Ingrese una clave para la encripción: ");
                                    answer = Console.ReadLine();
                                } while (answer == "");
                                FileController.Encrypted(answer, Path(Instructions));
                            }
                            else
                            {
                                Console.WriteLine("Sintaxis no válida\nEscriba --ayuda para obtener ayuda");
                            }
                               
                            break;
                        case "-d":
                            answer = "";

                            if (Instructions[1] == "-f")
                            {
                                do
                                {
                                    Console.WriteLine("Ingrese la clave para la desencripción: ");
                                    answer = Console.ReadLine();
                                } while (answer == "");
                                var path = Path(Instructions);

                                if (path.Split('.')[path.Split('.').Length - 1] == "cif")
                                    FileController.Decrypted(answer, path);
                                else
                                {
                                    Console.WriteLine("El archivo a descifrar tiene que ser .cif, es decir, el original cifrado");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Sintaxis no válida\nEscriba --ayuda para obtener ayuda");
                            }
                            
                            break;
                        case "--ayuda":
                            Console.WriteLine("Encriptar -c <palabra clave>\nDesenciptar <palabra clave>-d\nRuta de archivo -f\nAyuda --ayuda");
                            Console.WriteLine("Ejemplo ayuda: -c -f Path\\Texto.txt");
                            break;
                        default:
                            Console.WriteLine("Comando no válido.\nEscriba --ayuda para obtener ayuda");
                            break;
                    }
                    Console.WriteLine("ESTADO DE LA PETICIÓN: Terminada");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Sintaxis no válida\nEscriba --ayuda para obtener ayuda");
                    Console.Clear();
                }
            }
        }

        private static string Path(string[] InstructionArray)
        {
            var path = "";
            for (int i = 2; i < InstructionArray.Length; i++)
            {
                path += InstructionArray[i]+" ";
            }
            return path.Trim();
        }
    }
}
