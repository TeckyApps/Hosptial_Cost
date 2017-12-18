using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Costs.Interfaces
{
    public interface Chargeable
    {
        int Id { get; set; }
        string Total_Cost { get; set; }
        double Total_Payments { get; set; }
        double Total_Medicare_Payments { get; set; }
        Diagnosable Current_Diagnosis { get; set; }
        IHospital Current_Hospital { get; set; }
    }
}
