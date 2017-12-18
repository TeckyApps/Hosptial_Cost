using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Costs.Interfaces
{
    public interface IState
    {
        int Id { get; set; }
        string State_Name { get; set; }
        string State_Abbreviation { get; set; }
    }
}
