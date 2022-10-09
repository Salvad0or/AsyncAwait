using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sheduler.Sheduler
{
    internal class TSheduler : TaskScheduler 
    {
        /// <summary>
        /// Связный список для хранения задач на выполнение
        /// </summary>
        private readonly LinkedList<Task> taskList = new LinkedList<Task>();

        /// <summary>
        /// Метод используется только в отладке.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return taskList;
        }

        /// <summary>
        /// Метод вызывается методом Start класса Task
        /// </summary>
        /// <param name="task"></param>
        protected override void QueueTask(Task task)
        {
            Console.WriteLine($"[QueueTask] Задача #{task.Id} поставлена в очередь");

            taskList.AddLast(task);

            ThreadPool.QueueUserWorkItem(ExecuteTask);
        }

        /// <summary>
        /// Метод вызывается методами ожидания: Wait, WaitAll
        /// </summary>
        /// <param name="task"></param>
        /// <param name="taskWasPreviouslyQueued"></param>
        /// <returns></returns>
        /// 
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            Console.WriteLine($"[TryExecuteTaskInline] Попытка выполнить задачу{task.Id} синхронно");

            lock (taskList)
            {
                taskList.Remove(task);
            }

            return TryExecuteTask(task);

            /// Задача должна вызваться в контексте потока который вызвал метод
        }

        /// <summary>
        /// Метод вызывается при отмене выполнения задачи
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected override bool TryDequeue(Task task)
        {
            Console.WriteLine($"Попытка удалить задачу из очереди : {task.Id}");
            bool result = false;

            lock (taskList)
            {
                result = taskList.Remove(task); /// если не сможет удалить вернет false
            }

            if (result)
            {
                Console.WriteLine($"Задача {task.Id} была удалена");
            }

            return result;

            ///Метод наделен логикой извлечения задачи. Выполняется при отмене задачи.
        }

        /// <summary>
        /// Метод обработки выполнения задач
        /// </summary>
        /// <param name="_"></param>
        private void ExecuteTask(object _)
        {
            while (true)
            {
                //Thread.Sleep(2000);
                Task task = null;

                lock (taskList) 
                {
                    if (taskList.Count == 0)
                    {
                        break; // если задач нет прерываемся
                    }

                    task = taskList.First.Value; // извлекаем из коллекции первый элемент
                    taskList.RemoveFirst(); // удаляем первый элемент
                }

                if (task == null) // доп. проверка.
                {
                    break;
                }

                TryExecuteTask(task);  // Метод создан специально для запуска задач, 
                                      // вызвать его можно только из класса наследника, protected.
            }
        }
    }
}
