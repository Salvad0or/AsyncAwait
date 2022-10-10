using System;
using System.Threading;
using System.Threading.Tasks;

namespace _002_ChildTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task parent = new Task(() =>
            {
                new Task(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Child 1 completed");
                }, TaskCreationOptions.AttachedToParent).Start(); /// Этот флаг прикрепляет нашу задачу и отмечает её как вложенную

                new Task(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Child 2 completed");
                }, TaskCreationOptions.AttachedToParent).Start(); /// Этот флаг прикрепляет нашу задачу и отмечает её как вложенную

            });


            parent.Start();
            parent.Wait();

            Console.WriteLine("Completed");

            ///Здесь родитель будет ждать пока дочерние задачи закончат
            Console.ReadKey();
        }
    }
}
