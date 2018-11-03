using OblPR.Data.Entities;
using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Http;

namespace OblPR.WebService
{
    public class PlayerController : ApiController
    {
        private IPlayerManager playerManager;
        private int ip;
        private int port;
   
        public PlayerController()
        {

        }
        // POST: api/Player
        public IHttpActionResult Post([FromBody]AddPlayerModel playerModel)
        {
            try
            {
                var player = new Player(playerModel.Nick, playerModel.Image);
                var playerManager = GetPlayerService();
                playerManager.AddPlayer(player);
                return Content(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Player
        public IHttpActionResult Get()
        {
            try
            {
                var playerManager = GetPlayerService();
                var listPlayers = (List<Player>)playerManager.GetAllRegisteredPlayers();
                return Content(HttpStatusCode.OK, ParseResponsePlayers(listPlayers));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Player/5
        public IHttpActionResult Delete(string nick)
        {
            try
            {
                var playerManager = GetPlayerService();
                playerManager.DeletePlayer(nick);
                return Content(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Player/5
        public IHttpActionResult Put(Guid id, [FromBody]UpdatePlayerModel playerModel)
        {
            try
            {
                //var playerManager = GetPlayerService();
                //playerManager.UpdatePlayer(player);
                return Content(HttpStatusCode.OK, "Success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private IPlayerManager GetPlayerService()
        {
            try
            {
                var ip = int.Parse(ConfigurationManager.AppSettings["serverIp"]);
                var port = int.Parse(ConfigurationManager.AppSettings["serverPort"]);
            
                var playerManager = (IPlayerManager)Activator.GetObject(
                            typeof(IPlayerManager),
                            $"tcp://{ip}:{port}/{ServiceNames.PlayerManager}");

                return playerManager;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<GetAllPlayersModel> ParseResponsePlayers(List<Player> listPlayers)
        {
            var result = new List<GetAllPlayersModel>();
            foreach (var player in listPlayers)
            {
                var playerModel = new GetAllPlayersModel(player.Id,player.Nick);
                result.Add(playerModel);
            }
            return result;
        }
    }
}