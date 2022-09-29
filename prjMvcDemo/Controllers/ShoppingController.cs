using prjMvcDemo.Models;
using prjMvcDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        public ActionResult List()
        {
            var products = from p in (new dbDemoEntities()).tProduct
                           select p;
            return View(products);
        }

        public ActionResult CartView()
        {
            List<CShoppingCartItem> cart = Session[CDictionary.SK_PUCHARESED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            if(cart==null)
                return RedirectToAction("List");
            return View(cart);
        }

        public ActionResult Edit(int? id)
        {
            List<CShoppingCartItem> cart = Session[CDictionary.SK_PUCHARESED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            if (cart == null)
                return RedirectToAction("List");
            CShoppingCartItem item = cart.FirstOrDefault(p => p.productId == id);
            //ViewBag.count = 100;
            ViewBag.count = item.count;
            if (item != null)
                return View(item);

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(CShoppingCartItem c)
        {
            List<CShoppingCartItem> cart = Session[CDictionary.SK_PUCHARESED_PRODUCTS_LIST] as List<CShoppingCartItem>;
            CShoppingCartItem item = cart.FirstOrDefault(p => p.productId == c.productId);

            item.count = c.count;
            return RedirectToAction("CartView");
        }

        public ActionResult AddToSession(int? id)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == id);
            if (prod != null)
                return View(prod);
            
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult AddToSession(CAddToCartViewModel p)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == p.txtFid);
            if (prod != null)
            {
                List<CShoppingCartItem> cart = Session[CDictionary.SK_PUCHARESED_PRODUCTS_LIST] as List<CShoppingCartItem>;
                if (cart == null)
                {
                    cart = new List<CShoppingCartItem>();
                    Session[CDictionary.SK_PUCHARESED_PRODUCTS_LIST] = cart;
                }
                CShoppingCartItem item = new CShoppingCartItem()
                {
                    count=p.txtCount,                   
                    price =(decimal)prod.fPrice,
                    productId=prod.fId,
                    product = prod
                };
                cart.Add(item);
            }
            return RedirectToAction("List");
        }
        // GET: Shopping
        public ActionResult AddToCart(int? id)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(p => p.fId == id);
            if (prod != null)
                return View(prod);
            return RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult AddToCart(CAddToCartViewModel p)
        {
            dbDemoEntities db = new dbDemoEntities();
            tProduct prod = db.tProduct.FirstOrDefault(t => t.fId == p.txtFid);
            if (prod != null)
            {
                tShoppingCart item = new tShoppingCart();
                item.fCount = p.txtCount;
                item.fCustomer = 1;
                item.fDate =DateTime.Now.ToString("yyyyMMddHHmmss");
                item.fPrice = prod.fPrice;
                item.fProduct = prod.fId;
                db.tShoppingCart.Add(item);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}