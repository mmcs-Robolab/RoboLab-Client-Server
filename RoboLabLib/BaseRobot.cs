using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{

    public abstract class BaseRobot
    {
        public event SensorUpdateEventHandler SensorUpdate;

        public event ActionCompletedEventHandler ActionCompleted;

        /// <summary>
        /// Команда "идти вперед"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public abstract void MoveForward(double distance);

        /// <summary>
        /// Команда "идти назад"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public abstract void MoveBackward(double distance);

        /// <summary>
        /// Остановка всех текущих действий
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Команда "повернуть налево"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public abstract void TurnLeft(double angle);

        /// <summary>
        /// Команда "повернуть направо"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public abstract void TurnRight(double angle);

        
        /// <summary>
        /// Асинхронная команда "идти вперед"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public abstract void StartMoveForward(double distance);

        /// <summary>
        /// Асинхронная команда "идти назад"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public abstract void StartMoveBackward(double distance);

        /// <summary>
        /// Асинхронная команда "повернуть налево"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public abstract void StartTurnLeft(double angle);

        /// <summary>
        /// Асинхронная команда "повернуть направо"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public abstract void StartTurnRight(double angle);

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

        
    }

    //TODO: Добавить еще видов сенсоров
    public enum SensorType { LaserScanner }
    
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

    public abstract class Action
    {
        public string Name { get; protected set; }
        
    }
    public enum MovementType { Forward, Backward, TurnLeft, TurnRight }
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

    public class SpecialAction : Action
    {
        public object Param { get; private set; }
        public SpecialAction(string action, object param = null)
        {
            Name = action;
            Param = param;
        }
    }
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
