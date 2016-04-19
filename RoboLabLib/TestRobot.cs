using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public class TestRobot : Robot
    {
        
        public TestRobot()
        {

        }
        
        asd
        /// <summary>
        /// Основная программа робота.
        /// Главный метод для переопределения!
        /// </summary>
        public override void Run()
        {
            if (Environment.StackTrace.Count() > 10000)
            {
                Print(Environment.StackTrace.Count());
                throw new StackOverflowException();
            }
            Run();
            this.SensorUpdated += TestRobot_SensorUpdated;
            while (true)
            {
                Print("TEST");
                Print(AppDomain.CurrentDomain.FriendlyName);
                Sleep(1000);
            }
        }

        private void badRecursion()
        {
            if (Environment.StackTrace.Count() > 512)
            {
                throw new StackOverflowException();
            }
            badRecursion();
        }

        private void TestRobot_SensorUpdated(object sender, SensorUpdateEventArgs args)
        {
            //badRecursion();
            Print(AppDomain.CurrentDomain.FriendlyName);
            /*while (true)
            {
                Print("TEST");
                Print(AppDomain.CurrentDomain.FriendlyName);
                Sleep(1000);
            }*/
        }
    }
    
}
