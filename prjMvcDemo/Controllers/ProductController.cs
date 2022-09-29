using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult List()
        {
            string keyword = Request.Form["txtKeyword"];
            dbDemoEntities db = new dbDemoEntities();
            IEnumerable<tProduct> datas = null;
            if (string.IsNullOrEmpty(keyword))
                datas = from p in db.tProduct
                        select p;
            else
                datas = db.tProduct.Where(p => p.fName.Contains(keyword));             
            return View(datas);
        }
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                dbDemoEntities db = new dbDemoEntities();
                tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == id);
                if (prod != null)
                    return View(prod);
            }
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult Edit(tProduct inProd)
        {
           
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == inProd.fId);
            if (prod != null)
            {
                if (inProd.photo != null)
                {
                    string photoName =Guid.NewGuid().ToString()+".jpg";
                    inProd.photo.SaveAs(Server.MapPath("../../Images/"+ photoName));
                    prod.fImagePath = photoName;
                }
                prod.fName = inProd.fName;
                prod.fQty = inProd.fQty;
                prod.fCost = inProd.fCost;
                prod.fPrice = inProd.fPrice;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(tProduct p)
        {
            dbDemoEntities db = new dbDemoEntities();
            db.tProduct.Add(p);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                dbDemoEntities db = new dbDemoEntities();
                tProduct prod=db.tProduct.FirstOrDefault(p=>p.fId== id);
                if (prod != null)
                {
                    db.tProduct.Remove(prod);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("List");
        }
    }
}