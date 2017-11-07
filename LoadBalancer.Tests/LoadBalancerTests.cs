using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace LoadBalancer.Tests
{
    [TestFixture]
    public class LoadBalancerTests
    {
        [Test]
        public void TestName()
        {
            var queue = new BlockingCollection<int>();

            var producers = Enumerable.Range(0, 3)
                .Select(x => Task.Factory.StartNew(() =>
                {
                    Enumerable.Range(1, 100).ToList()
                        .ForEach(i => queue.Add(i));
                    Thread.Sleep(100);

                })).ToArray();

            var consumres = Enumerable.Range(0, 3)
               .Select(x => Task.Factory.StartNew(() =>
               {
                   foreach (var item in queue.GetConsumingEnumerable())
                   {
                       Console.WriteLine(item);
                   }
                   Thread.Sleep(100);

               })).ToArray();

            Task.WaitAll(producers);
            queue.CompleteAdding();
            Task.WaitAll(consumres);
        }
    }
}
