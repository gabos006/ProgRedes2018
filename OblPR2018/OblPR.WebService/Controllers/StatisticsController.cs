using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OblPR.WebService.Controller
{
    public class StatisticsController : ApiController
    {
        public StatisticsController()
        {

        }
        [Route("statistics")]
        [HttpGet]
        public IHttpActionResult GetStatistics()
        {
            try
            {
                var matchManager = GetMatchService();
                var statistics = matchManager.GetStatistics();

                return Ok(statistics.Select(x => new GetStatitsticsModel(x.Id, x.Date, x.Results)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("ranking")]
        [HttpGet]
        public IHttpActionResult GetRanking()
        {
            try
            {
                var matchManager = GetMatchService();
                var ranking = matchManager.GetRanking();
                return Ok(ranking.Select(x => new GetRankingModel(x)).ToList());
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