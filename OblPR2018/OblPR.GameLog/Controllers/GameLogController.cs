using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                GameLogModel gameLog = new GameLogModel();
                return Ok(gameLog.GetGameLog());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}