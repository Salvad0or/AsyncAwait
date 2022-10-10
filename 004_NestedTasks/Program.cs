using System;
using System.Threading;
using System.Threading.Tasks;

namespace _004_NestedTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<string> parent = new Task<string>(() =>
            {
                Task<int> t1 = new Task<int>(() => Addition(5000), TaskCreationOptions.AttachedToParent); /// используя флаг помечаем как дочерние
                Task<int> t2 = new Task<int>(() => Addition(10000), TaskCreationOptions.AttachedToParent);
                Task<int> t3 = new Task<int>(() => Addition(15000), TaskCreationOptions.AttachedToParent);

                t1.Start();
                t2.Start();
                t3.Start();

                t1.ContinueWith((t) => Console.WriteLine($"Сложение 1 - {t.Result}"), TaskContinuationOptions.AttachedToParent); /// Делаем продолжение дочерним через флаг
                t2.ContinueWith((t) => Console.WriteLine($"Сложение 2 - {t.Result}"), TaskContinuationOptions.AttachedToParent);
                t3.ContinueWith((t) => Console.WriteLine($"Сложение 3 - {t.Result}"), TaskContinuationOptions.AttachedToParent);

                return "Выполнена";
            });

            parent.Start();

            ///Благодаря флагам родитель будет дожидаться выполнения задач
            Console.WriteLine($"Результат задачи - {parent.Result}");

            Console.ReadKey();

        }

        private static int Addition(int lenght)
        {
            int sum = 0;

            Thread.Sleep(3000);

            for (int i = 0; i < lenght; i++)
            {
                sum++;
            }

            return sum;
        }
    }
}
