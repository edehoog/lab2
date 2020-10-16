using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

namespace pi
{
    class Program
    {
        //private static Mutex mut = new Mutex();
        static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            // needs to be 10000000 to consitently hit first 3 decimal values of pi
            long numberOfSamples = 100000000;

            long hits = 0;

            double radius = 1.0;

            // set to true to run multi-threaded implementation and set to false to run the single thread implementation
            bool multiThreading = true;

            if (multiThreading)
            {
                int numberOfProcessors = Environment.ProcessorCount; // 6 cores on my computer

                long numberOfThreads = 12;

                long size = numberOfSamples / numberOfThreads;

                long leftOver = numberOfSamples % numberOfThreads;

                List<Thread> threads = new List<Thread>();

                double[,] samples = new double[size, 2];

                for (int i = 0; i < numberOfThreads; i++)
                {
                    if(i == numberOfThreads - 1 && leftOver != 0)
                    {
                        Thread thread = new Thread(() => isHit(ref hits, GenerateSamples(size + leftOver), radius));
                        thread.Name = string.Format("Thread{0}", i + 1);
                        thread.Start();
                        threads.Add(thread);
                    }
                    else
                    {
                        Thread thread = new Thread(() => isHit(ref hits, GenerateSamples(size), radius));
                        thread.Name = string.Format("Thread{0}", i + 1);
                        thread.Start();
                        threads.Add(thread);
                    }
                }
                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }
            else
            {
                double[,] sampling = GenerateSamples(numberOfSamples);

                for (int i = 0; i < numberOfSamples; i++)
                {
                    if (Math.Sqrt((Math.Pow(sampling[i, 0], 2) + Math.Pow(sampling[i, 1], 2))) <= radius)
                    {
                        hits++;
                    }
                }
            }

            double pi = EstimatePI(numberOfSamples, ref hits);

            Console.WriteLine(pi);

            stopwatch.Stop();

            long duration = stopwatch.ElapsedMilliseconds;

            string formatedTime = String.Format("Miliseconds: {0}", duration);

            Console.WriteLine(formatedTime);

            //--------------------Methods--------------------
            /* 
             * Takes the number of samples and hits by refernce and returns the estimation of pi
             */
            static double EstimatePI(long numberOfSamples, ref long hits)
            {
                double pi = (Convert.ToDouble(hits) / Convert.ToDouble(numberOfSamples)) * 4.0;
                return pi;
            }
            /* 
             * Takes the number of samples and generates a double 2D array full of randomized samples
             */
            static double[,] GenerateSamples(long numberOfSamples)
            {
                double length = 1.0;

                var random = new Random();

                double[,] sampling = new double[numberOfSamples, 2];

                for (int i = 0; i < numberOfSamples; i++)
                {
                    sampling[i, 0] = length - (random.NextDouble() * 2);
                    sampling[i, 1] = length - (random.NextDouble() * 2);
                }
                return sampling;
            }
            /*
             Takes hits by reference, a 2D double array and the defined radius and increments hits if a sample
             is inside the area of the circle. This method is for multi threading.
             */
            static void isHit(ref long hits, double[,] samples, double radius)
            {
                //numberOfSamples is the number of rows in samples
                int numberOfSamples = samples.GetLength(0);

                for (int i = 0; i < numberOfSamples; i++)
                {   
                    if (Math.Sqrt((Math.Pow(samples[i, 0], 2) + Math.Pow(samples[i, 1], 2))) <= radius)
                    {
                        Interlocked.Increment(ref hits); //hits++;
                    }
                }
            }
        }
    }
}

