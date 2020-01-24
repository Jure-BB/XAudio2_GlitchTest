using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Text;
using System.Threading;

namespace CommonTools
{
    public static class Tools
    {
        static GCLatencyMode GCLatencyMode = GCLatencyMode.Interactive;

        public static void ShowHelp(string testName)
        {
            Console.WriteLine($"Test: {testName}{Environment.NewLine}" +
                $"Press P to show performance data{Environment.NewLine}" +
                $"Press I to initialize new garbage{Environment.NewLine}" +
                $"Press G to run full non-blocking garbage collection{Environment.NewLine}" +
                $"Press H to run full blocking garbage collection{Environment.NewLine}" +
                $"Press Esc to exit{Environment.NewLine}"
                );
        }

        public static void SetGCLatencyMode() =>
            GCSettings.LatencyMode = GCLatencyMode;

        public static void FullBatchCollect(bool block = false)
        {
            var type = block ? "blocking" : "non-blocking";
            var message = $"Full {type} GC";
            Debug.WriteLine(message);
            Console.WriteLine($"{Environment.NewLine}{message}");
            GCSettings.LatencyMode = GCLatencyMode;
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, block);
            //GC.WaitForPendingFinalizers();
        }

        public static void StartLoop(Func<int> getGlitchesCount)
        {
            while (true)
            {
                var pressed = Console.ReadKey();
                if (pressed.Key == ConsoleKey.Escape)
                    break;

                if (pressed.Key == ConsoleKey.G)
                    FullBatchCollect();

                if (pressed.Key == ConsoleKey.H)
                    FullBatchCollect(true);

                if (pressed.Key == ConsoleKey.I)
                { 
                    CreateGarbage();
                    OutputGlitchesCount(getGlitchesCount);
                }

                if (pressed.Key == ConsoleKey.P)
                    OutputGlitchesCount(getGlitchesCount);
            }
        }

        private static void OutputGlitchesCount(Func<int> getGlitchesCount)
        {
            Console.WriteLine($"{Environment.NewLine}Glitches since engine started:: {getGlitchesCount()}");
        }

        public static void CreateGarbage()
        {
            Console.WriteLine($"{Environment.NewLine}Creating garbage...");
            gList.Clear();
            for (int i = 0; i < 10_000_000; i++)
            {
                var g = new Garbage();
                gList.Add(g);
            }
            Console.WriteLine("Garbage creation completed!");
        }

        static List<Garbage> gList = new List<Garbage>();
    }
    class Garbage
    {
        int[] list = new int[100];
    }
}
