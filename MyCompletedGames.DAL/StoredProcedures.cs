using MyCompletedGames.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCompletedGames.DAL
{
    /// <summary>
    /// Helper class to call the underlying SQLServer Stored Procedures.
    /// </summary>
    public class StoredProcedures
    {
        /// <summary>
        /// Inserts a new platform into the DB.
        /// </summary>
        /// <param name="platform">Platform data</param>
        public static void InsertPlatform(Platform platform)
        {
            Database.ExecuteStoredProcedureNonQuery("InsertPlatform", System.IO.File.ReadAllBytes(platform.Image), "@Name=" + platform.Name, "@Type=" + platform.Type);
        }

        /// <summary>
        /// Retrieves a specific platform (by its id). Returns null if not found.
        /// </summary>
        /// <param name="id">The id of the platform to return</param>
        /// <returns>Platform data.</returns>
        public static Platform GetPlatformById(int id)
        {
            foreach (var platform in Database.ExecuteStoredProcedure("GetPlatformById", "@ID=" + id))
                return new Platform() { Name = platform["Name"].ToString(), Type = Convert.ToInt32(platform["Type"]) };

            return null;
        }

        /// <summary>
        /// Inserts a new completed game into the DB. A platform can have 0..n completed games
        /// </summary>
        /// <param name="game">Game data</param>
        public static void InsertGame(Game game)
        {
            Database.ExecuteStoredProcedureNonQuery("InsertGame", null, "@PlatformId=" + game.PlatformId, "@Name=" + game.Name, "@Completed=" + game.Completed);
        }

        /// <summary>
        /// Gets the total platform count.
        /// </summary>
        /// <param name="search">The text to search. Leave empty to count everything.</param>
        /// <returns>Platform count</returns>
        public static int CountPlatforms(string search) {
            return Convert.ToInt32(Database.ExecuteStoredProcedureScalar("CountPlatforms", "@Search=" + search));
        }

        /// <summary>
        /// Edits a platform
        /// </summary>
        /// <param name="platform">Platform data</param>
        public static void EditPlatform(Platform platform)
        {
            Database.ExecuteStoredProcedureNonQuery("EditPlatform", System.IO.File.ReadAllBytes(platform.Image), "@Name=" + platform.Name, "@Type=" + platform.Type, "@ID=" + platform.ID);
        }

        /// <summary>
        /// Deletes a Platform by its ID
        /// </summary>
        /// <param name="id">Platform ID</param>
        public static void DeletePlatformById(int id)
        {
            Database.ExecuteStoredProcedureNonQuery("DeletePlatformById", null, "@ID=" + id);
        }

        /// <summary>
        /// Retrieves a list of platforms.
        /// </summary>
        /// <param name="search">The text to search. Leave empty to retrieve everything.</param>
        /// <param name="min">Min rownum to return</param>
        /// <param name="max">Max rownum to return</param>
        /// <returns>A list of Platforms</returns>
        public static IEnumerable<Platform> SearchPlatforms(string search, int min, int max)
        {
            foreach (var platform in Database.ExecuteStoredProcedure("SearchPlatforms", "@Search=" + search, "@Min=" + min, "@Max=" + max))
            {
                yield return new Platform() { ID = Convert.ToInt32(platform["ID"]), Name = platform["Name"].ToString(), Type = Convert.ToInt32(platform["Type"].ToString()), Image = "/Platforms/GetImage/" + platform["ID"] };
            }
        }

        /// <summary>
        /// Retrieves the list of completed games for the specified platform
        /// </summary>
        /// <param name="platformId">The platform ID</param>
        /// <returns>A list of games</returns>
        public static IEnumerable<Game> GetPlatformGames(int platformId)
        {
            foreach(var game in Database.ExecuteStoredProcedure("GetPlatformGames", "@PlatformId = " + platformId))
            {
                yield return new Game() { ID = Convert.ToInt32(game["ID"]), Name = game["Name"].ToString(), Completed = game["Completed"].ToString() };
            }
        }

    }
}
