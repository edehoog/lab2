using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;
using System.Linq.Expressions;
using System.Collections.Immutable;
using System.Globalization;

namespace MergeSort
{
    class Program
    {
        static void Main(string[] args)
        {
            int ARRAY_SIZE = Convert.ToInt32(Math.Pow(10,6));
            int[] arraySingleThread = new int[ARRAY_SIZE];
            int[] arrayMultiThread = new int[ARRAY_SIZE];
            var randomNumber = new Random();

            for(var i = 0; i < ARRAY_SIZE; i++)
            {
                arraySingleThread[i] = randomNumber.Next(0, 1000);
            }

            Array.Copy(arraySingleThread, arrayMultiThread, arraySingleThread.Length);

            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine("Multi-threaded:");

            stopwatch.Start();
            arrayMultiThread = MergeSortMulti(arrayMultiThread);
            stopwatch.Stop();

            long duration = stopwatch.ElapsedMilliseconds;
            string formatedTime = String.Format("Miliseconds: {0}", duration);
            Console.WriteLine("Runtime: " + formatedTime);

            bool sorted = IsSorted(arrayMultiThread);
            Console.WriteLine("The array is sorted: " + sorted);

            Console.WriteLine("Single-threaded:");

            stopwatch.Reset();
            stopwatch.Start();
            arraySingleThread = MergeSort(arraySingleThread);
            stopwatch.Stop();

            duration = stopwatch.ElapsedMilliseconds;
            formatedTime = String.Format("Miliseconds: {0}", duration);
            Console.WriteLine("Runtime: " + formatedTime);

            sorted = IsSorted(arraySingleThread);
            Console.WriteLine("The array is sorted: " + sorted);

            /*********************** Methods **********************
             *****************************************************/
            /*
            This method takes two sorted array and
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
            Takes an integer array by reference
            and makes some recursive calls to intself and then sorts the array
            for single threads
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
                Merge(MergeSort(LA), MergeSort(RA), A);

                return A;
            }
            /*
            Takes an integer array by reference
            splits the array into sub arrays and sorts the arrays
            with multi-threading
            */
            static int[] MergeSortMulti(int[] A)
            {
                int numberOfThreads = 12;
                int lengthA = A.Length;
                int portion = lengthA / numberOfThreads;
                int leftOver = lengthA % numberOfThreads;
                int[][] holderArray = new int[numberOfThreads][];
                int k = 0;

                if(leftOver == 0)
                {
                    for (int i = 0; i < numberOfThreads; i++)
                    {
                        int[] filler = new int[portion];
                        for (int j = 0; j < portion; j++)
                        {
                            filler[j] = A[k];
                            k++;
                        }
                        holderArray[i] = filler;
                    }
                }
                else // this is for when the the length of the array is not divided evenly by the number of threads
                {
                    for (int i = 0; i < numberOfThreads - 1; i++)
                    {
                        int[] filler = new int[portion];
                        for (int j = 0; j < portion; j++)
                        {
                            filler[j] = A[k];
                            k++;
                        }
                        holderArray[i] = filler;
                    }
                    int[] specialFiller = new int[portion + leftOver];
                    for (int j = 0; j < portion + leftOver; j++)
                    {
                        specialFiller[j] = A[k];
                        k++;
                    }
                    holderArray[numberOfThreads - 1] = specialFiller;
                }

                List<Thread> threads = new List<Thread>();

                //create and start threads
                int q = 0;
                while(q < numberOfThreads)
                {
                    int[] myArray = holderArray[q];
                    Thread thread = new Thread(() => MergeSort(myArray));
                    thread.Name = string.Format("Thread{0}", q + 1);
                    thread.Start();
                    threads.Add(thread);
                    q++;
                }

                // Await threads
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }

                int length = portion * 2;

                for (int j = 0; j < numberOfThreads - 1; j++)
                {
                    int[] test = new int[length];
                    holderArray[j] = Merge(holderArray[j], holderArray[j + 1], test);
                    length = length + portion;
                }

                return holderArray[holderArray.Length - 1];
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
