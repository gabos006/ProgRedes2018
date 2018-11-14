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
            GameLogHandler handler = new GameLogHandler();
            try
            {
                return Ok(handler.ReadLogQueue());
            }
            catch (MessageQueueException e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}