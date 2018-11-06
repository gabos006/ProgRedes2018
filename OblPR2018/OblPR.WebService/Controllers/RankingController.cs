using OblPR.Data.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;

namespace OblPR.WebService
{
    public class RankingController : ApiController
    {
        [Route("api/ranking")]
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