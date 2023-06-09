﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarDiceCalculator
{
    class Program
    {
        static void Main()
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
                Console.WriteLine("Enter the bonus to be added to each roll. Must be a whole number. Enter 'exit' to quit.");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    appRunning = false;
                }

                int bonus = int.Parse(input);
                Console.WriteLine("Enter the roll advantage. Negative numbers will result in a disadvantage. Enter 'exit' to quit.");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    appRunning = false;
                }

                int advantage = int.Parse(input);
                Console.WriteLine("Enter the minimum roll to succeed. Entering zero may stop it from being calculated. Enter 'exit' to quit.");
                input = Console.ReadLine();

                if (input == "exit")
                {
                    appRunning = false;
                }

                int minimumRoll = int.Parse(input);
                Console.WriteLine("Calculating...");

                if (advantage >= amount)
                {
                    Console.WriteLine("Error: No dice left over");
                }
                else
                {
                    Console.WriteLine(Calculate(amount, type, bonus, advantage, minimumRoll));
                }

                //Exit program
                Console.WriteLine("Input 'exit' to quit, press enter to retry");
                if (Console.ReadLine() == "exit")
                {
                    appRunning = false;
                }
            }
        }

        private static string Calculate(int amount, int type, int bonus, int advantage, int minimumRoll)
        {
            //This is the simple version. It is very fast. 
            if (bonus == 0 && advantage == 0 && minimumRoll == 0)
            {
                if (amount <= 0 || type <= 0) return "Error: Cannot process numbers";
                var coefficient = (type + 1) / 2f;
                return "Dice: " + amount
                + "d" + type
                + ". Bonus: " + bonus
                + ". Advantage/Disadvantage: " + advantage
                + ". Minimum Roll: " + minimumRoll
                + ". Most likely roll: " + Math.Round(amount * coefficient)
                + ". Chance to succeed: not calculated.";
            }

            if (Math.Pow(type, amount) > uint.MaxValue)
            {
                return "Type and amount of die results in a value larger than the maximum of an int. This is not acceptable.";
            }

            Dictionary<int, ulong> counts = new Dictionary<int, ulong>();
            int[] combo = new int[amount];
            int combinationsCounted = 0;
            int loop = 0;
            bool isDone = false;

            for (int i = combo.Length - 1; i > 0; i--)
            {
                combo.SetValue(1, i);
            }

            while (isDone == false)
            {
                int[] finalCombo;
                int total = 0;

                isDone = Increment(ref combo, 0, type);

                if (total >= amount * type)
                {
                    isDone = true;
                }

                //Check for the lowest values and remove them based on advantage
                if (advantage < 0)
                {
                    int[] a = new int[combo.Length];
                    int[] b = new int[a.Length + advantage];
                    Array.Copy(combo, a, combo.Length);
                    Array.Sort(a);
                    Array.Copy(a, 0, b, 0, a.Length + advantage);
                    finalCombo = b;
                }
                else if (advantage > 0)
                {
                    int[] a = new int[combo.Length];
                    int[] b = new int[a.Length - advantage];
                    Array.Copy(combo, a, combo.Length);
                    Array.Sort(a, new CustomReverseComparer());
                    Array.Copy(a, 0, b, 0, a.Length - advantage);
                    finalCombo = b;
                }
                else
                {
                    finalCombo = combo;
                }

                foreach (int value in finalCombo)
                {
                    total += value;
                }

                total += bonus;

                if (!counts.ContainsKey(total))
                {
                    counts.Add(total, 0);
                }

                counts[total]++;
                combinationsCounted++;
                loop = (loop + 1) % 250000;

                if (loop == 0)
                {
                    Console.WriteLine("Combinations calculated: " + combinationsCounted + " of " + Math.Pow(type, amount));
                }
            }

            int mostLikelyRoll = counts.OrderByDescending(kv => kv.Value).First().Key;
            ulong above = 0;
            ulong below = 0;
            //Calculate chanceToSucceed
            foreach (KeyValuePair<int, ulong> key in counts)
            {
                if (key.Key >= minimumRoll)
                {
                    above += key.Value;
                }
                else
                {
                    below += key.Value;
                }
            }

            ulong aboveBelowTotal = above + below;
            ulong chanceToSucceed = (200 * above + 1) / (aboveBelowTotal * 2);

            return "Dice: " + amount
                + "d" + type
                + ". Bonus: " + bonus
                + ". Advantage/Disadvantage: " + advantage
                + ". Minimum Roll: " + minimumRoll
                + ". Most likely roll: " + mostLikelyRoll
                + ". Chance to succeed: " + chanceToSucceed;
        }

        private static bool Increment(ref int[] array, int pos, int dieType)
        {
            if (pos < array.Length)
            {
                if (array[pos] < dieType)
                {
                    array[pos] = array[pos] + 1;
                    return false;
                }
                else
                {
                    array[pos] = 1;
                    return Increment(ref array, pos + 1, dieType);
                }
            }
            else
            {
                return true;
            }
        }
    }
}
