using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TestTimer
{
    class Program
    {
        public static Timer iTimer;
        public static int iCount=5;
        static void Main(string[] args)
        {
            iTimer = new Timer();
            iTimer.Interval = 500;
            iTimer.Elapsed += ITimer_Elapsed;
            iTimer.Start();
            Console.ReadKey();
        }

        private static void ITimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (iCount != 0)
            {
                Console.Write(iCount-- );
            }
            else
            {
                //Console.WriteLine("\n");
                iCount = 5;
            }

        }

    }
}
