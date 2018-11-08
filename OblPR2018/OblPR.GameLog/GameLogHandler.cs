using System;
using System.Collections.Generic;
using System.Configuration;
using System.Messaging;

namespace OblPR.GameLog
{
    public class GameLogHandler
    {
        public GameLogHandler()
        {

        }

        public List<string> ReadLogQueue()
        {
            List<string>  log = new List<string>();
            string ip = ConfigurationManager.AppSettings["serverIp"];
            string serverQueueName = ConfigurationManager.AppSettings["queueName"];

            if (MessageQueue.Exists(serverQueueName))
            {
                using (var myQueue = new MessageQueue(serverQueueName))
                {
                    myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    {
                        var firstMessage = myQueue.Receive();
                        log.Add(firstMessage.Body.ToString());
                    }
                }
            }

            return log;
        }
    }
}