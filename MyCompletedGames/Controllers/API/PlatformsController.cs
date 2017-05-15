using MyCompletedGames.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCompletedGames.Controllers.API
{
    /// <summary>
    /// Platforms WebAPI endpoint. Demonstrates the use of WebAPI and AJAX Calls from client side.
    /// </summary>
    public class PlatformsController : ApiController
    {
        /// <summary>
        /// Retrieves a paginated list of Platforms as a JSON Array.
        /// Supports the following query string parameters:
        /// page: The page number to retrieve, from 1 to n
        /// items: The max number of rows to retrieve per page, for example 10
        /// search: An optional parameter to specify the text to search platform names
        /// </summary>
        /// <returns>JSON Array of Platforms</returns>
        public dynamic Get()
        {
            var qs = Request.GetQueryNameValuePairs().ToDictionary(m => m.Key, m => m.Value);

            var page = 1;
            if (qs.ContainsKey("page"))
                page = Convert.ToInt32(qs["page"]);

            page = Math.Max(1,page);

            var totalItems = 0;
            var itemsPerPage = 3;

            if (qs.ContainsKey("items"))
                itemsPerPage = Convert.ToInt32(qs["items"]);

            itemsPerPage = Math.Max(3, itemsPerPage);

            var totalPages = 1;

            var search = qs.ContainsKey("search") ? qs["search"] : "";

            totalItems = StoredProcedures.CountPlatforms(search);

            totalPages = (int)Math.Ceiling((double)totalItems / (double)itemsPerPage);
            page = Math.Min(totalPages, page);

            var min = 1 + itemsPerPage * (page - 1);
            var max = min + itemsPerPage - 1;

            return new
            {
                page = page,
                total = totalPages,
                platforms = from p in StoredProcedures.SearchPlatforms(search, min, max)
                            select new { id = p.ID, name = p.Name, type = p.Type, image = p.Image }
            };
    
        }

        /// <summary>
        /// Retrieves a list of completed games for the specified platform as a JSON Array
        /// </summary>
        /// <param name="id">The platform ID</param>
        /// <returns>JSON Array of Games</returns>
        public dynamic GetGames(int id)
        {
            return from g in StoredProcedures.GetPlatformGames(id)
                   select new { id = g.ID, name = g.Name, completed = g.Completed };
        }

    }
}
