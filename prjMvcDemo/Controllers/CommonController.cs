using prjMvcDemo.Models;
using prjMvcDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Home()
        {
            if(Session[CDictionary.SK_LOGINED_USER]==null)
                return RedirectToAction("Login");
            return View();
        }        
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(CLoginViewModel model)
        {
            CCustomer x = (new CCustomerFactory()).queryByEmail(model.txtAccount);
            if (x != null && x.fPassword.Equals(model.txtPassword))
            {
                Session[CDictionary.SK_LOGINED_USER] = x;
                return RedirectToAction("Home");
            }
            return View();
        }
    }
}