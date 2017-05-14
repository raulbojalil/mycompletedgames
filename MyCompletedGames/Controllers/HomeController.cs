using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCompletedGames.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            //Database.ExecuteNonQuery("CREATE PROCEDURE CountPlatforms AS SELECT COUNT(*) FROM Platforms");
            //Database.ExecuteNonQuery("CREATE PROCEDURE SelectPlatforms (@Total int OUTPUT) AS SELECT * FROM Platforms SELECT  @Total = COUNT(*) FROM Platforms");

            //Database.ExecuteNonQuery("DROP TABLE Games");
            //Database.ExecuteNonQuery("DROP TABLE Platforms");
            //Database.ExecuteNonQuery("CREATE TABLE Platforms (ID int IDENTITY(1,1) PRIMARY KEY, Name varchar(255) NOT NULL, Type int NOT NULL, Image varBinary(MAX) NOT NULL)");
            //Database.ExecuteNonQuery("CREATE TABLE Games (ID int IDENTITY(1,1) PRIMARY KEY, Name varchar(255) NOT NULL, Completed varchar(255) NOT NULL, PlatformID int FOREIGN KEY REFERENCES Platforms(ID) NOT NULL)");

            //Database.ExecuteNonQuery("INSERT INTO Platforms(Name,Type) VALUES('Other', 2)");
            //foreach(var something in Database.ExecuteReader("SELECT * FROM Platforms ORDER BY ID OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY"))
            //{

            //}

            //Database.ExecuteNonQuery("CREATE PROCEDURE EditPlatform (@ID int, @Name varchar(255), @Type int, @Image varBinary(MAX)) AS UPDATE Platforms SET Name = @Name, Type = @Type, Image = @Image WHERE ID = @ID");

            //Database.ExecuteNonQuery("CREATE PROCEDURE InsertPlatform (@Name varchar(255), @Type int, @Image varBinary(MAX)) AS INSERT INTO Platforms(Name,Type,Image) VALUES(@Name, @Type, @Image)");

            //Database.ExecuteNonQuery("CREATE PROCEDURE SearchPlatforms4 (@search varchar) AS SELECT * FROM Platforms WHERE Name LIKE '%' + @search + '%'");
            /*foreach(var algo in Database.ExecuteStoredProcedure("SearchPlatforms4", "@search=th"))
            {

            }*/


            //foreach (var algo in Database.ExecuteNonQuery("SELECT * FROM Platforms"))
            //{

            //}



            //var algo2 = Database.ExecuteStoredProcedureScalar("CountPlatforms");

            //var algo = Database.ExecuteReader("SELECT * FROM Platforms ORDER BY ID OFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;");
            ViewBag.Username = User.Identity.Name;
            return View();
        }

       
    }
}