using Hospital_Costs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Models
{
    public class IndexViewModel : Diagnosable, IHospital
    {
        public string Address { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
        public int Code { get; set; }
        public int Diagnosis_Id { get; set; }
        public string DRG_Definition { get; set; }
        public int Hospital_Id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionDescription { get; set; }
        public string State { get; set; }
        public string Total_Cost { get; set; }
        public double Total_Medicare_Payments { get; set; }
        public double Total_Payments { get; set; }
        public int Total { get; set; }
    }
}