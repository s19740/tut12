using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tut12.DTOs;
using tut12.Models;
using tut12.Services;

namespace tut12.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class NewOrderController : ControllerBase
    {

        private readonly DBContext _dbcontext;  //db

        public NewOrderController(DBContext context)
        {
            _dbcontext = context;
        }

        [HttpPost("{id}/orders")]
        public IActionResult newOrder(int id,NewOrderRequest req)
        {
            var res = _dbcontext.Customer.Any(e => e.IdClient == id);

            if (res == true)
            {
                var result = _dbcontext.Confectionery.Where(x => req.Confectionery.Select(e=>e.Name).Contains(x.Name)).Any();
                _dbcontext.Database.BeginTransaction();
                
                if (result==true)
                {
                    try
                    {

                        var newOrd = new Order
                        {
                            DateAccepted = req.DateAccepted,
                            DateFinished = req.DateAccepted.AddDays(7),
                            Notes = req.Notes,
                            IdClient = id,
                            IdEmployee = 1
                        };
                        _dbcontext.Order.Add(newOrd);
                        _dbcontext.SaveChanges();
                        for (int i = 0; i < req.Confectionery.Count(); i++)
                        {
                            var add = new Confectionery_Order
                            {
                                IdConfectionery = _dbcontext.Confectionery.Where(e => e.Name == req.Confectionery.ElementAt(i).Name)
                                                            .Select(e => e.IdConfectionery).FirstOrDefault(),
                                IdOrder = _dbcontext.Order.Max(e => e.IdOrder),
                                Quantity = Int32.Parse(req.Confectionery.ElementAt(i).Quantity),
                                Notes = req.Confectionery.ElementAt(i).Notes

                            };
                            _dbcontext.Confectionery_Order.Add(add);
                            _dbcontext.SaveChanges();
                        }
                        _dbcontext.Database.CommitTransaction();
                        return Ok("Added succesfully!");
                    }
                    catch (Exception e)
                    {
                        _dbcontext.Database.RollbackTransaction();
                        return BadRequest("there is a problem");
                    }
                }else
                {
                    return BadRequest("dont have this product");
                }
                
            }
            else
            {
                return NotFound("There is no client with id " + id);
            }
        }


    }
}