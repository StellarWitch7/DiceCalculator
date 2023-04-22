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

                if (Math.Pow(type, amount) > 2000000000)
                {
                    amount = 0;
                    type = 0;
                    Console.WriteLine("Numbers are too big");
                }
                else
                {
                    Console.WriteLine(Calculate(amount, type));
                }

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
            Array combinations = new Array[(int) Math.Pow(type, amount)];
            Array lastCombo = new int[amount];

            for (int n = amount; n > 0; n--)
            {
                lastCombo.SetValue(1, amount - n);
            }

            for (int i = (int) Math.Pow(type, amount); i > 0; i--)
            {
                Array newCombo = lastCombo;

                for (int n = amount; n > 0; n--)
                {
                    if ((int) newCombo.GetValue(amount - n) < type)
                    {
                        newCombo.SetValue(1 + (int) newCombo.GetValue(amount - n), amount - n);
                        break;
                    }
                }

                lastCombo = newCombo;
                combinations.SetValue(newCombo, (int) Math.Pow(type, amount) - i);
            }

            Array totals = new int[combinations.Length];

            for (int i = combinations.Length; i > 0; i--)
            {
                int newTotal = 0;

                foreach (int value in (Array) combinations.GetValue(combinations.Length - i))
                {
                    newTotal += value;
                }

                totals.SetValue(newTotal, combinations.Length - i);
            }

            var counts = new Dictionary<int, int>();
            foreach (int i in totals)
            {
                if (!counts.ContainsKey(i))
                {
                    counts.Add(i, 0);
                }

                counts[i]++;
            }

            return counts.OrderByDescending(kv => kv.Value).First().Key.ToString();
        }
    }
}
