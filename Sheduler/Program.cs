using Sheduler.Sheduler;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Sheduler
{
    internal class Program
    {

     
        static void Main(string[] args)
        {

            Console.WriteLine($"Id потока - {Thread.CurrentThread.ManagedThreadId }");

            Task[] tasks = new Task[10];

            TSheduler reviewTaskSheduler = new TSheduler();

            //QueueTaskTesting(tasks, reviewTaskSheduler);
            //TryExecuteTaskInLineTesting(tasks, reviewTaskSheduler);
            TryDequeueTesting(tasks, reviewTaskSheduler);

            try
            {
                Task.WaitAll(tasks); /// заставляепм первичный поток ждать выполнение всех задач
            }
            catch (Exception)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Несколько задач были отменены");

                Console.ResetColor();
               
            }
            finally
            {
                Console.WriteLine("Метод закончил свое выполнение");
            }

            Console.ReadKey();

        }


        static void QueueTaskTesting(Task[] tasks, TSheduler sheduler)
        {

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}");
                });

                tasks[i].Start(sheduler);
            }

        }

        static void TryExecuteTaskInLineTesting(Task[] tasks, TSheduler sheduler)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task<int>(() =>
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"Задача {Task.CurrentId} выполнилась в потоке {Thread.CurrentThread.ManagedThreadId}");
                    return 1;
                });
            }

            foreach (var task in tasks)
            {
                task.Start(sheduler);

                task.Wait();
            }
        }

        static void TryDequeueTesting(Task[] tasks, TSheduler sheduler)
        {
            #region скоординированная отмена

            CancellationTokenSource cts = new CancellationTokenSource(); 
            CancellationToken token = cts.Token; /// уникальный маркер отмены. Уникал 

            cts.CancelAfter(555);

            #endregion

            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    Thread.Sleep(2000);

                    Console.WriteLine($"Задача выполнилась в потоке {Task.CurrentId}, в потоке {Thread.CurrentThread.ManagedThreadId}") ;
                } , token);

                tasks[i].Start(sheduler);

                 ///Метод TryDeque вызывается т.к. здесь мы передаем токен отмены
            }
        }


      


    }
}
