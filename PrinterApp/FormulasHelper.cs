using System;

namespace PrinterApp
{
    partial class Program
    {
        public static class FormulasHelper
        {
            public static int RandomTime
            {
                get
                {
                    var rnd = new Random();
                    return (1000 * rnd.Next(1, 6));
                }
            }

            public static int PageTime(int pageNumber, int millisPerPage)
            {
                return pageNumber * millisPerPage;
            }
        }
    }
}