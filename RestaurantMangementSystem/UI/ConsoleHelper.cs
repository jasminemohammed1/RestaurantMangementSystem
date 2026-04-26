using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantMangementSystem.UI
{
   public static class ConsoleHelper
    {
        private const  string Sepertor = "══════════════════════════════════════════════════════";
        public static void  PrintHeader(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Sepertor);
            Console.WriteLine(title.ToUpper());
            Console.WriteLine(Sepertor);
            Console.ResetColor();

        }
        public static void PrintSubHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"\n ── {title} ── ");
            Console.ResetColor();
        }
        public static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✔  {msg}");
            Console.ResetColor();
        }
        public static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n  ✘  {msg}");
            Console.ResetColor();
        }
        public static void Warning(string msg) 
            {

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ⚠  {msg}");
            Console.ResetColor();
            }
        public static void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"     {msg}");
            Console.ResetColor();

        }
        public static void Row(string label , object ? value)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"{label,-22}");
            Console.ResetColor();
            Console.WriteLine(value);


        }
        public static int ReadInt(string prompt , int min = int.MinValue , int max =  int.MaxValue)
        {
            while(true)
            {
                Console.Write($"\n  {prompt}: ");
                if(int.TryParse(prompt, out int value) && value>=min && value<=max)
                        return value;
                Error($"Please enter a whole number between {min} and {max}.");

            }
        }
        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {

                Console.Write($"\n  {prompt}: ");
                if(decimal.TryParse(prompt,out decimal value) && value >0) return value;
                Error("Please enter a valid positive number.");
            }
        }
        public static string ReadString(string prompt , bool allowEmpty = false)
        {
            while (true)
            {
                Console.Write($"\n  {prompt}: ");
                string input = Console.ReadLine() ?? "";
                if (allowEmpty || !string.IsNullOrWhiteSpace(input))
                    return input;
                Error("This field cannot be empty.");
            }
        }
        public static bool Confirm(string prompt)
        {
            Console.Write($"\n  {prompt} (y/n): ");
            return (Console.ReadLine() ?? "").Trim().Equals("y", StringComparison.OrdinalIgnoreCase);
        }

        public static void Wait()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  Press any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        public static void PrintOption(string key, string label)
         => Console.WriteLine($"   [{key}] {label}");


        public static string Stars(int rating)
        {
            return new string('★', rating)+ new string('☆', 5-rating);
        }

    }
}
