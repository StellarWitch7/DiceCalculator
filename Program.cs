using System;
using System.Collections.Generic;
using System.Linq;

namespace DiceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool appRunning = true;

            while (appRunning)
            {
                Console.WriteLine("Enter the amount of die. Example: the 6 in 6d10. Enter 'exit' to quit.");
                string input = Console.ReadLine();

                if (input == "exit")
                {
                    appRunning = false;
                }

                int amount = int.Parse(input);
                Console.WriteLine("Enter the type of die. Example: the 10 in 6d10. Enter 'exit' to quit.");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    appRunning = false;
                }

                int type = int.Parse(input);
                Console.WriteLine("Calculating...");
                Console.WriteLine(Calculate(amount, type));

                //Exit program
                Console.WriteLine("Input 'exit' to quit, press enter to retry");
                if (Console.ReadLine() == "exit")
                {
                    appRunning = false;
                }
            }
        }

        private static string Calculate(int amount, int type)
        {
            Dictionary<int, int> Counts = new Dictionary<int, int>();
            Array combo = new int[amount];
            bool isDone = false;

            for (int i = combo.Length; i > 0; i--)
            {
                combo.SetValue(1, i - 1);
            }

            while (isDone == false)
            {
                int newTotal = 0;

                foreach (int value in combo)
                {
                    newTotal += value;
                }

                if (!Counts.ContainsKey(newTotal))
                {
                    Counts.Add(newTotal, 0);
                }

                Counts[newTotal]++;
                isDone = Increment(ref combo, 0, amount, type);
            }

            return Counts.OrderByDescending(kv => kv.Value).First().Key.ToString();
        }

        private static bool Increment(ref Array array, int pos, int dieAmount, int dieType)
        {
            if (pos < array.Length)
            {
                if ((int) array.GetValue(pos) < dieType)
                {
                    array.SetValue((int) array.GetValue(pos) + 1, pos);
                    return false;
                }
                else
                {
                    array.SetValue(1, pos);
                    return Increment(ref array, pos + 1, dieAmount, dieType);
                }
            }
            else
            {
                return true;
            }
        }
    }
}
