using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseAPI.Data;
using CourseAPI.Models;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;
using MassTransit;
using ClassLibrary;

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        Publisher publisher;
        IPublishEndpoint publishEndpoint;
  
        public EnrollmentsController(Publisher publisher, IPublishEndpoint publishEndpoint)
        {
            this.publisher = publisher;
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public void Post([FromBody] Enrollment en)
        {
            string data = JsonConvert.SerializeObject(en);
            publisher.Publish(data);
        }

        [HttpPost("mtmmsg")]
        public async void  PublishMessage(string msg)
        {
            var mtmmsg = new MTMsg
            {
                Timestamp = DateTime.Now,
                Content = msg
            };
            await publishEndpoint.Publish(mtmmsg);
        }
    }
  
}
