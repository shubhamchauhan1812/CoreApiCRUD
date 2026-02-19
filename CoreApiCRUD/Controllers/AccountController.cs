using CoreApiCRUD.Data;
using CoreApiCRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiCRUD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext context;
        public AccountController(ApplicationContext _context)
        {
            context = _context;
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ReactInsert model)
        {
           bool success = false;
            var customer = await context.Customers.Where(c => c.email == model.email).FirstOrDefaultAsync();
            if(customer == null)
            {
                return NotFound(success);
            }
            else {
                success = true;
                return Ok(success);
            }
            
        }
        [HttpPut("UpdateData")]
        public async Task<IActionResult> UpdateData([FromBody] ReactInsert model)
        {
           
            var customer = await context.ReactInsert.Where(c => c.Id == model.Id).FirstOrDefaultAsync();
            customer.firstName = model.firstName;
            customer.lastName = model.lastName;
            customer.email = model.email;
            
            await context.SaveChangesAsync();
            return Ok(customer);

        }
        [HttpPatch]
        public async Task<IActionResult> PatchData([FromBody] ReactInsert model)
        {
            bool success = false;
            var customer = await context.Customers.Where(c => c.email == model.email).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound(success);
            }
            else
            {
                success = true;
                return Ok(success);
            }

        }
        public IActionResult Data2()
        {
            return Ok();
        }
        public IActionResult Data4()
        {
            return Ok();
        }
    }
}
