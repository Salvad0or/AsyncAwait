using System;
using System.Threading;
using System.Threading.Tasks;

namespace _003_NestedTasks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// вложенные задачи

            Task<string> parent = new Task<string>(() =>
            {

                var t1 = new Task<int>(() => Addition(5000));
                var t2 = new Task<int>(() => Addition(10000));
                var t3 = new Task<int>(() => Addition(50000));

                t1.Start();
                t2.Start();
                t3.Start();

                t1.ContinueWith((t) => Console.WriteLine($"Сложение 1 - {t.Result}"));
                t2.ContinueWith((t) => Console.WriteLine($"Сложение 2 - {t.Result}"));
                t3.ContinueWith((t) => Console.WriteLine($"Сложение 3 - {t.Result}"));

                return "Выполнена";

                //return $"Результат. Задача 1: {t1.Result}, Задача 2: {t2.Result}, Задача 3: {t3.Result}";


            });

            parent.Start();

            Console.WriteLine($"Результат задачи: {parent.Result}");

            //Console.WriteLine(parent.Result);

            /// Здесь родительская задача так же завершилась раньше вложенных.  
            /// Закомментированный код позволяет дождаться выполнения вложенных задач
            /// Блокирует поток дожидаясь выполнения асинхронной задачи

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
