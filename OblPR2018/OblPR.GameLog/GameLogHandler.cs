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
            List<string> result = new List<string>();
            string ip = ConfigurationManager.AppSettings["serverIp"];
            string serverQueueName = ConfigurationManager.AppSettings["queueName"];
            try
            {
                if (MessageQueue.Exists(serverQueueName))
                {
                    using (var queue = new MessageQueue(serverQueueName))
                    {
                        queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                        var messages = queue.GetMessageEnumerator2();
                        while (messages.MoveNext(new TimeSpan(0, 0, 1)))
                        {
                            result.Add($"{messages.Current.Body.ToString()}");
                            messages.RemoveCurrent();
                        }
                    }
                }
                return result;
            }
            catch (MessageQueueException e)
            {
                throw e;
            }
        }
    }
}