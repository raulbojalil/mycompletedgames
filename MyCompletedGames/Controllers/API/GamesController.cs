using MyCompletedGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCompletedGames.Controllers.API
{
    public class GamesController : ApiController
    {
        public dynamic Post(Game game)
        {
            if(ModelState.IsValid)
            {
                Database.ExecuteStoredProcedureNonQuery("InsertGame", null, "@PlatformId=" + game.PlatformId , "@Name=" + game.Name, "@Completed=" + game.Completed);
                return Ok();
            }
            return BadRequest();
        }
    }
}
