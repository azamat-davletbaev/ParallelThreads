using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCalc
{
    internal class Calc
    {        
        const int count = 100_000;
        public int[] items = new int[count];

        public Calc()
        {
            Random rnd = new Random();

            for (int i = 0; i < count; i++)
                items[i] = rnd.Next(0, 10);
        }

        /// <summary>
        /// Делегат, обертка для вызова методов. для удобства замера времени выполнения
        /// </summary>
        Func<Func<int>, (int Sum, long Milliseconds)> Watcher = (func) =>
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int sum = func.Invoke();

            sw.Stop();
            return (sum, sw.ElapsedMilliseconds);
        };

        /// <summary>
        /// обычное суммирование без потоков
        /// </summary>
        /// <param name="size">множитель 100k</param>
        /// <returns></returns>        
        public (int Sum, long Milliseconds) GetSum(int size) => Watcher(() =>
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                foreach (var item in items)
                {
                    sum += item;
                }
            }
            return sum;
        });


        public (int Sum, long Milliseconds) GetSumByLINQ(int size) => Watcher(() =>
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                sum += items.AsParallel().Sum();
            }
            return sum;
        });

        private object lockObject = new object();

        public (int Sum, long Milliseconds) GetSumByThread(int size) => Watcher(() =>
        {
            int sum = 0;

            Thread[] threads = new Thread[size];

            for (int i = 0; i < size; i++)
            {
                threads[i] = new Thread(() =>
                {
                    lock (lockObject)
                    {
                        sum += items.Sum();
                    }                    
                });
                threads[i].Start();                                
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return sum;
        });



    }
}
