using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.Model.Internal.MarshallTransformations;
using ApiProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiProject.Data
{
    public class AWSServices : IAWSService
    {

        RegionEndpoint Region = RegionEndpoint.CACentral1;
        IAmazonDynamoDB dynamoDBClient { get; set; }
        public AWSServices(IAmazonDynamoDB dynamoDBClient)
        {
            this.dynamoDBClient = dynamoDBClient;
            CreateTable();
        }

        //GetAll
        public async Task<List<Employee>> GetAll()
        {
            DynamoDBContext Context = new DynamoDBContext(dynamoDBClient);
            var conditions = new List<ScanCondition>();
            var employees = await Context.ScanAsync<Employee>(conditions).GetRemainingAsync();

            return employees;
        }

        //GetById
        public Task<Employee> GetById(int Id)
        {
            DynamoDBContext Context = new DynamoDBContext(dynamoDBClient);
            return Context.LoadAsync<Employee>(Id, default(System.Threading.CancellationToken));
        }

        //post
        public async Task<Employee> CreateEmployee(Employee employee)
        {
            DynamoDBContext Context = new DynamoDBContext(dynamoDBClient);
            await Context.SaveAsync<Employee>(employee);
            return await GetById(employee.Id);
        }

        //put
        public async Task UpdateEmployee(int Id, Employee employee)
        {
            var key = new Dictionary<string, AttributeValue>
            {
                { "Id", new AttributeValue { N = Id.ToString() } }
            };
            var result = await dynamoDBClient.UpdateItemAsync("Employee", key, new Dictionary<string, AttributeValueUpdate>
            {
                { "FullName", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = employee.FullName } } },
                { "Email", new AttributeValueUpdate { Action = AttributeAction.PUT , Value = new AttributeValue { S = employee.Email } } },
                { "Address", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = employee.Address} } },
                { "Role", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = employee.Role} } },
                { "Salary", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { N = employee.Salary.ToString()} } },
                { "StartContractDate", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = employee.StartContractDate.ToString()} } },
                { "EndContractDate", new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = employee.EndContractDate.ToString()} } },
            });
        }

        //delete
        public async Task DeleteEmployee(int Id)
        {
            DynamoDBContext Context = new DynamoDBContext(dynamoDBClient);
            await Context.DeleteAsync<Employee>(Id); 
        }

        public void CreateTable()
        {
            String tableName = "Employee";
            Task<ListTablesResponse> table = dynamoDBClient.ListTablesAsync();
            List<string> currentTables = table.Result.TableNames;
            bool tablesAdded = false;
            if (!currentTables.Contains(tableName))
            {
                dynamoDBClient.CreateTableAsync(new CreateTableRequest
                {
                    TableName = tableName,
                    ProvisionedThroughput = new ProvisionedThroughput { ReadCapacityUnits = 3, WriteCapacityUnits = 1 },
                    KeySchema = new List<KeySchemaElement>
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = KeyType.HASH
                        }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new AttributeDefinition { AttributeName = "Id", AttributeType = ScalarAttributeType.N}
                    },
                });
                tablesAdded = true;
            }

            if (tablesAdded)
            {
                bool allActive;
                do
                {
                    allActive = true;
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    TableStatus tableStatus = GetTableStatus(tableName);
                    if (!object.Equals(tableStatus, TableStatus.ACTIVE))
                        allActive = false;

                } while (!allActive);
            }
        }

        private TableStatus GetTableStatus(string tableName)
        {
            try
            {
                Task<DescribeTableResponse> tableResp = dynamoDBClient.DescribeTableAsync(new DescribeTableRequest { TableName = tableName });
                TableDescription table = tableResp.Result.Table;
                return (table == null) ? null : table.TableStatus;
            }
            catch (AmazonDynamoDBException db)
            {
                if (db.ErrorCode == "ResourceNotFoundException")
                    return string.Empty;
                throw;
            }
        }
        ////end
    }
}
