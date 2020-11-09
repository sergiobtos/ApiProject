using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Data
{
    public interface IRepository
    {
        //GetAll/getById/post/put/delete method

        IEnumerable<Employee> GetAll();

    }
}
