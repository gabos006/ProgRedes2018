using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Web;

namespace OblPR.GameLog
{
    public class GameLogModel
    {
        public GameLogModel()
        {

        }

        public List<string> GetGameLog()
        {
            List<string>  log = new List<string>();
            string ip = ConfigurationManager.AppSettings["serverIp"];
            string queueName = ConfigurationManager.AppSettings["queueName"];

            string serverQueueName = @"FormatName:Direct=TCP:" + ip.Trim() + @"\private$\" + queueName.Trim();

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