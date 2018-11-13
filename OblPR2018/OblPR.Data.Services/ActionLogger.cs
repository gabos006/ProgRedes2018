using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Services;

namespace OblPR.Game
{
   public  class ActionLogger:IActionLogger
    {

        private readonly string _queueName;
        public ActionLogger(string queueName)
        {
            this._queueName = queueName;
        }

        public void Log(string action)
        {
            using (var myQueue = new MessageQueue(_queueName, QueueAccessMode.SendAndReceive))
            {
                var message = new Message(action)
                {
                    Label = "Game Server message"
                };
                myQueue.Send(message);
            }
        }
    }
}
