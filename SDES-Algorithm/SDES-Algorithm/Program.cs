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
            string name = "", password = "Hola";

            SDESMethods Encryption = new SDESMethods("SDES", ref name, password);
            var n = Encryption.Ciphertext("11100010");
            var d = Encryption.Desencrypted(n,name);

            n = Encryption.Ciphertext("11111111");
            d = Encryption.Desencrypted(n, name);

            n = Encryption.Ciphertext("00001111");
            d = Encryption.Desencrypted(n, name);
        }
    }
}
