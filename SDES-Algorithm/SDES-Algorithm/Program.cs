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
            string name = "", nemeInFile = "";
            
            string[] entrada = Console.ReadLine().Split(' ');
            int current = CaracterValido(0, entrada);


            switch (entrada[current])
            {
                case "-c":
                    Encrypt(entrada, current, name);
                    break;
                case "-d":
                    Decrypt(entrada, current, name, nemeInFile);
                    break;
                case "--ayuda":
                    Console.WriteLine("Encriptar -c <palabra clave>\nDesenciptar <palabra clave>-d\nRuta de archivo -f\nAyuda --ayuda");
                    break;
                default:
                    Console.WriteLine("Comando no válido.\nEscriba --ayuda para obtener ayuda");
                    break;
            }
            Console.ReadKey();
        }

        static public int CaracterValido(int posicion, string[] entrada)
        {
            if (entrada[posicion] == "")
                return CaracterValido(posicion + 1, entrada);
            return posicion;
        }

        static public void Encrypt(string[] entrada, int current, string name)
        {
            string password;
            var eightBits = "";
            current = CaracterValido(current++, entrada);
            password = entrada[current].Trim();
            if (password == null)
                Console.WriteLine("Sintaxis no válida\nEscriba --ayuda para obtener ayuda");
            else
            {
                SDESMethods Encryption = new SDESMethods("SDES", ref name, password);
                //Encryption.Ciphertext(eightBits);
            }
        }

        static public void Decrypt(string[] entrada, int current, string name, string nameInFile)
        {
            string password;
            var eightBits = "";
            current = CaracterValido(current++, entrada);
            password = entrada[current].Trim();
            if(password == null)
                Console.WriteLine("Sintaxis no válida\nEscriba --ayuda para obtener ayuda");
            else
            {
                SDESMethods Encryption = new SDESMethods("SDES", ref name, password);
                Encryption.Desencrypted(eightBits, nameInFile);
            }
        }
    }
}
