using System;
using System.Threading;

namespace Common
{
    public static class Printer
    {
        public static float speed = 12f;
        public static void Print(string text)
        {
            float sleep = 1 / speed;

            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(TimeSpan.FromSeconds(sleep));
            }
            Console.WriteLine();
        }

        public static void Wait(float seconds)
        {
            Thread.Sleep(TimeSpan.FromSeconds(seconds));
        }
    }
}
