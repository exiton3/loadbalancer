using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoadBalancer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(1234);
            TestName();
            Console.ReadKey();
        }

        public static void TestName()
        {
            var queue = new BlockingCollection<int>();

            var producers = Enumerable.Range(0, 3)
                .Select(x => Task.Factory.StartNew(() =>
                {
                    Enumerable.Range(1, 100).ToList()
                        .ForEach(i => queue.Add(i));
                    Thread.Sleep(100);

                })).ToArray();

            var consumres = Enumerable.Range(0, 2)
                .Select(x => Task.Factory.StartNew(() =>
                {
                    foreach (var item in queue.GetConsumingEnumerable())
                    {
                        Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} - ({item})"); 
                       
                    }
                    Thread.Sleep(100);

                })).ToArray();

            Task.WaitAll(producers);
            queue.CompleteAdding();
            Task.WaitAll(consumres);
        }
    }

    

    interface IWorker
    {
        void Do();
    }
}
