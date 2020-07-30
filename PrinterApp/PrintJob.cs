namespace PrinterApp
{
    partial class Program
    {
        public class PrintJob
        {
            public PrintJob(string jobName, int numberOfPages)
            {
                JobName = jobName ?? throw new System.ArgumentNullException(nameof(jobName));
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
    }
}