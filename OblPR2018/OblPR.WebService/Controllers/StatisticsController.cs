using OblPR.Data.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace OblPR.WebService
{
    public class StatisticsController : ApiController
    {
        public StatisticsController()
        {

        }

        [Route("api/statistics")]
        [HttpGet]
        public IHttpActionResult GetStatistics()
        {
            try
            {
                var matchManager = GetMatchService();
                var statistics = matchManager.GetStatistics();

                return Ok(statistics.Select(x => new GetStatitsticsModel(x)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IGameMatchManager GetMatchService()
        {
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);

            var matchManager = (IGameMatchManager)Activator.GetObject(
                        typeof(IGameMatchManager),
                        $"tcp://{ip}:{port}/{ServiceNames.MatchManager}");

            return matchManager;
        }
    }
}