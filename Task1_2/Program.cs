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
            Console.Write("first diag: {0}\tsecond diag: {1}\t", d1, d2);
            Console.WriteLine();
            
            int max = int.MinValue;
            int min = int.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (myArr[i, j] > max)
                    {
                        max = myArr[i, j];
                    }
                    if (myArr[i, j] < min)
                    {
                        min = myArr[i, j];
                    }
                }
            }
            Console.Write("Max: {0}\tMin: {1}\t", max, min);

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
