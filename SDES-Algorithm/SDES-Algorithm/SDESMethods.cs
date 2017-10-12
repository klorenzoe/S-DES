using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDES_Algorithm
{
    class SDESMethods
    {
        //for keys
        private string k1 { get; set; }
        private string k2 { get; set; }
        private int[] permutation10 { get; set; }
        private int[] permutation8 { get; set; }
        private int[] permutationInitialKey { get; set; }

        //for encryption
        private int[] initialPermutation { get; set; }
        private int[] permutation4 { get; set; }
        private int[] expandAndPermut { get; set; }
        private string[,] switchBox1 { get; set; }
        private string[,] switchBox2 { get; set; }


        //others propieties
        Random binaryAleatory { get; set; }
        Random Order { get; set; }


        //Generate the key
        private void GetKeys(string password)
        {
            var tenBits = GetThenBits(password);
            tenBits = Permutate(tenBits, permutation10);
            var FirstFive = LeftShift(GetFiveBitsKey(tenBits, 1));
            var SecondFive = LeftShift(GetFiveBitsKey(tenBits, 2));
            k1 = Permutate(FirstFive + SecondFive , permutation8);
            k2 = Permutate(LeftShift(FirstFive) + LeftShift(SecondFive), permutation8);
        }

        private string GetThenBits(string password)
        {
            var bytes = "";
            password = Permutate(password, permutationInitialKey);

            while(bytes.Length < 10)
            {
                foreach (char ch in password)
                {
                    bytes += Convert.ToString((int)ch, 2)[3];
                }
            }
            return bytes.Substring(0, 10);
        }

        private string GetFiveBitsKey(string tenBitKey, int firstOrSecond)
        {
            return firstOrSecond == 1 ? tenBitKey.Substring(0, 5) : tenBitKey.Substring(4, 5);
        }

        private string LeftShift(string key)
        {
            var temporal = key[0].ToString();
            var LS = "";
            for (int i = 0; i < 4; i++)
            {
                LS += key[i + 1].ToString();
            }
            LS += temporal;
            return LS;
        }

        //Encrypt area
        private int[] GetInitialPermutation()
        {
            return GetAPermutation(8);
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

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    switchBox[x, y] = binaryAleatory.Next(0, 2).ToString() + binaryAleatory.Next(0, 2).ToString();
                }
            }
            return switchBox;
        }

        /// <summary>
        /// it assigns new tools for the encrypt
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <param name="nameOnFile"></param>
        /// <param name="password"></param>
        public SDESMethods(string originalFileName, ref string nameOnFile, string password)
        {
            binaryAleatory = new Random();
            Order = new Random();
            initialPermutation = GetInitialPermutation();
            permutation4 = GetAPermutation(4);
            expandAndPermut = GetExpandAndPermut();
            switchBox1 = GetSwitchBox();
            switchBox2 = GetSwitchBox();
            permutation10 = GetAPermutation(10);
            permutation8 = GetAPermutation(8);
            permutationInitialKey = GetAPermutation(password.Length);
            GetKeys(password);

            //this parth saves the important tools for after Decrypt.
            nameOnFile = SaveToolsForDecrypt(originalFileName);
        }

        /// <summary>
        /// this is only for decrypt, seachs in the file the tools.
        /// </summary>
        /// <param name="nameOnFile"></param>
        /// <param name="password"></param>
        public SDESMethods(string nameOnFile, string password)
        {
            GetToolsForDecrypt(nameOnFile, password);
        }

        private string XOR(string valueA, string valueB)
        {
            var result = "";
            for (int i = 0; i < valueA.Length; i++)
            {
                result += (int.Parse(valueA[i].ToString()) ^ int.Parse(valueB[i].ToString())).ToString();
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
            TwoParts[0] = toDivide.Substring(0, 4);
            TwoParts[1] = toDivide.Substring(toDivide.Length / 2, 4);
            return TwoParts;
        }

        private string GetSwitchBoxValues(string coordinates, string[,] switchBox)
        {
            var x = Convert.ToInt32((coordinates[1].ToString() + coordinates[2].ToString()), 2);
            var y = Convert.ToInt32((coordinates[0].ToString() + coordinates[3].ToString()), 2);
            return switchBox[x, y];
        }

        /// <summary>
        /// this method retuns aleatory permutations.
        /// </summary>
        /// <param name="HowMany"></param>
        /// <returns></returns>
        private int[] GetAPermutation(int HowMany)
        {
            int[] permutation = new int[HowMany];
            for (int i = 0; i < HowMany; i++)
            {
                permutation[i] = i;
            }
            return permutation.OrderBy(x => Order.Next()).ToArray();
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

        //Decrypt area

        public string Decrypted(string eightBits)
        {
            //GetToolsForDecrypt(NameInFile, password);
            var result = new string[2];
            //Fist phase
            result = Divide(Permutate(eightBits, initialPermutation));
            result = Encryption(k2, result);

            //Second phase
            result = Encryption(k1, result);

            return InversePermutate(result[1].ToString() + result[0].ToString(), initialPermutation);
        }

        //Files area

        private string SaveToolsForDecrypt(string OriginalName)
        {
            var newName = OriginalName + "::" + FileController.GetNewCode();
            FileController.DataToDecrypth(newName + "|" + ArrayValuesToString(initialPermutation) + "|" + ArrayValuesToString(expandAndPermut) + "|" + MatrixValuesToString(switchBox1) + "|" + MatrixValuesToString(switchBox2)+"|"+ArrayValuesToString(permutation4)+"|"+ArrayValuesToString(permutation10)+"|"+ArrayValuesToString(permutation8)+"|"+ArrayValuesToString(permutationInitialKey));
            return newName;
        }

        public bool GetToolsForDecrypt(string NameOnFile, string password)
        {
            try
            {
                var data = FileController.GetDataToDecrypth(NameOnFile).Split('|');
                initialPermutation = StringToIntArray(data[1]);
                expandAndPermut = StringToIntArray(data[2]);
                switchBox1 = StringToStringMatrix(data[3]);
                switchBox2 = StringToStringMatrix(data[4]);
                permutation4 = StringToIntArray(data[5]);
                permutation10 = StringToIntArray(data[6]);
                permutation8 = StringToIntArray(data[7]);
                permutationInitialKey = StringToIntArray(data[8]);
                GetKeys(password);
                return true;
            }catch(Exception e)
            {
                return false;
            }
            
        }

        private string ArrayValuesToString(int[] values)
        {
            var result = "";
            for (var i = 0; i < values.Length; i++)
            {
                result += values[i];
            }
            return result;
        }

        private string MatrixValuesToString(string[,] matrix)
        {
            var result = "";
            for (var y = 0; y < 4; y++)
            {
                for (var x = 0; x < 4; x++)
                {
                    result += matrix[x, y] + " ";
                }
                result += ":";
            }
            return result;
        }

        private int[] StringToIntArray(string arrayStr)
        {
            var intArray = new int[arrayStr.Length];
            for (int i = 0; i < arrayStr.Length; i++)
            {
                intArray[i] = int.Parse(arrayStr[i].ToString());
            }
            return intArray;
        }

        private string[,] StringToStringMatrix(string matrixStr)
        {
            var matrix = new string[4, 4];
            var rows = matrixStr.Split(':');

            for (int r = 0; r < 4; r++)
            {
                var columns = rows[r].Split();
                for (int c = 0; c < 4; c++)
                {
                    matrix[c, r] = columns[c];
                }
            }
            return matrix;
        }
    }
}
