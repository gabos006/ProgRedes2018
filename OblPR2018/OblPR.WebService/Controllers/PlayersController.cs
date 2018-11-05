using OblPR.Data.Entities;
using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OblPR.WebService.Controllers
{

    public class PlayersController : ApiController
    {

        public PlayersController()
        {

        }
        [HttpPost]
        public IHttpActionResult Post([FromBody]AddPlayerModel playerModel)
        {
            try
            {
                var player = new Player(playerModel.Nick, playerModel.Image);
                var playerManager = GetPlayerService();
                playerManager.AddPlayer(player);
                return Created("/api/players", new GetPlayerModel(player.Id, player.Nick));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IHttpActionResult GetPlayers()
        {
            try
            {
                var playerManager = GetPlayerService();
                var listPlayers = playerManager.GetAllRegisteredPlayers();

                return Ok(listPlayers.Select(x => new GetPlayerModel(x.Id, x.Nick)).ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        public IHttpActionResult Delete(string nick)
        {
            try
            {
                var playerManager = GetPlayerService();
                playerManager.DeletePlayer(nick);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IHttpActionResult Put(Guid id, [FromBody]UpdatePlayerModel playerModel)
        {
            try
            {
                //var playerManager = GetPlayerService();
                //playerManager.UpdatePlayer(player);
                return Ok();
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
