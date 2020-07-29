using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PrinterApp
{
    class Program
    {
        static object locker = new Object();
        static Boolean isLocked = false;
        static List<PrintJob> files = new List<PrintJob>();

        static void Main(string[] args)
        {
            const int MAX_FILES_QUEUE = 10;

            Thread threadMock = new Thread(() => MockFiles());
            threadMock.Start();

            Queue circularQueue = new CircularQueue(MAX_FILES_QUEUE);

            Thread threadPrinter = new Thread(() => Printer.printer("Printer", circularQueue));
            threadPrinter.Start();

            Thread[] threadsProducer = new Thread[2];
            for (int i = 0; i < 2; i++)
            {
                String prodId = string.Format("#Producer{0}#", (i + 1));
                ThreadStart prodWork = delegate { Producer.producer(prodId, circularQueue); };
                threadsProducer[i] = new Thread(prodWork);
                threadsProducer[i].Name = prodId;
                threadsProducer[i].Start();
            }

        }
        public class Producer
        {
            public static void producer(string name, Queue queue)
            {
                while (true)
                {
                    lock (locker)
                    {
                        while (isLocked)
                        {
                            Monitor.Wait(locker);
                        }
                        if (files.Count() > 0)
                        {
                            var printjob = files[0];
                            try
                            {
                                queue.addBack(printjob);

                                files.Remove(files[0]);

                            }
                            catch (FullQueueException)
                            {
                                Console.WriteLine("Fila cheia.");    
                            }

                            Console.WriteLine( name + " produzindo arquivo " + printjob.getJobName() + ", número de páginas: " + printjob.getNumberOfPages());
                            Monitor.PulseAll(locker);
                            Thread.Sleep(FormulasHelper.RandomTime());
                        }
                    }
                }
            }

        }

        public class Printer
        {
            public static void printer(string name, Queue queue)
            {
                const long MILLIS_PER_PAGE = 500;

                Console.WriteLine("[" + name + "]: Ligando...");

                while (true)
                {
                    lock (locker)
                    {
                        if (queue.isEmpty())
                        {
                            try
                            {
                                Console.WriteLine("[" + name + "]: Esperando por trabalho de impressão...");
                                isLocked = false;
                                Monitor.PulseAll(locker);
                                halt();
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw e;
                            }
                        }
                        else
                        {
                            isLocked = true;
                            PrintJob pj = queue.removeFront();
                            Console.WriteLine("Imprimindo '" + pj.getJobName() + "'");
                            try
                            {
                                Thread.Sleep(FormulasHelper.PageTime(pj.getNumberOfPages(), (int)MILLIS_PER_PAGE));
                                Console.WriteLine("'" + pj.getJobName() + "' ok.");
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw e;
                            }
                        }
                    }
                }
            }
            public static void halt()
            {
                lock (locker)
                {
                    Monitor.Wait(locker);
                }
            }
        }
        public interface Queue
        {
            public void addBack(PrintJob job);
            public PrintJob removeFront();
            public Boolean isEmpty();
            public int getNumbersOfJob();
        }

        private class CircularQueue : Queue
        {
            private static Queue<PrintJob> _jobQueue = new Queue<PrintJob>();
            private static int _capacity;

            public CircularQueue(int capacity)
            {
                _capacity = capacity;
            }

            public void addBack(PrintJob job)
            {
                if (getNumbersOfJob() >= _capacity)
                {
                    throw new FullQueueException("Fila cheia.");
                }
                else
                {
                    lock (locker)
                    {
                        _jobQueue.Enqueue(job);
                        Monitor.PulseAll(locker);
                    }
                }
            }

            public int getNumbersOfJob()
            {
                return _jobQueue.Count;
            }

            public bool isEmpty()
            {
                if (getNumbersOfJob() > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public PrintJob removeFront()
            {
                lock (locker)
                {
                    Monitor.PulseAll(locker);
                    return _jobQueue.Dequeue();
                }
            }
        }

        public class PrintJob
        {
            public PrintJob(string jobName, int numberOfPages)
            {
                JobName = jobName;
                NumberOfPages = numberOfPages;
            }

            public string JobName { get; set; }
            public int NumberOfPages { get; set; }

            public string getJobName()
            {
                return JobName;
            }

            public int getNumberOfPages()
            {
                return NumberOfPages;
            }
        }

        public class FullQueueException : Exception
        {
            public FullQueueException()
            {
            }

            public FullQueueException(string message)
                : base(message)
            {
            }
        }

        static class FormulasHelper
        {
            public static int RandomTime()
            {
                var rnd = new Random();
                return (1000 * rnd.Next(1, 6));
            }

            public static int PageTime(int pageNumber, int millisPerPage)
            {
                return pageNumber*millisPerPage;
            }
        }

        public static void MockFiles()
        {
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("hellofriend.xls", 3));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("ones-and-zeroes.txt", 10));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("debug.doc", 12));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("daemons.rtf", 5));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("exploits.xls", 1));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("undo.txt", 8));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("view-source.doc", 35));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("kernelpanic.rtf", 4));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("init.xls", 1));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("master-slave.txt", 5));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("hidden-process.doc", 4));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("kill-process.rtf", 1));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("not-found.xls", 3));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("runtime-error.txt", 9));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("metadata.txt", 18));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("power-saver-mode.txt", 20));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("dont-delete-me.txt", 5));
            Thread.Sleep(FormulasHelper.RandomTime());
            files.Add(new PrintJob("shutdown-r.txt", 1));
        }
    }
}
