using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiberaryWeb;
namespace LiberaryWeb.Controllers
{
    public class AccountController : Controller
    {

        public List<Account> db()
        {
                 

                List<Account> accounts = new List<Account>();
                accounts.Add(new Account()
                {
                    AccountNumber = "067457", AccHolderName = "Hem Pandey", Email = "hem@gmail.com", Phone = "5105202090",
                        AccountType = "Checking", AccountBalance = 300000 });
                    accounts.Add(new Account()
                {
                    AccountNumber = "045457", AccHolderName = "Harish Chand", Email = "harish@gmail.com", Phone = "510008877",
                        AccountType = "Shaving", AccountBalance = 7000000 });
                    accounts.Add(new Account()
                {
                    AccountNumber = "067687", AccHolderName = "Bharat Adhikari", Email = "Brad@gmail.com", Phone = "5109876098",
                        AccountType = "Checking", AccountBalance = 9000000 });
                    accounts.Add(new Account()
                {
                    AccountNumber = "067547", AccHolderName = "Ram Sunwar", Email = "Ram@gmail.com", Phone = "5105123290",
                        AccountType = "Shaving", AccountBalance = 4000000 });


            return accounts;

        }

        
        
        // GET: Account
        public ActionResult Index()
        {        

            return View(db().ToList());
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
