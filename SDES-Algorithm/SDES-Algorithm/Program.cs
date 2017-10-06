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
            string name = "";

            SDESMethods Encryption = new SDESMethods("SDES", ref name);
            var n = Encryption.Ciphertext("11100010");
            var d = Encryption.Desencrypted(n,name);
        }
    }
}
