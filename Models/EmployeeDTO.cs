using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    public class EmployeeDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartContractDate { get; set; }
        public DateTime EndContractDate { get; set; }
    }
}
