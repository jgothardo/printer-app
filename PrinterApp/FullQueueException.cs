using System;

namespace PrinterApp
{
    partial class Program
    {
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
    }
}