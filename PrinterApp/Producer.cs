using System;
using System.Threading;

namespace PrinterApp
{
    partial class Program
    {
        public class Producer
        {
            public static void Run(string name, IQueue queue)
            {
                while (true)
                {
                    lock (Locker)
                    {
                        while (IsLocked)
                        {
                            _ = Monitor.Wait(Locker);
                        }
                        if (Files.IsEmpty)
                        {
                            continue;
                        }
                        bool success = Files.TryTake(out PrintJob printJob);
                        if (!success)
                        {
                            continue;
                        }
                        try
                        {
                            queue.addBack(printJob);
                            Console.WriteLine($"{name} Produzindo arquivo {printJob.getJobName()}, número de páginas: {printJob.getNumberOfPages()}");
                            Monitor.PulseAll(Locker);
                            Thread.Sleep(FormulasHelper.RandomTime);
                        }
                        catch (FullQueueException)
                        {
                            Console.WriteLine($"{name} Fila cheia. Arquivo {printJob.getJobName()} aguardando.");
                            Monitor.Wait(Locker);
                            Files.Add(printJob);
                        }
                    }
                }
            }
        }
    }
}