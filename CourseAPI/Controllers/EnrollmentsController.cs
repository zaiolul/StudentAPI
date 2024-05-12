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

namespace CourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        Publisher publisher;
  
        public EnrollmentsController(Publisher publisher)
        {
            this.publisher = publisher;
          
        }

        [HttpPost]
        public void Post([FromBody] Enrollment en)
        {
            string data = JsonConvert.SerializeObject(en);
            publisher.Publish(data);
        }
    }
}
