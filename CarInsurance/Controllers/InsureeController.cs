using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        public ActionResult Admin()
        {
            return View(db.Insurees.ToList());
        }



        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                var ageInput = insuree.DateOfBirth;// an assortment of variables are evaluated by inspections to which their conditions add values
                int age = ageInput.Year;
                int appAge = DateTime.Now.Year - age;//dateTime use to quantify age

                int appAgeAdd1 = appAge <= 18 ? 100 : 0;////works - gets factored into beginning sum 'tertTotal'
                int appAgeAdd2 = appAge < 25 && appAge > 19 ? 50 : 0;////works - gets factored into beginning sum 'tertTotal'
                int appAgeAdd3 = appAge >= 26 ? 25 : 0;////works - gets factored into beginning sum 'tertTotal'


                var ageInputCar = insuree.CarYear;
                int carFactor = ageInputCar;
                int carAge = DateTime.Now.Year - carFactor;//dateTime use to quantify age

                int vehAgeAdd1 = carFactor<2000 ? 25 : 0;////works - gets factored into beginning sum 'tertTotal'
                int vehAgeAdd2 = carFactor>2015 ? 25: 0;////works - gets factored into beginning sum 'tertTotal'


                var carInput1 = insuree.CarMake;
                bool carAdd1 = (carInput1 == "Porsche");//boolean use to verify
                var carInput2 = insuree.CarModel;
                bool carAdd2 = (carInput2 == "911 Carrera");

                bool porche_No = (carAdd1 == false && carAdd2 == false);
                bool porche_One = (carAdd1 == true && carAdd2 == true);
                bool porche_Two = (carAdd1 == true && carAdd2 == true);

                int porcheAdd1 = porche_No ? 0 : 0;////works - gets factored into beginning sum 'tertTotal'
                int porcheAdd2 = porche_One ? 25 : 0;////works - gets factored into beginning sum 'tertTotal'
                int porcheAdd3 = porche_Two ? 50 : 0;////works - gets factored into beginning sum 'tertTotal'



                var ticketsInput = insuree.SpeedingTickets;
                int tickets = ticketsInput * 10;////works - gets factored into beginning sum 'tertTotal'

                int tertTotal = 50 + appAgeAdd1 + appAgeAdd2 + appAgeAdd3 + vehAgeAdd1 + vehAgeAdd2 + porcheAdd1 + porcheAdd2 + porcheAdd3 + tickets;

                Double tertTotal2 = 1.25 * tertTotal; // use of double and later converting back to int


                bool addTest = insuree.DUI;

                int sQuote = insuree.DUI? Convert.ToInt32(tertTotal2) : tertTotal;



                Double secTotal = 1.5 * sQuote; // use of double and later converting back to int

                bool addTest2 = insuree.CoverageType;

                int ssecTotal2 = insuree.CoverageType ? Convert.ToInt32(secTotal) : sQuote;

                int primTotal = ssecTotal2;


                insuree.Quote = primTotal;////works - applies necessary percentage adders based on DUI or Coverage beinng checked




                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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
