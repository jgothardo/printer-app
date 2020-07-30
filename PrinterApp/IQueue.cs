namespace PrinterApp
{
    internal partial class Program
    {
        public interface IQueue
        {
            public void addBack(PrintJob job);
            public PrintJob removeFront();
            public bool isEmpty();
            public int getNumbersOfJob();
        }
    }
}