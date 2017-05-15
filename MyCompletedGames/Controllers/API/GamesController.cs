using MyCompletedGames.DAL;
using MyCompletedGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCompletedGames.Controllers.API
{
    /// <summary>
    /// Games WebAPI endpoint. Demonstrates the use of WebAPI and AJAX Calls from client side.
    /// </summary>
    public class GamesController : ApiController
    {
        /// <summary>
        /// Inserts a new game into the database.
        /// </summary>
        /// <param name="game">Game data</param>
        /// <returns>200 if successful, 400 otherwise</returns>
        public dynamic Post(Game game)
        {
            if(ModelState.IsValid)
            {
                StoredProcedures.InsertGame(game);
                return Ok();
            }
            return BadRequest();
        }
    }
}
