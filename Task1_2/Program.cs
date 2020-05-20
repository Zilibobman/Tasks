using System;

namespace Task1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] myArr = new int[4, 5];
            Random ran = new Random();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    myArr[i, j] = ran.Next(1, 25);
                    Console.Write("{0}\t", myArr[i, j]);
                }
                Console.WriteLine();
            }
            int d1 = 0;
            for (int i = 0; i < 4; i++)
            {
                d1 += myArr[i, i];
            }
            int d2 = 0;
            for (int i = 4; i > 0; i--)
            {
                d2 += myArr[4 - i, i];
            }
            Console.Write("Max: {0}\tMin: {1}\t", Math.Max(d1, d2), Math.Min(d1, d2));
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
