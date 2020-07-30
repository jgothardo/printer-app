using System.Collections.Generic;
using System.Threading;

namespace PrinterApp
{
    internal partial class Program
    {
        private class CircularQueue : IQueue
        {
            public CircularQueue(int capacity)
            {
                Capacity = capacity;
            }

            public static int Capacity;

            internal static Queue<PrintJob> JobQueue = new Queue<PrintJob>();

            public void addBack(PrintJob job)
            {
                if (getNumbersOfJob() < Capacity)
                {
                    lock (Locker)
                    {
                        JobQueue.Enqueue(job);
                        Monitor.PulseAll(Locker);
                    }
                }
                else
                {
                    throw new FullQueueException("Fila cheia.");
                }
            }

            public int getNumbersOfJob() => JobQueue.Count;

            public bool isEmpty()
            {
                return getNumbersOfJob() <= 0;
            }

            public PrintJob removeFront()
            {
                lock (Locker)
                {
                    Monitor.PulseAll(Locker);
                    return JobQueue.Dequeue();
                }
            }
        }
    }
}