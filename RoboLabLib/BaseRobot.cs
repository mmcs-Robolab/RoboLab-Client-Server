using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    
    public abstract class BaseRobot : MarshalByRefObject
    {
        public event SensorUpdateEventHandler SensorUpdate;

        /// <summary>
        /// Команда "идти вперед"
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        public abstract void BeginMoveForward(double speed);

        /// <summary>
        /// Команда "идти назад"
        /// </summary>
        /// <param name="speed">Скорость движения</param>
        public abstract void BeginMoveBackward(double speed);

        /// <summary>
        /// Остановка всех текущих действий
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Команда "повернуть налево"
        /// </summary>
        /// <param name="speed">Скорость поворота</param>
        public abstract void BeginTurnLeft(double speed);

        /// <summary>
        /// Команда "повернуть направо"
        /// </summary>
        /// <param name="speed">Скорость поворота</param>
        public abstract void BeginTurnRight(double speed);

        /// <summary>
        /// Получить список установленных на роботе сенсоров
        /// </summary>
        /// <returns></returns>
        public virtual SensorType[] GetSupportedSensors()
        {
            return new SensorType[] { };
        }

        public abstract void BeginGetSensorValue(SensorType type);

        public abstract double[] GetSensorValue(SensorType type);

        /// <summary>
        /// Специальное действие
        /// </summary>
        /// <param name="action">Название действия</param>
        /// <param name="param">Параметры к вызову действия (если необходимо)</param>
        public virtual void SpecialAction(string action, object param = null)
        {

        }
        /// <summary>
        /// Получить описание поддерживаемых специальных действий
        /// </summary>
        /// <returns>Массив поддерживаемых специальных действий</returns>
        public virtual string[] GetSpecialActions()
        {
            return new string[] { };
        }

        /// <summary>
        /// Метод вызова события получения данных с сенсоров
        /// </summary>
        protected void onSensorUpdate(SensorUpdateEventArgs e)
        {
            if (this.SensorUpdate != null)
                this.SensorUpdate(this, e);
        }
    }

    //TODO: Добавить еще видов сенсоров
    public enum SensorType { LaserScanner, Odometry }
    [Serializable]
    public class SensorUpdateEventArgs : EventArgs
    {
        public SensorType Type { get; set; }
        public double[] Data { get; set; }
        public SensorUpdateEventArgs(SensorType type, double[] data)
        {
            Type = type;
            Data = data;
        }
    }

    public delegate void SensorUpdateEventHandler(object sender, SensorUpdateEventArgs args);
    [Serializable]
    public abstract class Action
    {
        public string Name { get; protected set; }
        
    }
    public enum MovementType { Forward, Backward, TurnLeft, TurnRight }
    [Serializable]
    public class MovementAction : Action
    {
        public MovementType Type { get; private set; }
        public double Amount { get; private set; }
        public MovementAction(MovementType type, double amount)
        {
            switch (type)
            {
                case MovementType.Forward:
                    Name = "MoveForward";
                    break;
                case MovementType.Backward:
                    Name = "MoveBackward";
                    break;
                case MovementType.TurnLeft:
                    Name = "TurnLeft";
                    break;
                case MovementType.TurnRight:
                    Name = "TurnRight";
                    break;
            }
            Amount = amount;
        }
    }
    [Serializable]
    public class SpecialAction : Action
    {
        public object Param { get; private set; }
        public SpecialAction(string action, object param = null)
        {
            Name = action;
            Param = param;
        }
    }
    [Serializable]
    public class ActionCompletedEventArgs : EventArgs
    {
        public Action Action { get; set; }
        public ActionCompletedEventArgs(Action action = null)
        {
            Action = action;
        }
    }

    public delegate void ActionCompletedEventHandler(object sender, ActionCompletedEventArgs args);

    
}
