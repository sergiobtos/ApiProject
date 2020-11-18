using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Models
{
    [DynamoDBTable("Employee")]
    public class Employee
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBProperty]
        public string FullName { get; set; }
        [DynamoDBProperty]
        public string Email { get; set; }
        [DynamoDBProperty]
        public string Address { get; set; }
        [DynamoDBProperty]
        public string Role { get; set; }
        [DynamoDBProperty]
        public decimal Salary { get; set; }
        [DynamoDBProperty]
        public DateTime StartContractDate { get; set; }
        [DynamoDBProperty]
        public DateTime EndContractDate { get; set; }
    }
}
