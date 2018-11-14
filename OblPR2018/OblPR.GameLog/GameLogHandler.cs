using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
                using (var queue = new MessageQueue(serverQueueName))
                {
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    var messages = queue.GetMessageEnumerator2();

                    var allMessages = queue.GetAllMessages();
                    foreach (var message in allMessages)
                    {
                        result.Add(message.Body.ToString());
                    }

                    // Delete all queue messages
                    queue.Purge();
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