﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Treehouse.FitnessFrog.Data;
using Treehouse.FitnessFrog.Models;

namespace Treehouse.FitnessFrog.Controllers
{
    public class EntriesController : Controller
    {
        private EntriesRepository _entriesRepository = null;

        public EntriesController()
        {
            _entriesRepository = new EntriesRepository();
        }

        public ActionResult Index()
        {
            List<Entry> entries = _entriesRepository.GetEntries();

            // Calculate the total activity.
            double totalActivity = entries
                .Where(e => e.Exclude == false)
                .Sum(e => e.Duration);

            // Determine the number of days that have entries.
            int numberOfActiveDays = entries
                .Select(e => e.Date)
                .Distinct()
                .Count();

            ViewBag.TotalActivity = totalActivity;
            ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

            return View(entries);
        }

        public ActionResult Add()
        {
            //instantiate a new model instance ( a blank form...with today's date as default )
            var entry = new Entry()
            {
                Date = DateTime.Today
            };
            return View(entry);
        }

        [HttpPost]
        public ActionResult Add(Entry entry)
        {
            if (ModelState.IsValid)
            {
                _entriesRepository.AddEntry(entry);

                //TODO Display the Entries List page instead of the Entry view
                List<Entry> entries = _entriesRepository.GetEntries();

                // Calculate the total activity.
                double totalActivity = entries
                    .Where(e => e.Exclude == false)
                    .Sum(e => e.Duration);

                // Determine the number of days that have entries.
                int numberOfActiveDays = entries
                    .Select(e => e.Date)
                    .Distinct()
                    .Count();

                ViewBag.TotalActivity = totalActivity;
                ViewBag.AverageDailyActivity = (totalActivity / (double)numberOfActiveDays);

                //return View(entries);
                return RedirectToAction("Index", entries);

            }


            return View(entry);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }
    }
}