using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinanceOptionsApp
{
    public static class MyParallel
    {
        class ForStarter
        {
            readonly List<Task> tasks = new List<Task>();
            readonly Action<int> action;
            public ForStarter(int start, int count, Action<int> action)
            {
                this.action = action;
                for (int i = start; i < (start + count); i++)
                {
                    tasks.Add(new Task(InternalAction,i));
                }
            }
            public void Start()
            {
                foreach (var task in tasks) task.Start();
                Task.WaitAll(tasks.ToArray());
            }
            void InternalAction(object o)
            {
                action((int)o);
            }
        }

        class InvokeStarter
        {
            readonly List<Task> tasks = new List<Task>();
            public InvokeStarter(params Action[] actions)
            {
                foreach (var action in actions)
                {
                    tasks.Add(new Task(action));
                }
            }
            public InvokeStarter(int n, Action action)
            { 
                for (int i=0;i<n;i++)
                {
                    tasks.Add(new Task(action));
                }
            }
            public void Start()
            {
                foreach (var task in tasks) task.Start();
                Task.WaitAll(tasks.ToArray());
            }
        }

        public static void For(int start, int count, Action<int> action)
        {
            new ForStarter(start, count, action).Start();
        }
        public static void Invoke(params Action[] actions)
        {
            new InvokeStarter(actions).Start();
        }
        public static void Invoke(int n, Action action)
        {
            new InvokeStarter(n,action).Start();
        }
    }
}
