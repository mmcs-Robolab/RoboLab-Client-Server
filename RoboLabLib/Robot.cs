using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    public class Robot
    {
        private BaseRobot baseRobot {
            get
            { return baseRobot; }
            set
            {
                baseRobot = value;
                baseRobot.ActionCompleted += BaseRobot_ActionCompleted;
                baseRobot.SensorUpdate += BaseRobot_SensorUpdate;
            }
        }

        private void BaseRobot_SensorUpdate(object sender, SensorUpdateEventArgs args)
        {
            if (SensorUpdate != null)
                SensorUpdate(this, args);
        }

        private void BaseRobot_ActionCompleted(object sender, ActionCompletedEventArgs args)
        {
            if (ActionCompleted != null)
                ActionCompleted(this, args);
        }

        public event SensorUpdateEventHandler SensorUpdate;

        public event ActionCompletedEventHandler ActionCompleted;

        public event PrintEventHandler PrintMessage;

        public Robot()
        {

        }

        public void SetBaseRobot(BaseRobot baseRobot)
        {
            this.baseRobot = baseRobot;
        }

        public Robot(BaseRobot baseRobot)
        {
            this.baseRobot = baseRobot;
        }

        public void Print(IFormattable msg)
        {
            if (PrintMessage != null)
                PrintMessage(this, new PrintEventArgs(msg.ToString()));
        }

        /// <summary>
        /// Команда "идти вперед"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public void MoveForward(double distance)
        {
            baseRobot.MoveForward(distance);
        }

        /// <summary>
        /// Команда "идти назад"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public void MoveBackward(double distance)
        {
            baseRobot.MoveBackward(distance);
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
        /// <param name="angle">Угол поворота</param>
        public void TurnLeft(double angle)
        {
            baseRobot.TurnLeft(angle);
        }

        /// <summary>
        /// Команда "повернуть направо"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public void TurnRight(double angle)
        {
            baseRobot.TurnRight(angle);
        }


        /// <summary>
        /// Асинхронная команда "идти вперед"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public void StartMoveForward(double distance)
        {
            baseRobot.StartMoveForward(distance);
        }

        /// <summary>
        /// Асинхронная команда "идти назад"
        /// </summary>
        /// <param name="distance">Расстояние, на которое пройти</param>
        public void StartMoveBackward(double distance)
        {
            baseRobot.StartMoveBackward(distance);
        }

        /// <summary>
        /// Асинхронная команда "повернуть налево"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public void StartTurnLeft(double angle)
        {
            baseRobot.StartTurnLeft(angle);
        }

        /// <summary>
        /// Асинхронная команда "повернуть направо"
        /// </summary>
        /// <param name="angle">Угол поворота</param>
        public void StartTurnRight(double angle)
        {
            baseRobot.StartTurnRight(angle);
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
        public virtual void Main()
        {

        }
    }

    public class PrintEventArgs : EventArgs
    {
        public string Message { get; set; }
        public PrintEventArgs(string msg = "")
        {
            Message = msg;
        }
    }

    public delegate void PrintEventHandler(object sender, PrintEventArgs args);
}
