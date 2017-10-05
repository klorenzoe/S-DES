using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_Algorithm
{
    class SDESMethods
    {
        private string k1 { get; set; }
        private string k2 { get; set; }
        private int[] initialPermutation { get; set; }
        private int[] permutation4 { get; set; }
        private int[] expandAndPermut { get; set; }
        private string[,] switchBox1 { get; set; }
        private string[,] switchBox2 { get; set; }


        //Generate the key
        private void GetKey()
        {

        }

        //Encrypt area
        private int[] GetInitialPermutation()
        {
            var numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 };
            var aleatory = new Random();
            return numbers.OrderBy(x => aleatory.Next()).ToArray();
        }

        private int[] GetPermuation4()
        {
            var numbers = new int[] { 0, 1, 2, 3 };
            var aleatory = new Random();
            return numbers.OrderBy(x => aleatory.Next()).ToArray();
        }

        private int[] GetExpandAndPermut()
        {
            var numbers = new int[] { 0, 1, 2, 3, 0, 1, 2, 3 };
            var aleatory = new Random();
            return numbers.OrderBy(x => aleatory.Next()).ToArray();
        }

        private string[,] GetSwitchBox()
        {
            var switchBox = new String[4, 4];
            var aleatory = new Random();

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    switchBox[x, y] = aleatory.Next(0, 1).ToString() +aleatory.Next(0, 1).ToString();
                }
            }
            return switchBox;
        }

        private string XOR(string valueA, string valueB)
        {
            var result = "";
            for (int i = 0; i < valueA.Length; i++)
            {
                if (valueA[i] == valueB[i])
                    result += "0";
                else
                    result += "1";
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
                while (Permutation[index] != i)
                {
                    index++;
                }
                result += toPermute[index].ToString();
                index = 0;
            }
            return result;
        }

        private string[] Divide(string toDivide)
        {
            var TwoParts = new string[2];
            TwoParts[0] = toDivide.Substring(0, toDivide.Length / 2);
            TwoParts[1] = toDivide.Replace(TwoParts[0], "");
            return TwoParts;
        }

        private string GetSwitchBoxValues(string coordinates, string[,] switchBox)
        {
            var x = Convert.ToInt32((coordinates[1].ToString() + coordinates[2].ToString()), 2);
            var y = Convert.ToInt32((coordinates[0].ToString() + coordinates[3].ToString()), 2);
            return switchBox[x, y];
        }

        private string[] Encryption(string key, string[] result)
        {
            var SecondPart = result[1];
            //Expand and permut the second part of result, after it does the XOR.
            result[1] = Permutate(result[1], expandAndPermut);
            result[1] = XOR(result[1], key);

            //the before result bring me the position for the switch boxes, this result is joined.
            result[1] = GetSwitchBoxValues(Divide(result[1])[0], switchBox1) + GetSwitchBoxValues(Divide(result[1])[1], switchBox2);

            //the before result has a permutation 4
            result[1] = Permutate(result[1], permutation4);

            //the before result has a XOR comparation with the result[0]
            result[0] = XOR(result[0], result[1]);

            // now this results are returned for other process. 
            return new string[] { SecondPart, result[0] };

        }

        /// <summary>
        /// this method excript a group of eight bits.
        /// </summary>
        public string Ciphertext(string eightBits)
        {
            //k1 = GetKey();
            //k2 = GetKey();
            k1 = "10100011";
            k2 = "00111101";
            initialPermutation = GetInitialPermutation();
            //initialPermutation = new int[] { 1, 5, 2, 0, 7, 3, 4, 6 };
            permutation4 = GetPermuation4();
            //permutation4 = new int[] { 0, 3, 1, 2 };
            expandAndPermut = GetExpandAndPermut();
            //expandAndPermut = new int[] { 3, 0, 1, 2, 1, 2, 3, 0 };
            switchBox1 = GetSwitchBox();
            switchBox2 = GetSwitchBox();
            //switchBox1 = new string[4, 4] { { "01", "11", "00", "11" }, { "00", "10", "10", "01" }, { "11", "01", "01", "11" }, { "10", "00", "11", "10" } };
            //switchBox2 = new string[4, 4] { { "00", "10", "11", "10" }, { "01", "00", "00", "01" }, { "10", "01", "01", "00" }, { "11", "11", "00", "11" } };
            //First Phase  (FIRST KEY)

            var result = new string[2];
            //Initial permut and divide in two parts
            result = Divide(Permutate(eightBits, initialPermutation));
            //it does the first encrypted with the first key
            result = Encryption(k1, result);

            //Second Phase (SECOND KEY)

            result = Encryption(k2, result);

            //the before result is joined and it has a inverse permutate, it is important send the result[1] before result[0]
            return InversePermutate(result[1].ToString() + result[0].ToString(),initialPermutation);
        }

        //Desencrypt area

        public string Desencrypted(string eightBits)
        {
            var result = new string[2];

            //Fist phase
            result = Divide(Permutate(eightBits, initialPermutation));
            result = Encryption(k2, result);

            //Second phase
            result = Encryption(k1, result);

            return InversePermutate(result[1].ToString() + result[0].ToString(), initialPermutation);
        }
    }
}
