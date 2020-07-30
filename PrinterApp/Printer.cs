using System;
using System.Threading;

namespace PrinterApp
{
    internal partial class Program
    {
        public class Printer
        {
            protected const long MILLIS_PER_PAGE = 500;

            public static void Run(string name, IQueue queue)
            {
                Console.WriteLine("[" + name + "]: Ligando...");

                while (true)
                {
                    lock (Locker)
                    {
                        if (!queue.isEmpty())
                        {
                            IsLocked = true;
                            PrintJob pj = queue.removeFront();
                            Console.WriteLine($"Imprimindo '{pj.getJobName()}'");
                            try
                            {
                                Thread.Sleep(FormulasHelper.PageTime(pj.getNumberOfPages(), (int)MILLIS_PER_PAGE));
                                Console.WriteLine($"'{pj.getJobName()}' ok.");
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw e;
                            }
                        }
                        else
                        {
                            try
                            {
                                Console.WriteLine($"[{name}]: Esperando por trabalho de impressão...");
                                IsLocked = false;
                                Monitor.PulseAll(Locker);
                                Halt();
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw e;
                            }
                        }
                    }
                }
            }
            public static void Halt()
            {
                lock (Locker)
                {
                    _ = Monitor.Wait(Locker);
                }
            }
        }
    }
}