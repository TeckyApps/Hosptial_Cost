using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Costs.Interfaces
{
    public interface Diagnosable
    {
        int Id { get; set; }
        int Code { get; set; }
        string DRG_Definition { get; set; }
    }
}
