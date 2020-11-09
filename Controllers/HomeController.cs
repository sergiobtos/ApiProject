using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {

        private IAmazonDynamoDB dynamoDBClient;
        private IAmazonS3 s3Client;

        public HomeController(IAmazonDynamoDB dynamoDBClient, IAmazonS3 s3Client)
        {
            this.dynamoDBClient = dynamoDBClient;
            this.s3Client = s3Client;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
