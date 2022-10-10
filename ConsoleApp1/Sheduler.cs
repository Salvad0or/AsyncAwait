using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace ConsoleApp1
{
    internal class Sheduler : TaskScheduler
    {
        private readonly LinkedList<Task> taskList = new LinkedList<Task>();

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new System.NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            taskList.AddFirst(task);

            ThreadPool.QueueUserWorkItem(ExecuteTask);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new System.NotImplementedException();
        }

        public void ExecuteTask(object _)
        {

            while (true)
            {
                Task task = null;

                lock (taskList)
                {

                    Console.WriteLine("Мы попали в тело LOCK ExecuteTask");

                    if (taskList.Count == 0)
                    {
                        break;
                    }

                    task = taskList.First.Value;

                    taskList.RemoveFirst();

                }

                if (task == null)
                {
                    break;
                }

                TryExecuteTask(task);
            }
                

                
            
            
        }
    }
}
