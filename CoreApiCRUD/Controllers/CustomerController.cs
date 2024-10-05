using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiCRUD.Models;
using Microsoft.EntityFrameworkCore;
using CoreApiCRUD.Data;

namespace CoreApiCRUD.Controllers
{
    [Route("api/[controller]")]
    
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationContext context;
        public CustomerController(ApplicationContext context)
        {
            this.context = context;
        }
        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var data = context.Customers.ToList();
            if(data.Count() == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(data);
            }
            
            
        }

        

        [HttpGet]
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = context.Customers.Where(e => e.Id == id).SingleOrDefault();
                if(data == null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(data);
                }
            }

        }


        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer([FromBody] Customer model)
        {
           if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                context.Customers.Add(model);
                context.SaveChanges();
                return Ok();
              
            }

        }
        
        [HttpPost]
        [Route("ReactAdd")]
        public IActionResult ReactAdd([FromBody] ReactInsert model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                context.ReactInsert.Add(model);
                context.SaveChanges();
                return Ok("Data saved successfully");

            }

        }
        [HttpPut]
        [Route("UpdateCustomer")]
        public IActionResult UpdateCustomer([FromBody] Customer model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            else
            {
                var data = context.Customers.Where(e => e.Id == model.Id).SingleOrDefault();
                if (data == null)
                {
                    return BadRequest();
                }
                else
                {
                    data.Name = model.Name;
                    data.Gender = model.Gender;
                    data.IsActive = model.IsActive;
                    context.Customers.Update(data);
                    context.SaveChanges();
                    return Ok();


                }
              

            }


        }
        [HttpDelete]
        [Route("DeleteCustomers/{id}")]
        public IActionResult DeleteCustomers(int id)
        {
            if(id != 0)
            {
                var data = context.Customers.Where(e => e.Id == id).SingleOrDefault();
                if(data == null)
                {
                    return BadRequest();
                }
                else
                {
                    context.Customers.Remove(data);
                    context.SaveChanges();
                }
            }
            else
            {
                return BadRequest();
            }
            return Ok();

        }
       
    }
}
