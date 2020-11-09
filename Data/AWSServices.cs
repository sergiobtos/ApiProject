using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiProject.Data
{
    public class AWSServices
    {
        RegionEndpoint Region = RegionEndpoint.CACentral1;
        IAmazonDynamoDB dynamoDBClient { get; set; }
        IAmazonS3 s3Client { get; set; }
        public AWSServices(IAmazonDynamoDB dynamoDBClient, IAmazonS3 s3Client)
        {
            this.dynamoDBClient = dynamoDBClient;
            this.s3Client = s3Client;
            CreateTable();
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
                        new AttributeDefinition { AttributeName = "Id", AttributeType = ScalarAttributeType.S}
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
