using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Data
{
    public interface IAWSService
    {
        Task<List<Employee>> GetAll();
        Task<Employee> GetById(string Id);
        Task<Employee> CreateEmployee(Employee employee);
        Task<Employee> UpdateEmployee(string Id, Employee employee);
        Task<Employee> DeleteEmployee(string Id);
        void CreateTable();
    }
}
