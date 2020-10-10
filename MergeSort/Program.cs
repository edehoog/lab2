using System;
using System.Linq;
using System.Diagnostics;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {

            int ARRAY_SIZE = 1000;
            int[] arraySingleThread = new int[ARRAY_SIZE];
            int[] arrayMultiThread = new int[ARRAY_SIZE];

            var randomNumber = new Random();

            for(var i = 0; i<ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = randomNumber.Next(0, 1000);
            }

            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            arraySingleThread = MergeSort(arraySingleThread);

            bool sorted = IsSorted(arraySingleThread);

            stopwatch.Stop();

            long duration = stopwatch.ElapsedMilliseconds;

            string formatedTime = String.Format("Miliseconds:{0}", duration);

            Console.WriteLine("Runtime: " + formatedTime);

            Console.WriteLine(sorted);

            PrintArray(arraySingleThread);

            //TODO: Multi Threading Merge Sort

            /*********************** Methods **********************
             *****************************************************/
            /*
            implement Merge method. This method takes two sorted array and
            and constructs a sorted array in the size of combined arrays
            */

            static int[] Merge(int[] LA, int[] RA, int[] A)
            {
                int lengthLA = LA.Length;
                int lengthRA = RA.Length;
                int i = 0;
                int j = 0;
                int k = 0;

                while( i< lengthLA && j < lengthRA)
                {
                    if(LA[i] <= RA[j])
                    {
                        A[k] = LA[i];
                        k++;
                        i++;
                    }
                    else
                    {
                        A[k] = RA[j];
                        k++;
                        j++;
                    }
                }
                while(i < lengthLA)
                {
                    A[k] = LA[i];
                    i++;
                    k++;
                }
                while (j < lengthRA)
                {
                    A[k] = RA[j];
                    j++;
                    k++;
                }

                return A;

            }
            /*
            implement MergeSort method: takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            */
            static int[] MergeSort(int[] A)
            {
                int lengthA = A.Length;
                if(lengthA < 2)
                {
                    return A;
                }
                int mid = lengthA / 2;

                int[] LA = new int[mid];
                int[] RA = new int[lengthA - mid];

                for(int i = 0; i< mid; i++)
                {
                    LA[i] = A[i];
                }
                for (int i = mid; i < lengthA; i++)
                {
                    RA[i - mid] = A[i];
                }
                MergeSort(LA);
                MergeSort(RA);
                Merge(LA, RA, A);

                return A;
            }
            // a helper function to print your array
            static void PrintArray(int[] myArray)
            {
                Console.Write("[");
                for (int i = 0; i < myArray.Length; i++)
                {
                    Console.Write("{0} ", myArray[i]);

                }
                Console.Write("]");
                Console.WriteLine();

            }

            // a helper function to confirm your array is sorted
            // returns boolean True if the array is sorted
            static bool IsSorted(int[] a)
            {
                int j = a.Length - 1;
                if (j < 1) return true;
                int ai = a[0], i = 1;
                while (i <= j && ai <= (ai = a[i])) i++;
                return i > j;
            }


        }


    }
}
