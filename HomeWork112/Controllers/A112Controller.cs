using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HomeWork112.Models;
using System.Data.SqlClient;
using System.Data;

namespace HomeWork112.Controllers
{
    public class A112Controller : Controller
    {
        string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A11010112.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
 
        // GET: A112
        A11010112Entities1 myDB = new A11010112Entities1();
        public ActionResult indexHome()
        {
            return View(myDB.product.ToList());
        }
        public ActionResult Index()
        {
            return View(myDB.product.ToList());
        }
        public ActionResult order()
        {
            string mySql = "select DISTINCT   cAccount from orderlist";
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = connString;
            SqlDataAdapter adp = new SqlDataAdapter(mySql, myConn);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            DataTable dt = ds.Tables[0];
            string[] Datalist=new string[dt.Rows.Count];
            for (int i=0; i < dt.Rows.Count; i++)
            {
                Datalist[i]=dt.Rows[i]["cAccount"].ToString();
            }
            ViewBag.DataList = Datalist;
            return View(myDB.orderlist.ToList());
        }
        [HttpGet]
        public ActionResult amountList(string optionValue)
        {
            var query = from p in myDB.orderlist where p.cAccount == optionValue select p.oAmount;
            int count = 0;
            foreach(var item in query)
            {
                count += item;
            }
            ViewBag.orderAccount = optionValue;
            ViewBag.orderAmount= count;
            return View();
        }
        public ActionResult CustomList()
        {
            return View(myDB.custom.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(custom p)
        {
            if (ModelState.IsValid)
            {
                p.cCard = 1;
                myDB.custom.Add(p);
                myDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Edit(int id)
        {
            var p = myDB.custom.Find(id);

            return View(p);
        }
        [HttpPost]


        public ActionResult Edit(custom c)
        {
            if (ModelState.IsValid)
            {
                var temp = myDB.custom
                    .Where(m => m.Id == c.Id)
                    .FirstOrDefault();
                temp.cName = c.cName;
                temp.cAccount = c.cAccount;
                temp.cCard = c.cCard;
                temp.cPasswd = c.cPasswd;
                myDB.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Delete(int id)
        {
            var temp = myDB.custom
                .Where(m => m.Id == id)
                .FirstOrDefault();
            myDB.custom.Remove(temp);
            myDB.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult orderListCount()
        {
            return View(getAllData());
        }

        private List<customCount> getAllData()
        {
            string mySql = "select cName,sum(orderlist.oAmount) as oAmount from custom inner join orderlist on orderlist.cAccount=custom.cAccount group by cName";
            SqlConnection myConn = new SqlConnection();
            myConn.ConnectionString = connString;

            SqlDataAdapter adp = new SqlDataAdapter(mySql, myConn);

            DataSet ds = new DataSet();
            adp.Fill(ds);

            DataTable dt = ds.Tables[0];
            List<customCount> pp = new List<customCount>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                customCount who = new customCount();
                who.cName = dt.Rows[i]["cName"].ToString();
                who.oAmount =Int32.Parse(dt.Rows[i]["oAmount"].ToString()) ;
                pp.Add(who);
            }

            return pp;
        }
    }
}
