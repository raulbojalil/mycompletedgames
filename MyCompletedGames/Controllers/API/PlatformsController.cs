using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyCompletedGames.Controllers.API
{
    public class PlatformsController : ApiController
    {

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

            totalItems = Convert.ToInt32(Database.ExecuteStoredProcedureScalar("CountPlatforms", "@Search=" + search));

            totalPages = (int)Math.Ceiling((double)totalItems / (double)itemsPerPage);
            page = Math.Min(totalPages, page);

            var min = 1 + itemsPerPage * (page - 1);
            var max = min + itemsPerPage - 1;

            return new
            {
                page = page,
                total = totalPages,
                platforms = from p in Database.ExecuteStoredProcedure("SearchPlatforms", "@Search=" + search, "@Min=" + min, "@Max=" + max)
                            select new { id = p["ID"], name = p["Name"], type = p["Type"], image = "/Platforms/GetImage/" + p["ID"] }
            };
    
        }


        public dynamic GetGames(int id)
        {
            return from g in Database.ExecuteStoredProcedure("GetPlatformGames", "@PlatformId = " + id)
                   select new { id = g["ID"], name = g["Name"], completed = g["Completed"] };
        }

    }
}
