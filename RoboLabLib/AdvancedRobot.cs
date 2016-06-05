using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab
{
    /*
    //TODO
    public class AdvancedRobot : Robot
    {
        private Action currentAction;

        public event ActionCompletedEventHandler ActionCompleted;

        public AdvancedRobot()
        {
            this.ActionCompleted += AdvancedRobot_ActionCompleted;
            currentAction = null;
        }
        

        private void AdvancedRobot_ActionCompleted(object sender, ActionCompletedEventArgs args)
        {
            
        }
    }

    public class QueueAction : Action
    {
        Queue<Action> queue;

        public QueueAction(IEnumerable<Action> actions)
        {
            queue = new Queue<Action>(actions);
        }

        public Action GetCurrentAction()
        {
            return queue.Peek();
        }

        public void Enqueue(Action action)
        {
            queue.Enqueue(action);
        }

        public Action Dequeue()
        {
            return queue.Dequeue();
        }

        public bool IsEmpty()
        {
            return queue.Count == 0;
        }
    }

    public class ParallelAction : Action
    {
        LinkedList<Action> actions;

        public ParallelAction(IEnumerable<Action> actions)
        {
            this.actions = new LinkedList<Action>(actions);
        }

        public IEnumerable<Action> GetActions()
        {
            return actions;
        }

        public void AddAction(Action action)
        {
            actions.AddLast(action);
        }

        public void RemoveAction(Action action)
        {
            actions.Remove(action);
        }

        public bool IsEmpty()
        {
            return actions.Count == 0;
        }
    }
    */
}
