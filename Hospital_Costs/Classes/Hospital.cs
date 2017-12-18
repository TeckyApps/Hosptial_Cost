using Hospital_Costs.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hospital_Costs.Classes
{
    public class Hospital : IHospital
    {
        public string Address { get; set; }
        public string City { get; set; }
        public int Hospital_Id { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegionDescription { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }
}