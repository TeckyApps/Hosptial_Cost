using Hospital_Costs.Classes;
using Hospital_Costs.Interfaces;
using Hospital_Costs.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hospital_Costs.Controllers
{
    public class IndexController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Charges_Read([DataSourceRequest]DataSourceRequest request, string state, string numberOfResults)
        {
            return Json(GetQuerySelectionResults(state, Convert.ToInt32(numberOfResults)).ToDataSourceResult(request));
        }
        [HttpPost]
        public ActionResult Read_DiagnosisTotal(string state)
        {
            if (string.IsNullOrEmpty(state))
                state = "All";
            return Json(GetDiagnosisTotal(state));
        }
        private static IEnumerable<Diagnosis> GetDiagnosisTotal(string state)
        {
            var diagnosis = new Diagnosis();
            return diagnosis.ReadDiagnosisCount(state);
        }
        public JsonResult Charges_ReadLowest(string state)
        {
            return Json(GetLowest(state), JsonRequestBehavior.AllowGet); // return the SayHello value of the current instance of the World object.
        }
        public JsonResult Charges_ReadHighest(string state)
        {
            return Json(GetHightest(state), JsonRequestBehavior.AllowGet); // return the SayHello value of the current instance of the World object.
        }
        public string GetHightest(string state)
        {
            string highest = null;
            if (state.ToLower() == "all")
            {
                IEnumerable<IndexViewModel> value = GetChargesTopResults("All", 1);
                foreach (var item in value)
                {
                    highest = item.Name + " $" + item.Total_Cost;
                }
            }
            else
            {
                IEnumerable<IndexViewModel> value = GetChargesTopResults(state, 1);
                foreach (var item in value)
                {
                    highest = item.Name + " $" + item.Total_Cost;
                }
            }
            return highest;
        }
        //   Method that retrieves the hello text
        public string GetLowest(string state)
        {
            string lowest = null;
            if (state.ToLower() == "all")
            {
                IEnumerable<IndexViewModel> value = GetChargesLowestResults("All", 1);
                foreach (var item in value)
                {
                    lowest = item.Name + " $" + item.Total_Cost;
                }
            }
            else
            {
                IEnumerable<IndexViewModel> value = GetChargesLowestResults(state, 1);
                foreach (var item in value)
                {
                    lowest = item.Name + " $" + item.Total_Cost;
                }
            }
            return lowest;
        }
        public IEnumerable<IndexViewModel> GetQuerySelectionResults(string state, int numberOfResults)
        {
            if (state.ToLower() == "all")
            {
                 return GetChargesTopResults("All", numberOfResults);
            }
            else
            {
                return GetChargesTopResults(state, numberOfResults);
            }   
        }

        // Get the states for the dropdown
        public ActionResult States_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetStates().ToDataSourceResult(request));
        }
        private static IEnumerable<IState> GetStates()
        {
            var states = new State();
            return states.Read();
        }

        private static IEnumerable<IndexViewModel> GetChargesLowestResults(string state, int numberOfResults)
        {
            var charge = new Charge();
            return charge.ReadLowestResults(state, numberOfResults).Select(current_charge => new IndexViewModel
            {
                Id = current_charge.Id,
                Name = current_charge.Current_Hospital.Name,
                Address = current_charge.Current_Hospital.Address,
                City = current_charge.Current_Hospital.City,
                State = current_charge.Current_Hospital.State,
                Zip = current_charge.Current_Hospital.Zip,
                RegionDescription = current_charge.Current_Hospital.RegionDescription,
                Total_Cost = current_charge.Total_Cost,
                Total_Medicare_Payments = current_charge.Total_Medicare_Payments,
                Total_Payments = current_charge.Total_Payments,
                Diagnosis_Id = current_charge.Current_Diagnosis.Id,
                Code = current_charge.Current_Diagnosis.Code,
                DRG_Definition = current_charge.Current_Diagnosis.DRG_Definition
            });
        }

        private static IEnumerable<IndexViewModel> GetChargesTopResults(string state, int numberOfResults)
        {
            var charge = new Charge();
            return charge.Read(state, numberOfResults).Select(current_charge => new IndexViewModel
            {
                Id = current_charge.Id,
                Name = current_charge.Current_Hospital.Name,
                Address = current_charge.Current_Hospital.Address,
                City = current_charge.Current_Hospital.City,
                State = current_charge.Current_Hospital.State,
                Zip = current_charge.Current_Hospital.Zip,
                RegionDescription = current_charge.Current_Hospital.RegionDescription,
                Total_Cost = current_charge.Total_Cost,
                Total_Medicare_Payments = current_charge.Total_Medicare_Payments,
                Total_Payments = current_charge.Total_Payments,
                Diagnosis_Id = current_charge.Current_Diagnosis.Id,
                Code = current_charge.Current_Diagnosis.Code,
                DRG_Definition = current_charge.Current_Diagnosis.DRG_Definition
            });
        }
    }
}