using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Domain.Interfaces.Model
{
    public interface ISelectable
    {
        bool IsActive { get; set; }
    }
}
