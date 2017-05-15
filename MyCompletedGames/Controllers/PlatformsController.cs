using MyCompletedGames.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyCompletedGames.Controllers
{
    [Authorize]
    public class PlatformsController : Controller
    {
        
        // GET: Platforms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Platforms/Create
        [HttpPost]
        public ActionResult Create(Platform platform)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please verify your data.");
                    return View();
                }

                if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                {
                    ModelState.AddModelError("", "An image file is required.");
                    return View();
                }

                var fileExtension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var contentType = Request.Files[0].ContentType.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg" && contentType != "image/png" && contentType != "image/jpeg" && contentType != "image/jpg")
                {
                    ModelState.AddModelError("", "Invalid image file. Supported formats: .jpg, .png, .jpeg");
                    return View(platform);
                }

                if (Request.Files[0].ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxImageFileSize"]))
                {
                    ModelState.AddModelError("", "The provided image is too large");
                    return View(platform);
                }

                var uploadDir = Server.MapPath("~/App_Data/images");
                Directory.CreateDirectory(uploadDir);
                var filepath = Path.Combine(uploadDir, Path.GetRandomFileName() + ".png");
                Request.Files[0].SaveAs(filepath);

                Database.ExecuteStoredProcedureNonQuery("InsertPlatform", System.IO.File.ReadAllBytes(filepath), "@Name=" + platform.Name, "@Type=" + platform.Type);

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error has occurred, please try again later.");
                return View();
            }
        }

        public ActionResult GetImage(int id)
        {
            return File(Database.ReadBinaryData("SELECT Image FROM Platforms WHERE ID = " + id), "image/png");
        }

        

        // GET: Platforms/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (var platform in Database.ExecuteStoredProcedure("GetPlatformById", "@ID=" + id))
                return View(new Platform() { Name = platform["Name"].ToString(), Type = Convert.ToInt32(platform["Type"]), Image = Url.Action("GetImage", "Platforms", new { id = platform["ID"] }) } );

            return View();
        }

        // POST: Platforms/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Platform platform)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "Please verify your data.");
                    return View(platform);
                }

                if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                {
                    ModelState.AddModelError("", "An image file is required.");
                    return View(platform);
                }

                var fileExtension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var contentType = Request.Files[0].ContentType.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg" && contentType != "image/png" && contentType != "image/jpeg" && contentType != "image/jpg")
                {
                    ModelState.AddModelError("", "Invalid image file. Supported formats: .jpg, .png, .jpeg");
                    return View(platform);
                }

                if (Request.Files[0].ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxImageFileSize"]))
                {
                    ModelState.AddModelError("", "The provided image is too large");
                    return View(platform);
                }

                var uploadDir = Server.MapPath("~/App_Data/images");
                Directory.CreateDirectory(uploadDir);
                var filepath = Path.Combine(uploadDir, Path.GetRandomFileName() + ".png");
                Request.Files[0].SaveAs(filepath);
                Database.ExecuteStoredProcedureNonQuery("EditPlatform", System.IO.File.ReadAllBytes(filepath), "@Name=" + platform.Name, "@Type=" + platform.Type, "@ID=" + id);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred, please try again later.");
                return View(platform);
            }
        }

        // GET: Platforms/Delete/5
        public ActionResult Delete(int id)
        {
            foreach(var p in Database.ExecuteStoredProcedure("GetPlatformById", "@ID=" + id))
                return View(new Platform() { Name = p["Name"].ToString() });

            return View();
        }

        // POST: Platforms/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Platform platform)
        {
            try
            {
                Database.ExecuteStoredProcedureNonQuery("DeletePlatformById", null, "@ID=" + id);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
