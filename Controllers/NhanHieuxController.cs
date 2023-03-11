using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CuoiKy.Models;

namespace CuoiKy.Controllers
{
    public class NhanHieuxController : Controller
    {
        private DataMyPhamContext db = new DataMyPhamContext();

        // GET: NhanHieux
        public ActionResult Index()
        {
            return View(db.NhanHieux.ToList());
        }

        // GET: NhanHieux/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // GET: NhanHieux/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NhanHieux/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNH,TenNhanHieu")] NhanHieu nhanHieu)
        {
            if (ModelState.IsValid)
            {
                db.NhanHieux.Add(nhanHieu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhanHieu);
        }

        // GET: NhanHieux/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // POST: NhanHieux/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNH,TenNhanHieu")] NhanHieu nhanHieu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanHieu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanHieu);
        }

        // GET: NhanHieux/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanHieu nhanHieu = db.NhanHieux.Find(id);
            if (nhanHieu == null)
            {
                return HttpNotFound();
            }
            return View(nhanHieu);
        }

        // POST: NhanHieux/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NhanHieu nhanHieu = db.NhanHieux.Find(id);
            db.NhanHieux.Remove(nhanHieu);
            db.SaveChanges();
            return RedirectToAction("Index");
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
