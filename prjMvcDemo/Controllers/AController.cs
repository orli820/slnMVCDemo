using prjMvcDemo.Models;
using prjXamlDemo.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjMvcDemo.Controllers
{
    public class AController : Controller
    {
        static int count = 0;


        public ActionResult demoUpload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult demoUpload(HttpPostedFileBase photo)
        {
            if(photo!=null)
                photo.SaveAs(@"C:\QN\Codes\slnMvcDemo\prjMvcDemo\Images\test.jpg");
            return View();
        }


        public ActionResult showCountByCookie()
        {
            int count = 0;
            HttpCookie x = Request.Cookies["KK"];
            if (x != null)
                count = Convert.ToInt32(x.Value);
            count++;
            x = new HttpCookie("KK");
            x.Value = count.ToString();
            x.Expires = DateTime.Now.AddSeconds(20);
            Response.Cookies.Add(x);

            ViewBag.COUNT = count;
            return View();
        }
        public ActionResult showCountBySession()
        {
            int count=0;
            if (Session["COUNT"] != null)
                count = (int)Session["COUNT"];
            count++;
            Session["COUNT"]= count;


            ViewBag.COUNT = count;
            return View();
        }
        public ActionResult showCount()
        {
            count++;
            ViewBag.COUNT = count;
            return View();
        }
        public string sayHello()
        {
            return "Hello ASP.NET MVC";
        }
        [NonAction]

        public string lotto()
        {
            return (new CLottoGen()).getNumber();
        }
        public string demoResponse()
        {
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.Filter.Close();
            Response.WriteFile(@"C:\QN\copy.jpg");
            Response.End();
            return "";
        }
        public string demoRequest()
        {
            string id = Request.QueryString["pid"];
            if (id == "1")
                return "XBox 加入購物車成功";
            else if (id == "2")
                return "PS5 加入購物車成功";
            else if (id == "3")
                return "Nintendo Switch 加入購物車成功";
            return "找不到該產品資料";
        }
        public string demoParameter(int? id)
        {
            if (id == null)
                return "未設定id";
            if (id == 1)
                return "XBox 加入購物車成功";
            else if (id == 2)
                return "PS5 加入購物車成功";
            else if (id == 3)
                return "Nintendo Switch 加入購物車成功";
            return "找不到該產品資料";
        }
        public string queryById(int? id)
        {
            string s = "未設定查詢條件";
            if (id == null)
                return s;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fId=" + id.ToString(), con);
            SqlDataReader reader = cmd.ExecuteReader();
            s = "查詢不到任何資料";
            if (reader.Read())
            {
                s = reader["fName"].ToString() + "<br/>" + reader["fPhone"].ToString();
            }
            con.Close();
            return s;
        }
        public ActionResult demoForm()
        {
            ViewBag.ANS = "X=?";
            if (!string.IsNullOrEmpty(Request.Form["txtA"]))
            {
                double a = Convert.ToDouble(Request.Form["txtA"]);
                double b = Convert.ToDouble(Request.Form["txtB"]);
                double c = Convert.ToDouble(Request.Form["txtC"]);
                ViewBag.a = a;
                ViewBag.b = b;
                ViewBag.c = c;
                double r = b * b - 4 * a * c;
                r = Math.Sqrt(r);
                ViewBag.ANS = "X="+((-b+r)/(2*a)).ToString("0.0#")+
                     "Or X=" + ((-b - r) / (2 * a)).ToString("0.0#") ;
            }
            return View();
        }


        public string demoServer()
        {
            return "目前伺服器上的實體位置：" + Server.MapPath(".");
        }

        // GET: A
        public ActionResult showById(int? id)
        {
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fId=" + id.ToString(), con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CCustomer x = new CCustomer();
                    x.fId = (int)reader["fId"];
                    x.fName = reader["fName"].ToString();
                    x.fPhone = reader["fPhone"].ToString();
                    x.fEmail = reader["fEmail"].ToString();
                    ViewBag.KK = x;
                }
                con.Close();
            }
            return View();
        }
        public string testingDelete(int? id)
        {
            if (id == null)
                return "請勿拿系統開玩笑";

            (new CCustomerFactory()).delete((int)id);
            return "刪除資料成功";
        }
        public string testingQuery()
        {
            return "目前客戶數：" + (new CCustomerFactory()).queryAll().Count.ToString();
        }
        public string testingInsert()
        {
            CCustomer x = new CCustomer()
            {
                //fAddress="Taipei",
                fEmail = "wanan@gmail.com",
                fName = "WanAn",
                fPassword = "1234",
                //fPhone="09797979"
            };
            (new CCustomerFactory()).create(x);
            return "新增資料成功";
        }

        public string testingUpdate()
        {
            CCustomer x = new CCustomer()
            {
                fId = 1012,
                fAddress = "Taipei",
                fEmail = "wanan@gmail.com",
                fName = "WanAn Chang",
                fPassword = "1234",
                fPhone = "09797979"
            };
            (new CCustomerFactory()).update(x);
            return "修改資料成功";
        }
        // GET: A
        public ActionResult bindingById(int? id)
        {
            CCustomer x = null;
            if (id != null)
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Data Source=.;Initial Catalog=dbDemo;Integrated Security=True";
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM tCustomer WHERE fId=" + id.ToString(), con);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    x = new CCustomer();
                    x.fId = (int)reader["fId"];
                    x.fName = reader["fName"].ToString();
                    x.fPhone = reader["fPhone"].ToString();
                    x.fEmail = reader["fEmail"].ToString();
                }
                con.Close();
            }
            return View(x);
        }
    }
}