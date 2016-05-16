using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComicConIT.Models;
using UserImageUploadAzure.Utilities;
using Microsoft.AspNet.Identity;
using System.IO;

namespace ComicConIT.Controllers
{
    public class ComicsController : Controller
    {
        private ComicsDBContext db = new ComicsDBContext();
        BlobUtility utility = new BlobUtility("itra", "ixuLGIOBLtHTpdu5apIgMS2XZL+G/vZqVADDyjLI8SbDDmPE8HNvIWYJ6W4nPeQSRAMYIGVQ8QowJzvdlG00gw==");
        // GET: Comics
        public ActionResult Index(string ComicGenre, string searchString)
        {
            var GenreLst = new List<string>();

            var GenreQry = from d in db.Comics
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.ComicGenre = new SelectList(GenreLst);

            var Comics = from m in db.Comics
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Comics = Comics.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(ComicGenre))
            {
                Comics = Comics.Where(x => x.Genre == ComicGenre);
            }
            Comics = Comics.OrderByDescending(x => x.Rate);

            return View(Comics);
        }

        public ActionResult New(string ComicGenre, string searchString)
        {
            var GenreLst = new List<string>();

            var GenreQry = from d in db.Comics
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.ComicGenre = new SelectList(GenreLst);

            var Comics = from m in db.Comics
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Comics = Comics.Where(s => s.Title.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(ComicGenre))
            {
                Comics = Comics.Where(x => x.Genre == ComicGenre);
            }
            Comics = Comics.OrderByDescending(x => x.ReleaseDate);

            return View("Index", Comics);
        }

        public ActionResult MY(string ComicGenre)
        {
            var GenreLst = new List<string>();

            var GenreQry = from d in db.Comics
                           orderby d.Genre
                           select d.Genre;

            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.ComicGenre = new SelectList(GenreLst);

            var Comics = from m in db.Comics
                         select m;

            var UserName = User.Identity.GetUserName();
            Comics = Comics.Where(s => s.UserName == UserName);

            var dd = User.Identity.GetUserName();

            return View("Index",Comics);
        }

        // GET: Comics/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            return View(comics);
        }

        // GET: Comics/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        public ActionResult Comicsgen()
        {
            return View();
        }
        [Authorize]
        public ActionResult Generate()
        {
            return View();
        }
        public ActionResult Vieww()
        {
            return View();
        }

        // POST: Comics/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ID,Title,ReleaseDate,Genre,Rating")] Comics comics, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    string ContainerName = "jsr"; //hardcoded container name. 
                    file = file ?? Request.Files["file"];
                    string fileName = Path.GetFileName(file.FileName);
                    Stream imageStream = file.InputStream;
                    var result = utility.UploadBlob(fileName, ContainerName, imageStream);
                    if (result != null)
                    {
                        string loggedInUserId = User.Identity.GetUserName();
                        comics.ID = new Random().Next();
                        comics.UserName = loggedInUserId;
                        comics.ImagePath = result.Uri.ToString();
                    }
                }
                comics.ReleaseDate = DateTime.Now;
                db.Comics.Add(comics);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comics);
        }

        // GET: Comics/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null)
            {
                return HttpNotFound();
            }
            return View(comics);
        }

        // POST: Comics/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "ID,Title,ReleaseDate,Genre,Rating")] Comics comics)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comics).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comics);
        }

        // GET: Comics/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comics comics = db.Comics.Find(id);
            if (comics == null || comics.UserName != User.Identity.GetUserName())
            {
                return HttpNotFound();
            }
            return View(comics);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comics comics = db.Comics.Find(id);
            db.Comics.Remove(comics);
            db.SaveChanges();
            string BlobNameToDelete = comics.ImagePath.Split('/').Last();
            utility.DeleteBlob(BlobNameToDelete, "jsr");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Vote(int? id, bool increment)
        {
            Comics comics = db.Comics.Find(id);


            if (increment) comics.Rate++;
            else comics.Rate--;
            db.SaveChanges();
            return PartialView("_PartialVote", comics);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
