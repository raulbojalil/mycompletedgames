using MyCompletedGames.DAL;
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
        /// <summary>
        /// Returns the Create Platform View
        /// </summary>
        /// <returns>Create Platform View</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Platforms/Create
        /// <summary>
        /// Creates a new Platform
        /// </summary>
        /// <param name="platform">The platform data</param>
        /// <returns>Create Platform View if error. Redirects to Index if successful.</returns>
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

                //Checks if the user uploaded a file
                if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                {
                    ModelState.AddModelError("", "An image file is required.");
                    return View();
                }

                //Only image files are allowed
                var fileExtension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var contentType = Request.Files[0].ContentType.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg" && contentType != "image/png" && contentType != "image/jpeg" && contentType != "image/jpg")
                {
                    ModelState.AddModelError("", "Invalid image file. Supported formats: .jpg, .png, .jpeg");
                    return View(platform);
                }

                //Large files are not allowed
                if (Request.Files[0].ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxImageFileSize"]))
                {
                    ModelState.AddModelError("", "The provided image is too large");
                    return View(platform);
                }

                //Save the image file to a temporary location to upload to the DB as varbinary
                var uploadDir = Server.MapPath("~/App_Data/images");
                Directory.CreateDirectory(uploadDir);
                var filepath = Path.Combine(uploadDir, Path.GetRandomFileName() + ".png");
                Request.Files[0].SaveAs(filepath);

                //Call the stored procedure
                platform.Image = filepath;
                StoredProcedures.InsertPlatform(platform);

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "An error has occurred, please try again later.");
                return View();
            }
        }

        // GET: Platforms/GetImage/5
        /// <summary>
        /// Gets the raw binary data of the platform Image stored in the DB
        /// </summary>
        /// <param name="id">Platform ID</param>
        /// <returns>A File Content Result</returns>
        public ActionResult GetImage(int id)
        {
            return File(Database.ReadBinaryData("SELECT Image FROM Platforms WHERE ID = " + id), "image/png");
        }

        // GET: Platforms/Edit/5
        /// <summary>
        /// Returns the Edit Platform View
        /// </summary>
        /// <param name="id">The platform ID</param>
        /// <returns>Edit Platform View</returns>
        public ActionResult Edit(int id)
        {
            var platform = StoredProcedures.GetPlatformById(id);
            platform.Image = Url.Action("GetImage", "Platforms", new { id = id });

            return View(platform);
        }

        // POST: Platforms/Edit/5
        /// <summary>
        /// Edits a Platform
        /// </summary>
        /// <param name="id">The platform ID</param>
        /// <param name="platform">Platform data</param>
        /// <returns>Edit Platform View if error. Redirects to Index if successful.</returns>
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

                //Checks if the user uploaded a file
                if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
                {
                    ModelState.AddModelError("", "An image file is required.");
                    return View(platform);
                }

                //Only image files are allowed
                var fileExtension = Path.GetExtension(Request.Files[0].FileName).ToLower();
                var contentType = Request.Files[0].ContentType.ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".jpeg" && contentType != "image/png" && contentType != "image/jpeg" && contentType != "image/jpg")
                {
                    ModelState.AddModelError("", "Invalid image file. Supported formats: .jpg, .png, .jpeg");
                    return View(platform);
                }

                //Large files are not allowed
                if (Request.Files[0].ContentLength > Convert.ToInt32(ConfigurationManager.AppSettings["MaxImageFileSize"]))
                {
                    ModelState.AddModelError("", "The provided image is too large");
                    return View(platform);
                }

                //Save the image file to a temporary location to upload to the DB as varbinary
                var uploadDir = Server.MapPath("~/App_Data/images");
                Directory.CreateDirectory(uploadDir);
                var filepath = Path.Combine(uploadDir, Path.GetRandomFileName() + ".png");
                Request.Files[0].SaveAs(filepath);

                //Call the stored procedure
                platform.ID = id;
                platform.Image = filepath;
                StoredProcedures.EditPlatform(platform);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred, please try again later.");
                return View(platform);
            }
        }

        // GET: Platforms/Delete/5
        /// <summary>
        /// Returns the Delete Platform View
        /// </summary>
        /// <param name="id">Platform Id</param>
        /// <returns>Delete Platform View</returns>
        public ActionResult Delete(int id)
        {
            return View(StoredProcedures.GetPlatformById(id));
        }

        // POST: Platforms/Delete/5
        /// <summary>
        /// Deletes a Platform
        /// </summary>
        /// <param name="id">Platform Id</param>
        /// <param name="platform">Platform data</param>
        /// <returns>Delete Platform View if error. Redirects to Index if successful.</returns>
        [HttpPost]
        public ActionResult Delete(int id, Platform platform)
        {
            try
            {
                StoredProcedures.DeletePlatformById(id);
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
