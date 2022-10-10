using System;
using System.Threading;
using System.Threading.Tasks;

namespace _NestedTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// Это родительская задача т.к. в её теле есть создание других задач
            Task parent = new Task(() =>
            {
                new Task(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Nested 1 completed");
                }).Start();

                new Task(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine("Nested 2 completed");
                }).Start();

                Thread.Sleep(200);
            });

            parent.Start();
            parent.Wait();

            Console.WriteLine("Completed");

            ///Родительская задача не будет ждать окончания выполнения вложенных задач
            Console.ReadKey();



        }
    }
}
