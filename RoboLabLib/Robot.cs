using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboLab
{
    public class Robot : MarshalByRefObject
    {
        private BaseRobot _baseRobot;
        private BaseRobot baseRobot
        {
            get
            { return _baseRobot; }
            set
            {
                if (_baseRobot != null)
                    _baseRobot.SensorUpdate -= BaseRobot_SensorUpdate;
                _baseRobot = value;
                if(_baseRobot != null)
                    _baseRobot.SensorUpdate += BaseRobot_SensorUpdate;
            }
        }

        private void BaseRobot_SensorUpdate(object sender, SensorUpdateEventArgs args)
        {
            if (SensorUpdated != null)
                Task.Factory.StartNew(() => this.SensorUpdated(this, args)).ContinueWith(t=>onCrashed(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
                
        }

        public event CrashedEventHandler Crashed;

        private void onCrashed(Exception e)
        {
            if (Crashed != null)
                Crashed(this, new CrashedEventArgs(e));
        }

        public event SensorUpdateEventHandler SensorUpdated;

        public event SleepEventHandler FellAsleep;

        public event SleepEventHandler WokeUp;

        public event PrintEventHandler PrintMessage;

        public event ReceiveEventHandler ReceiveMessage;

        public Robot()
        {

        }

        internal void SetBaseRobot(BaseRobot baseRobot)
        {
            this.baseRobot = baseRobot;
        }

        public BaseRobot GetBaseRobot()
        {
            return baseRobot;
        }

        public Robot(BaseRobot baseRobot)
        {
            this.baseRobot = baseRobot;
        }

        public void Receive(string msg)
        {
            if (ReceiveMessage != null)
                ReceiveMessage(this, new ReceiveEventArgs(msg));
        }

        public void Print(IFormattable msg)
        {
            if (PrintMessage != null)
                PrintMessage(this, new PrintEventArgs(msg.ToString()));
        }
        public void Print(string msg)
        {
            if (PrintMessage != null)
                PrintMessage(this, new PrintEventArgs(msg));
        }
        /// <summary>
        /// Команда "идти вперед"
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        public void BeginMoveForward(double speed)
        {
            baseRobot.BeginMoveForward(speed);
        }

        /// <summary>
        /// Команда "идти назад"
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        public void BeginMoveBackward(double speed)
        {
            baseRobot.BeginMoveBackward(speed);
        }

        /// <summary>
        /// Остановка всех текущих действий
        /// </summary>
        public void Stop()
        {
            baseRobot.Stop();
        }

        /// <summary>
        /// Команда "повернуть налево"
        /// </summary>
        /// <param name="speed">Скорость поворота</param>
        public void BeginTurnLeft(double speed)
        {
            baseRobot.BeginTurnLeft(speed);
        }

        /// <summary>
        /// Команда "повернуть направо"
        /// </summary>
        /// <param name="speed">Скорость поворота</param>
        public void BeginTurnRight(double speed)
        {
            baseRobot.BeginTurnRight(speed);
        }
        

        /// <summary>
        /// Специальное действие
        /// </summary>
        /// <param name="action">Название действия</param>
        /// <param name="param">Параметры к вызовы действия(если необходимо)</param>
        public void SpecialAction(string action, object param = null)
        {
            baseRobot.SpecialAction(action, param);
        }
        /// <summary>
        /// Получить описание поддерживаемых специальных действий
        /// </summary>
        /// <returns>Массив поддерживаемых специальных действий</returns>
        public string[] GetSpecialActions()
        {
            return baseRobot.GetSpecialActions();
        }
        
        /// <summary>
        /// Основная программа робота.
        /// Главный метод для переопределения!
        /// </summary>
        public virtual void Run()
        {
            
        }
        
        internal void RunAsync()
        {
            Task.Factory.StartNew(Run).ContinueWith(t => onCrashed(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
        }

        public void Sleep(double time = 0)
        {
            if (FellAsleep != null)
                FellAsleep(this, new SleepEventArgs(time));
        }

        public void WakeUp()
        {
            if (WokeUp != null)
                WokeUp(this, new SleepEventArgs());
        }
    }

    public delegate void CrashedEventHandler(object sender, CrashedEventArgs args);
    [Serializable]
    public class CrashedEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public CrashedEventArgs(Exception exception = null)
        {
            Exception = exception;
        }
    }
    [Serializable]
    public class PrintEventArgs : EventArgs
    {
        public string Message { get; set; }
        public PrintEventArgs(string msg = "")
        {
            Message = msg;
        }
    }
    [Serializable]
    public class ReceiveEventArgs : EventArgs
    {
        public string Message { get; set; }
        public ReceiveEventArgs(string msg = "")
        {
            Message = msg;
        }
    }
    [Serializable]
    public class SleepEventArgs : EventArgs
    {
        public double Time { get; set; }
        public SleepEventArgs(double time = 0)
        {
            Time = time;
        }
    }
    public delegate void ReceiveEventHandler(object sender, ReceiveEventArgs args);

    public delegate void PrintEventHandler(object sender, PrintEventArgs args);

    public delegate void SleepEventHandler(object sender, SleepEventArgs args);
}
