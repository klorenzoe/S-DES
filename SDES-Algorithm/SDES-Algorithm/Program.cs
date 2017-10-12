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


                    switch (Instructions[0])
                    {
                        case "-c":
                            var answer = "";
                            do
                            {
                                Console.WriteLine("Ingrese una clave para la encripción (Es necesaria para desencryptar): ");
                                answer = Console.ReadLine();
                            } while (answer=="");
                            FileController.Encrypted(answer, Path(Instructions));
                            Console.WriteLine("ESTADO DE LA PETICIÓN: Terminada");
                            break;
                        case "-d":
                            answer = "";
                            do
                            {
                                Console.WriteLine("Ingrese la clave para la desencripción: ");
                                answer = Console.ReadLine();
                            } while (answer == "");
                            FileController.Decrypted(answer, Path(Instructions));
                            Console.WriteLine("ESTADO DE LA PETICIÓN: Terminada");
                            break;
                        case "--ayuda":
                            Console.WriteLine("Encriptar -c <palabra clave>\nDesenciptar <palabra clave>-d\nRuta de archivo -f\nAyuda --ayuda");
                            break;
                        default:
                            Console.WriteLine("Comando no válido.\nEscriba --ayuda para obtener ayuda");
                            break;
                    }
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
