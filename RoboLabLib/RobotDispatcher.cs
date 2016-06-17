using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.IO;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace RoboLab
{
    public class RobotDispatcher : MarshalByRefObject
    {
        public event DispatcherPrintEventHandler DispatcherPrint;

        Dictionary<String, RobotThreadWrapper> robots;
        Dictionary<int, String> usersRobots;
        Dictionary<String, int> robotsUsers;

        System.Timers.Timer timer;
        Stopwatch stopwatch;
        public RobotDispatcher()
        {
            robots = new Dictionary<string, RobotThreadWrapper>();
            timer = new System.Timers.Timer(1);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Start();
            stopwatch = new Stopwatch();
            stopwatch.Start();
            usersRobots = new Dictionary<int, string>();
            robotsUsers = new Dictionary<string, int>();
        }
        public IEnumerable<string> GetRobots()
        {
            return robots.Keys;
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (KeyValuePair<string, RobotThreadWrapper> p in robots)
                p.Value.SleepTick(stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();
        }

        public void AddBaseRobot(string name, BaseRobot robot)
        {
            robots[name] = new RobotThreadWrapper();
            robots[name].Name = name;
            robots[name].PrintMessage += Robot_PrintMessage;
            robots[name].Robot = new Robot(robot);
        }

        public Robot GetUserRobot(int UserID)
        {
            return robots[usersRobots[UserID]].Robot;
        }

        public String GetUserRobotName(int UserID)
        {
            return usersRobots[UserID];
        }

        public bool BindUser(int userID, string robot)
        {
            if (usersRobots.ContainsKey(userID) || usersRobots.ContainsValue(robot))
                return false;
            usersRobots[userID] = robot;
            robotsUsers[robot] = userID;
            return true;
        }

        public string ManualControl(int userID)
        {
            if (!usersRobots.ContainsKey(userID))
                return "Failure";
            
            return RunRobot(usersRobots[userID], typeof(ControllableMovingRobot), true);
        }

        public string RunRobot(String name, Type RobotType, bool trusted = false)
        {
            BaseRobot baseRobot = robots[name].GetBaseRobot();
            robots[name].Finish();
            
            if (!trusted)
            {
                PermissionSet permSet = new PermissionSet(PermissionState.None);
                permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                Evidence ev = new Evidence();
                ev.AddHostEvidence(new Zone(SecurityZone.Untrusted));
                AppDomainSetup adSetup = new AppDomainSetup();
                
                AppDomain appDomain = AppDomain.CreateDomain(name, ev, adSetup, permSet);
                
                robots[name] = (RobotThreadWrapper)appDomain.CreateInstanceAndUnwrap(
                 Assembly.GetAssembly(typeof(Robot)).FullName,
                 "RoboLab.RobotThreadWrapper");
            }
            else
                robots[name] = new RobotThreadWrapper();
            robots[name].Name = name;
            robots[name].PrintMessage += Robot_PrintMessage;
            robots[name].SetBaseRobot(RobotType, baseRobot);
            robots[name].RunRobotAsync();
            return "Success";
            //System.Environment.StackTrace
        }
        public string RunRobot(String name, String source, String mainClass)
        {
            BaseRobot baseRobot = robots[name].GetBaseRobot();
            //if(robots[name].Robot != null)
            robots[name].Finish();
            /*PermissionSet permSet = new PermissionSet(PermissionState.None);
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
            //permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.));

            Evidence ev = new Evidence();
            ev.AddHostEvidence(new Zone(SecurityZone.Untrusted));
            
            AppDomainSetup adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            AppDomain appDomain = AppDomain.CreateDomain(name, ev, adSetup, permSet);*/
            AppDomain appDomain = AppDomain.CreateDomain(name);
            robots[name] = (RobotThreadWrapper)appDomain.CreateInstanceAndUnwrap(
                Assembly.GetAssembly(typeof(Robot)).FullName,
                "RoboLab.RobotThreadWrapper");

            robots[name].Name = name;
            robots[name].PrintMessage += Robot_PrintMessage;
            string s = robots[name].SetBaseRobot(source, mainClass, baseRobot);
            Logger.Log(s);
            if (s == "Success")
            {
                robots[name].RunRobotAsync();
                
            }
            else
                robots[name].SetBaseRobot(typeof(Robot), baseRobot);
            return s;
        }
        
        public void UnbindUser(int UserID)
        {
            if(usersRobots.ContainsKey(UserID))
            {
                string robot = usersRobots[UserID];
                StopRobot(robot);
                usersRobots.Remove(UserID);
                robotsUsers.Remove(robot);
            }
        }

        public void StopRobot(String name)
        {
            BaseRobot baseRobot = robots[name].GetBaseRobot();
            robots[name].Finish();
            robots[name] = new RobotThreadWrapper();
            robots[name].Name = name;
            robots[name].PrintMessage += Robot_PrintMessage;
            robots[name].SetBaseRobot(typeof(Robot), baseRobot);
        }

        private void Robot_PrintMessage(object sender, PrintEventArgs args)
        {
            Logger.Log(args.Message, sender);
            
            int userID = robotsUsers[((RobotThreadWrapper)sender).Name];

            if (DispatcherPrint != null)
                DispatcherPrint(this, new DispatcherPrintEventArgs(userID, args.Message));

        }
    }
    
    public class DispatcherPrintEventArgs : EventArgs
    {
        public string Message { get; set; }
        public int UserID { get; set; }
        public DispatcherPrintEventArgs(int userID, string message = "")
        {
            UserID = userID;
            Message = message;
        }
    }

    public delegate void DispatcherPrintEventHandler(Object sender, DispatcherPrintEventArgs args);
}
