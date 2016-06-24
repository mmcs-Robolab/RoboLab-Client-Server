using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RoboLab
{
    
    public class SequentialMovesRobot : MovingRobot
    {
        private Queue<MoveAction> actionQueue = new Queue<MoveAction>();

        public event ActionCompletedEventHandler ActionCompleted;

        public bool AutoStartNextAction = true;

        public SequentialMovesRobot()
        {
            
        }

        private void StartAction(MoveAction action)
        {
            actionQueue.Enqueue(action);
            action.ActionCompleted += Action_ActionCompleted;
            if(actionQueue.Count == 1)
                action.StartAction(this);
        }
        
        private void Action_ActionCompleted(object sender, ActionCompletedEventArgs e)
        {
            if (actionQueue.Peek() == e.Action)
            {
                actionQueue.Dequeue();
                if(AutoStartNextAction && actionQueue.Count > 0)
                    actionQueue.Peek().StartAction(this);
            }
            if (ActionCompleted != null)
                ActionCompleted(this, e);
        }

        public void EnqueueMoveForward(double tachoLimit, double power, double turnRatio = 0)
        {
            StartAction(new MoveAction(tachoLimit, power, turnRatio));
        }
        public void EnqueueMoveBackward(double tachoLimit, double power, double turnRatio = 0)
        {
            StartAction(new MoveAction(tachoLimit, -power, turnRatio));
        }

        public void EnqueueTurnRight(double tachoLimit, double power)
        {
            StartAction(new MoveAction(tachoLimit, power, 1));
        }

        public void EnqueueTurnLeft(double tachoLimit, double power)
        {
            StartAction(new MoveAction(tachoLimit, power, -1));
        }
        public void MoveForward(double tachoLimit, double power, double turnRatio = 0)
        {
            MoveAction a = new MoveAction(tachoLimit, power, turnRatio);
            StartAction(a);
            while(!a.Completed) { }
        }
        public void MoveBackward(double tachoLimit, double power, double turnRatio = 0)
        {
            MoveAction a = new MoveAction(tachoLimit, -power, turnRatio);
            StartAction(a);
            while (!a.Completed) { }
        }
        public void TurnRight(double tachoLimit, double power)
        {
            MoveAction a = new MoveAction(tachoLimit, power, 1);
            StartAction(a);
            while (!a.Completed) { }
        }
        public void TurnLeft(double tachoLimit, double power)
        {
            MoveAction a = new MoveAction(tachoLimit, power, -1);
            StartAction(a);
            while (!a.Completed) { }
        }
        public void SkipCurrentAction()
        {
            if (actionQueue.Count > 0)
            {
                actionQueue.Dequeue();
                if (actionQueue.Count > 0)
                    actionQueue.Peek().StartAction(this);
            }
        }
        public void StopCurrentAction()
        {
            if (actionQueue.Count > 0)
                actionQueue.Peek().StopAction();
        }
        public void ResumeCurrentAction()
        {
            if (actionQueue.Count > 0)
                actionQueue.Peek().ResumeAction();
        }

        public MoveAction GetCurrentAction()
        {
            if (actionQueue.Count > 0)
                return actionQueue.Peek();
            else
                return null;
        }
    }

    public class ActionCompletedEventArgs : EventArgs
    {
        public MoveAction Action { get; private set; }

        public ActionCompletedEventArgs(MoveAction action)
        {
            this.Action = action;
        }
    }

    public delegate void ActionCompletedEventHandler(object sender, ActionCompletedEventArgs e);

    public class MoveAction
    {
        public double Power
        {
            get; private set;
        }
        public double TurnRatio
        {
            get; private set;
        }
        public double TachoLimit
        {
            get; private set;
        }
        double tachoLeft;
        public double PollInterval
        {
            get
            {
                return timer.Interval;
            }
            set
            {
                timer.Interval = value;
            }
        }
        public bool Completed
        {
            get
            {
                return tachoLeft <= 0;
            }
        }
        public event ActionCompletedEventHandler ActionCompleted;
        MovingRobot robot;
        Timer timer = new Timer(1);
        public MoveAction(double tachoLimit, double power, double turnRatio = 0)
        {
            timer.AutoReset = true;
            TachoLimit = tachoLimit;
            tachoLeft = TachoLimit;
            Power = power;
            TurnRatio = turnRatio;
        }

        public void StartAction(MovingRobot robot)
        {
            this.robot = robot;
            robot.BeginMoveForward(Power, TurnRatio);
            
            if (TachoLimit > 0)
            {
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
            }
        }
        public void StopAction()
        {
            timer.Stop();
            if(robot != null)
                robot.Stop();
        }

        public void ResumeAction()
        {
            if (robot != null)
            {
                robot.BeginMoveForward(Power, TurnRatio);
                timer.Start();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            double tacho = 0;
            int mcount = 0;
            foreach (IMotor m in robot.GetMotors())
            {
                IPollable p = m as IPollable;
                if (p!= null && m.MotorType > 0)
                {
                    MotorPollResult pr = p.Poll() as MotorPollResult;
                    if (pr != null)
                    {
                        tacho += pr.TachoCount;
                        ++mcount;
                    }
                }
            }
            tachoLeft -= tacho / mcount;
            if(tachoLeft < 0)
            {
                robot.Stop();
                timer.Stop();
                if (ActionCompleted != null)
                    ActionCompleted(this, new ActionCompletedEventArgs(this));
            }

        }
    }
    
    
}
