using System;
using System.Collections.Concurrent;
using System.Threading;

namespace PrinterApp
{
    internal partial class Program
    {
        private const int MAX_FILES_QUEUE = 10;

        public static object Locker = new object();
        internal static ConcurrentBag<PrintJob> Files = new ConcurrentBag<PrintJob>();
        public static bool IsLocked = false;

        private static void Main()
        {
            try
            {
                Thread threadMock = new Thread(() => MockFiles());
                threadMock.Start();

                IQueue circularQueue = new CircularQueue(MAX_FILES_QUEUE);

                Thread threadPrinter = new Thread(() => Printer.Run("Printer", circularQueue));
                threadPrinter.Start();
                Thread[] threadsProducer = new Thread[2];
                for (int i = 0; i < 2; i++)
                {
                    string prodId = $"#Producer{i + 1}#";
                    void prodWork() { Producer.Run(prodId, circularQueue); }
                    threadsProducer[i] = new Thread(prodWork)
                    {
                        Name = prodId
                    };
                    threadsProducer[i].Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Environment.Exit(-1);
            }
        }

        public static void MockFiles()
        {
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("hellofriend.xls", 3));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("ones-and-zeroes.txt", 10));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("debug.doc", 12));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("daemons.rtf", 5));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("exploits.xls", 1));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("undo.txt", 8));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("view-source.doc", 35));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("kernelpanic.rtf", 4));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("init.xls", 1));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("master-slave.txt", 5));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("hidden-process.doc", 4));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("kill-process.rtf", 1));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("not-found.xls", 3));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("runtime-error.txt", 9));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("metadata.txt", 18));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("power-saver-mode.txt", 20));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("dont-delete-me.txt", 5));
            Thread.Sleep(FormulasHelper.RandomTime);
            Files.Add(new PrintJob("shutdown-r.txt", 1));
        }
    }
}