using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task[] tasks = new Task[10];

            Sheduler sheduler = new Sheduler();

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                Console.WriteLine($"Задача номер {Task.CurrentId}"));

                tasks[i].Start(sheduler);
            }

            Console.ReadKey();


        }
    }
}
