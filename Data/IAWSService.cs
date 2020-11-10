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
        Task<Employee> GetById(int Id);
        Task<Employee> CreateEmployee(Employee employee);
        Task UpdateEmployee(int Id, Employee employee);
        Task DeleteEmployee(int Id);
        void CreateTable();
    }
}
