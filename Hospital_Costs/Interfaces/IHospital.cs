using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Costs.Interfaces
{
    public interface IHospital
    {
        int Id { get; set; }
        int Hospital_Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        int Zip { get; set; }
        string RegionDescription { get; set; }
    }
}

