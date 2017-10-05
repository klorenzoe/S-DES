using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_Algorithm
{
    class SDES_methods
    {
        public var k1 = "10100011";
        public var k2 = "00111101";
        public var initialPermutation { get; set; }
        public var permutation4 { get; set; }
        public var expandAndPermut { get; set; }
        public var switchBox1 { get; set; }
        public var switchBox2 { get; set; }


        //Generate the key
        private void GetKey()
        {

        }

        //Encrypt area
        private int[] GetInitialPermutation()
        {
            var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            var aleatory = new Random();
            return numbers.OrderBy(x => rnd.Next()).ToArray();
        }
        
        private int[] GetPermuation4()
        {
            var numbers = new int[] { 1, 2, 3, 4};
            var aleatory = new Random();
            return numbers.OrderBy(x => rnd.Next()).ToArray();
        }

        private int[] GetExpandAndPermut()
        {
            var numbers = new int[] { 1, 2, 3, 4, 1, 2, 3, 4 };
            var aleatory = new Random();
            return numbers.OrderBy(x => rnd.Next()).ToArray();
        }

        private Array[] GetSwitchBox()
        {
            var switchBox = new String[4, 4];
            var aleatory = new Random();

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    switchBox[x, y] = aleatory.Next(0, 1).ToString(); +aleatory.Next(0, 1).ToString();
                }
            }
            return switchBox;
        }

        private string XOR(string expandAndPermut, string key)
        {
            var result = "";
            for (int i = 0; i < 8; i++)
            {
                ExpandAndPermut[i].Equals(key[i]) ? result += "0" : result += "1";
            }
            return result;
        }

        private string Permutate(string toPermute, int[] Permutation)
        {
            var result = "";
            for (int i = 0; i < Permutation.Length; i++)
            {
                result += toPermute[Permutation[i]];
            }
            return result;
        }

        private string InversePermutate(string toPermute, int[] Permutation)
        {
            var result = "";
            var index = 0;
            for (int i = 0; i < Permutation.Length; i++)
            {
                Permutation[index] == i ? (result += toPermute[index]) : index++;
            }
            return result;
        }

        private string[] Divide(string toDivide)
        {
            var TwoParts = new string[2]();
            TwoParts[0] = toDivide.Substring(0, toDivide.Length / 2);
            TwoParts[1] = toDivide.Substring(toDivide.Length / 2, toDivide.Length);
            return TwoParts;
        }

        private string GetSwitchBoxValues(string coordinates, Array[] switchBox)
        {
            var x = Convert.ToInt32((coordinates[1].ToString()+coordinates[2].ToString()), 2);
            var y = Convert.ToInt32((coordinates[0].ToString() + coordinates[3].ToString()), 2);
            return switchBox[x,y];
        }

        private string[] Encryption(string key, string[] result)
        {
            var SecondPart = result[1];
            //Expand and permut the second part of result, after it does the XOR.
            result[1] = Permutate(result[1], expandAndPermut);
            result[1] = XOR(result[1],key);

            //the before result bring me the position for the switch boxes, this result is joined.
            result[1] = GetSwitchBoxValues(Divide(result[1])[0], switchBox1) + GetSwitchBoxValues(Divide(result[1])[1], switchBox2);

            //the before result has a permutation 4
            result[1] = Permutate(result[1], permutation4);

            //the before result has a XOR comparation with the result[0]
            result[0] = XOR(result[0],result[1]);

            // now this results are returned for other process. 
            return new string[] { SecondPart, result[0]};

        }

        /// <summary>
        /// this method excript a group of eight bits.
        /// </summary>
        private void Ciphertext(string eightBits)
        {
            //First Phase  (FIRST KEY)

            var result = new string[2];
            //Initial permut and divide in two parts
            result = Divide(Permutate(eightBits, initialPermutation));
            //it does the first encrypted with the first key
            result = Encryption(k1, result);

            //Second Phase (SECOND KEY)
            
            result = Encryption(k2, result);

            //the before result is joined and it has a inverse permutate
            return InversePermutate(result[0].ToString()+result[1].ToString());
        }

        //Desencrypt area

    }
}
