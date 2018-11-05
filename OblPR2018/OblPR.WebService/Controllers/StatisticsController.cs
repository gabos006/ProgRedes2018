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

        [HttpGet]
        public IHttpActionResult GetStatistics()
        {
            try
            {
                var playerManager = GetPlayerService();
                var statistics = playerManager.GetStatistics();

                return Ok(statistics.Select(x => new GetStatitsticsModel(x.Id, x.Date, x.Results)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetRanking()
        {
            try
            {
                var playerManager = GetPlayerService();
                var ranking = playerManager.GetRanking();
                return Ok(ranking.Select(x => new GetRankingModel(x)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IPlayerManager GetPlayerService()
        {
            var ip = ConfigurationManager.AppSettings["serverIp"];
            var port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);

            var playerManager = (IPlayerManager)Activator.GetObject(
                        typeof(IPlayerManager),
                        $"tcp://{ip}:{port}/{ServiceNames.PlayerManager}");

            return playerManager;
        }
    }
}