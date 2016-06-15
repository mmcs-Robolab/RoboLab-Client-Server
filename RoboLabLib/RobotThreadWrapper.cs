using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboLab
{
    
    public class RobotThreadWrapper : MarshalByRefObject
    {
        Barrier barrier;
        private Robot robot;
        double timeToSleep;
        public bool Trusted;
        public string Name;
        Thread t;
        
        public event PrintEventHandler PrintMessage;

        public RobotThreadWrapper()
        {
            barrier = new Barrier(2);
            
        }

        public void SleepTick(double elapsed)
        {
            if (timeToSleep > 0)
            {
                timeToSleep -= elapsed;
                if(timeToSleep <= 0)
                    robot.WakeUp();
            }
        }
        public string SetBaseRobot(Type RobotType, BaseRobot baseRobot)
        {
            Robot = (Robot)Activator.CreateInstance(RobotType);
            Robot.SetBaseRobot(baseRobot);
            return "Success";
        }

        public string SetBaseRobot(String source, String mainClass, BaseRobot baseRobot)
        {
            dynamic classRef;
            try
            {
                classRef = DynamicCompiler.Compile(source, mainClass, new object[] { });
                if (classRef is CompilerErrorCollection)
                {
                    StringBuilder sberror = new StringBuilder();

                    foreach (CompilerError error in (CompilerErrorCollection)classRef)
                        sberror.AppendLine(string.Format("{0}:{1} {2} {3}",
                                           error.Line, error.Column, error.ErrorNumber, error.ErrorText));

                    Logger.Log(sberror.ToString());
                    
                    return sberror.ToString();
                }
                else
                {
                    return SetBaseRobot((Type)classRef, baseRobot);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
                return ex.Message;
            }
        }
        public BaseRobot GetBaseRobot()
        {
            return robot.GetBaseRobot();
        }
        public Robot Robot
        {
            get { return robot; }
            set
            {
                robot = value;
                robot.PrintMessage += Robot_PrintMessage;
                robot.FellAsleep += Robot_FellAsleep;
                robot.WokeUp += Robot_WokeUp;
            }
        }

        private void Print(string message)
        {
            if (PrintMessage != null)
                PrintMessage(this, new PrintEventArgs(message));
        }

        private void Robot_PrintMessage(object sender, PrintEventArgs args)
        {
            if(PrintMessage != null)
                PrintMessage(this, args);
        }

        private void Robot_Crashed(object sender, CrashedEventArgs args)
        {
            Exception e = args.Exception.InnerException;
            Print(e.Source + ": " + e.Message);
            Finish();
        }

        public void Finish()
        {
            try
            {
                robot.SetBaseRobot(null);

                robot.PrintMessage -= Robot_PrintMessage;
                robot.FellAsleep -= Robot_FellAsleep;
                robot.WokeUp -= Robot_WokeUp;
                //robot.StopRunning();
                //t.Interrupt();
                t.Abort();
                t.Join();
            }
            catch (Exception ee)
            {
                if (ee != null)
                    Print(ee.Message);
            }
            
            if (!Trusted)
            {
                try
                {
                    //AppDomain.Unload(AppDomain.CurrentDomain);
                    
                }
                catch (Exception ee)
                {
                    if(ee != null)
                        Print(ee.Message);
                }
            }
        }

        private void Robot_WokeUp(object sender, SleepEventArgs args)
        {
            if (barrier.ParticipantsRemaining == 1)
                barrier.SignalAndWait();
        }

        private void Robot_FellAsleep(object sender, SleepEventArgs args)
        {
            timeToSleep = args.Time;
            if (barrier.ParticipantsRemaining == 2)
                barrier.SignalAndWait();
        }
        private void TryRun()
        {
            try
            {
                Robot.Run();
            }
            catch(Exception e)
            {
                Print(e.Message);
            }
        }
        public void RunRobotAsync()
        {
            //robot.RunAsync();
            t = new Thread(new ThreadStart(TryRun));
            t.Start();
        }
        public AppDomain GetAppDomain()
        {
            return AppDomain.CurrentDomain;
        }
    }
}
