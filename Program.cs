using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CirclePaintingSimulator
{


    partial class Program
    {
        const int numberOfCircles = 1000;
        const int numberOfWorkers = 5;
        private static HashSet<int> PaintedCircles = new HashSet<int>();
        static void Main(string[] args)
        {

            ParallelPaintCircles(numberOfCircles, numberOfWorkers);
            // Measure execution time for K = 5
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ParallelPaintCircles(numberOfCircles, 5);
            stopwatch.Stop();
            long executionTimeForFiveWorkers = stopwatch.ElapsedMilliseconds;

            // Measure execution time for K = 20
            stopwatch.Reset();
            stopwatch.Start();
            ParallelPaintCircles(numberOfCircles, 20);
            stopwatch.Stop();
            long executionTimeForTwentyWorkers = stopwatch.ElapsedMilliseconds;

            // Measure execution time for K = 100
            stopwatch.Reset();
            stopwatch.Start();
            ParallelPaintCircles(numberOfCircles, 100);
            stopwatch.Stop();
            long executionTimeForHundredWorkers = stopwatch.ElapsedMilliseconds;

            // Print the execution times
            Console.WriteLine("Execution time for K = 5: {0}ms", executionTimeForFiveWorkers);
            Console.WriteLine("Execution time for K = 20: {0}ms", executionTimeForTwentyWorkers);
            Console.WriteLine("Execution time for K = 100: {0}ms", executionTimeForHundredWorkers);

        }
        // Function to add a painted circle to the HashSet
        public static void AddPaintedCircle(int circleId, int workerId, int threadId)
        {
            if (PaintedCircles.Add(circleId))
            {
                Console.WriteLine($"Thread {threadId} (Worker {workerId}): Circle with ID {circleId} finished paiting.");
            }
            else
            {
                Console.WriteLine($"Thread {threadId} (Worker {workerId}): Circle with ID {circleId} already exists in PaintedCircles set.");
            }
        }

        // Function to parallelize the painting process
        public static void ParallelPaintCircles(int numberOfCircles, int numberOfWorkers)
        {
            // Parallelize the process using Parallel.For
            Parallel.For(0, numberOfCircles, i =>
            {
                // Generate a unique circle ID based on the iteration index
                int circleId = i + 1;

                // Generate a worker ID based on the thread ID
                int workerId = Task.CurrentId ?? 0; // If Task.CurrentId is null, use 0 as the worker ID

                // Get the ID of the current thread
                int threadId = Thread.CurrentThread.ManagedThreadId;

                // Call the AddPaintedCircle function
                AddPaintedCircle(circleId, workerId, threadId);
            });
        }

    }

}