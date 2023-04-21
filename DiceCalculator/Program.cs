using System;

namespace DiceCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            bool appRunning = true;

            while (appRunning)
            {
                Console.WriteLine("Enter the amount and type of die. Example: 6d10");
                string input = Console.ReadLine();

                //Exit program
                if (Console.ReadLine() == "exit")
                {
                    appRunning = false;
                }
            }
        }
    }
}
