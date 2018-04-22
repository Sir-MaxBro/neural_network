using HashGenerator.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashGenerator.Tests
{
    internal class HashGenTest
    {
        private HashGen _hashGen;
        private readonly int _step;
        public HashGenTest(HashGen hashGen)
        {
            _hashGen = hashGen;
            _step = _hashGen.Step;
        }

        public bool CheckHashGen()
        {
            var hashList = _hashGen.Generate();
            //bool isCheck = false;
            IList<double> sums = new List<double>();

            long[] hashArray = new long[hashList.Count];   
            int index = 0;
            foreach (DictionaryEntry item in hashList)
            {
                hashArray[index] = (long)item.Key;
                index++;
            }
            Array.Sort(hashArray);

            for (int i = 1; i <= hashList.Count; i++)
            {
                int chooseCount = i;
                int allCount = hashList.Count; 

                //Print(hashArray, chooseCount);
                if (allCount >= chooseCount)
                {
                    while (NextSet(hashArray, allCount, chooseCount))
                    {
                        //Print(hashArray, chooseCount);
                        var sum = hashArray.Take(chooseCount).Sum();
                        sums.Add(sum);
                        Console.WriteLine(sum);
                    }
                }
            }
            long countBefore = sums.Count;
            long countAfter = sums.Distinct().Count();

            return countAfter == countBefore;
        }

        bool NextSet(long[] hashArray, int allCount, int chooseCount)
        {
            int k = chooseCount;
            for (int i = k - 1; i >= 0; i--)
            {
                if (hashArray[i] < hashArray[allCount - k + i])
                {
                    hashArray[i] += _step;
                    for (int j = i + 1; j < k; ++j)
                    {
                        hashArray[j] = hashArray[j - 1] + _step;
                    }
                    return true;
                }
            }
            return false;
        }

        void Print(long[] hashArray, int chooseCount)
        {
            int num = 1;
            Console.WriteLine(num++ + ": ");
            for (int i = 0; i < chooseCount; i++)
            {
                Console.WriteLine(hashArray[i] + " ");
            }
        }
    }
}
