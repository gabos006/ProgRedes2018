using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Messaging;
using System.Web;
using System.Web.Http;

namespace OblPR.GameLog
{
    public class LogsController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get()
        {
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var clientQueueName = @"FormatName:DIRECT=TCP:" + ip + @"\private$\server";

            using (var queue = new MessageQueue(clientQueueName))
            {

                try
                {
                    queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
                    {
                        var result = new List<string>();
                        var messages = queue.GetMessageEnumerator2();
                        while (messages.MoveNext(new TimeSpan(0, 0, 1)))
                        {
                            result.Add($"{messages.Current.Body.ToString()}");
                            messages.RemoveCurrent();
                        }
                        return Ok(result);

                    }
                }
                catch (MessageQueueException e)
                {
                    return BadRequest(e.Message);
                }
            }

        }
    }
}