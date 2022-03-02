using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTO
{
    public class EmployeeDTO
    {
        public List<POSITION> Positions { get; set; }
        public List<DEPARTMENT> Departments { get; set; }
        public List<EmployeeDetailDTO> Employees { get; set; }
    }
}
