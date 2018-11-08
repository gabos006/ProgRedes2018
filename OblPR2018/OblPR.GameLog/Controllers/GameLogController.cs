using System;
using System.Web.Http;

namespace OblPR.GameLog
{
    public class GameLogController : ApiController
    {
        public GameLogController()
        {

        }

        [Route("api/gamelog")]
        [HttpGet]
        public IHttpActionResult GetGameLog()
        {
            try
            {
                GameLogHandler handler = new GameLogHandler();
                return Ok(handler.ReadLogQueue());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}