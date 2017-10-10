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
        private int[] tenBitsKey = new int[10];
        private int[] lastfive = new int[5];
        private int[] firstFive = new int[5];
        private int[] permutation8 { get; set; }
        int[] permutation10 { get; set; }
        int[] toPermutation8K1 { get; set; }
        int[] toPermutation8K2 { get; set; }
        //others propieties
        Random binaryAleatory;


        //Generate the key
        private void GetKey(string password)
        {
            Get10BitKey(password);
            permutation10 = Permutation10();
            GetFiveBitsKey(permutation10);
            firstFive =  LeftShift(firstFive);
            lastfive = LeftShift(lastfive);
            GetPermutation8();
            toPermutation8K1 = Merge();
            k1 = GetPermutationValue(toPermutation8K1);
            firstFive = LeftShift(LeftShift(firstFive));
            lastfive = LeftShift(LeftShift(lastfive));
            toPermutation8K2 = Merge();
            k2 = GetPermutationValue(toPermutation8K2);
            
        }

        private int[] Permutation10()
        {
            var numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var aleatory = new Random();
            return numbers.OrderBy(x => aleatory.Next()).ToArray();
        }

        private void GetFiveBitsKey(int[] tenBitKey)
        {
            for (int i = 0; i < 10; i++)
            {
                if (i < 5)
                    firstFive[i] = tenBitKey[i];
                else
                    lastfive[i - 5] = tenBitKey[i];
            }
        }

        private void GetPermutation8()
        {
            var numbers = new int[] { 0, 1, 2, 3, 4, 5, 6, 7};
            var aleatory = new Random();
            permutation8 = numbers.OrderBy(x => aleatory.Next()).ToArray();
        }

        private string GetPermutationValue(int [] toPermutation)
        {
            int[] result = new int[8];
            for (int i = 0; i < 8; i++)
            {
                result[i] = toPermutation[permutation8[i]];
            }
            return GetRealKeyValues(result);
        }

        private string GetRealKeyValues(int[] orden)
        {
            var result = "";
            for (int i = 0; i < 8; i++)
            {
                result += tenBitsKey[orden[i]];
            }
            return result;
        }

        private void Get10BitKey(string password)
        {
            var aleatroy = new Random();
            var result = "";
            foreach (char character in password)
                result += Convert.ToString((int)character, 2);

            for (int i = 0; i < 10; i++)
            {
                tenBitsKey[i] = Convert.ToInt32(result.ToCharArray()[aleatroy.Next(0, result.ToCharArray().Length-15)].ToString());
            }
        } 

        private int[] LeftShift(int[] key)
        {
            int temporal = key[0];
            for (int i = 0; i < 4; i++)
            {
                key[i] = key[i + 1];
            }
            key[4] = temporal;
            return key;
        }

        private int[] Merge()
        {
            var aleatroy = new Random();
            int delete = aleatroy.Next(0, 4), j = 0;
            int[] key = new int[8];
            for (int i = 0; i < 5; i++)
            {
                if (i != delete)
                {
                    key[j] = firstFive[i];
                    key[j++] = lastfive[i];
                }
            }
            return key;
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

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    switchBox[x, y] = binaryAleatory.Next(0, 2).ToString() + binaryAleatory.Next(0, 2).ToString();
                }
            }
            return switchBox;
        }

        public SDESMethods(string originalFileName, ref string nameInFile, string password)
        {
            binaryAleatory = new Random();
            GetKey(password);
            //k1 = "10010111";
            //k2 = "01010010";
            initialPermutation = GetInitialPermutation();
            permutation4 = GetPermuation4();
            expandAndPermut = GetExpandAndPermut();
            switchBox1 = GetSwitchBox();
            switchBox2 = GetSwitchBox();

            //this parth saves the important tools for after Decrypt.
            nameInFile = SaveToolsForDecrypt(originalFileName);
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

        public string Desencrypted(string eightBits, string NameInFile)
        {
            GetToolsForDecrypt(NameInFile);
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
            FileController.DataToDecrypth(newName + "|" + ArrayValuesToString(initialPermutation) + "|" + ArrayValuesToString(expandAndPermut) + "|" + MatrixValuesToString(switchBox1) + "|" + MatrixValuesToString(switchBox2));
            return newName;
        }

        private void GetToolsForDecrypt(string NameInFile)
        {
            var data = FileController.GetDataToDecrypth(NameInFile).Split('|');
            initialPermutation = StringToIntArray(data[1]);
            expandAndPermut = StringToIntArray(data[2]);
            switchBox1 = StringToStringMatrix(data[3]);
            switchBox2 = StringToStringMatrix(data[4]);
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
