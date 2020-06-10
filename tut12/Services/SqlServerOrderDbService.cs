using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tut12.Models;

namespace tut12.Services
{
    public class SqlServerOrderDbService : IOrderDbService
    {

        private readonly DBContext _dbcontext;
        public SqlServerOrderDbService(DBContext context)
        {
            _dbcontext = context;
        }
        public IEnumerable getOrders(string name)
        {
              if (name != null) {
            
                  var res = _dbcontext.Customer.Any(e => e.Name == name);

                  if (res == true)
                  {

                      var id = _dbcontext.Customer.Where(e => e.Name == name).Select(e=>e.IdClient).FirstOrDefault();
                    //var res1 =_dbcontext.Order.
                    /*
                    var res2 = _dbcontext.Order.Where(e => e.IdClient == id).Join(_dbcontext.Confectionery_Order, e => e.IdOrder, d => d.IdOrder,
                        (e, d) => new
                        {
                            Order = e,
                            Confectionery_Order1 = d
                        }).Join(_dbcontext.Confectionery, e => e.Confectionery_Order1.IdConfectionery, d => d.IdConfectionery,
                        (e, d) => new
                        {
                            Confectionery = d,
                            Confectionery_Order2 = e
                        }).Select(e => new {
                            e.Confectionery_Order2.Order.IdOrder,
                            e.Confectionery_Order2.Order.DateAccepted,
                            e.Confectionery_Order2.Order.DateFinished,
                            e.Confectionery_Order2.Order.Notes,
                            //e.Confectionery,

                            Confectionery = new
                            {
                                e.Confectionery.IdConfectionery,
                                e.Confectionery.Name,
                               e.Confectionery.PricePerItem,
                                e.Confectionery_Order2.Confectionery_Order1.Quantity

                            }

                        }).ToList();

                  //return Ok(res2);
                  */
                    var res3 = _dbcontext.Customer.Include(e => e.Orders).Where(e => e.Name == name)
                                                       .Select(e => new
                                                       {

                                                           Orders = e.Orders
                                                           .Join(_dbcontext.Confectionery_Order, e => e.IdOrder, d => d.IdOrder,
                          (e, d) => new
                          {
                              Order = e,
                              Confectionery_Order1 = d
                          }).Join(_dbcontext.Confectionery, e => e.Confectionery_Order1.IdConfectionery, d => d.IdConfectionery,
                          (e, d) => new
                          {
                              Confectionery = d,
                              Confectionery_Order2 = e
                          }).Select(e => new
                          {
                              e.Confectionery_Order2.Order.IdOrder,
                              e.Confectionery_Order2.Order.DateAccepted,
                              e.Confectionery_Order2.Order.DateFinished,
                              e.Confectionery_Order2.Order.Notes,
                              e.Confectionery
                          }).ToList()
                                                       });
                              /*
                              Confectionery = new
                              {
                                  e.Confectionery.IdConfectionery,
                                  e.Confectionery.Name,
                                  e.Confectionery.PricePerItem,
                                  e.Confectionery_Order2.Confectionery_Order1.Quantity


                                                           // .Select(e => e.IdPrescription)

                                                           .ToList()
                                                       });
                                                       */
                    return res3;
                  }
                  else
                  {
                    //return NotFound("There is no customer with name " + name);
                    throw new Exception("There is no customer with name " + name);
                  }
                 
              } else
              {
                // var list = _dbcontext.Order.Select(e => new { e.IdOrder, e.DateAccepted, e.DateFinished, e.Notes }).ToList();

               

                var list = _dbcontext.Order.Join(_dbcontext.Confectionery_Order, e => e.IdOrder, d => d.IdOrder,
                          (e, d) => new
                          {
                              Order = e,
                              Confectionery_Order1 = d
                          }).Join(_dbcontext.Confectionery, e => e.Confectionery_Order1.IdConfectionery, d => d.IdConfectionery,
                          (e, d) => new
                          {
                              Confectionery = d,
                              Confectionery_Order2 = e
                          }).Select(e => new {
                              e.Confectionery_Order2.Order.IdOrder,
                              e.Confectionery_Order2.Order.DateAccepted,
                              e.Confectionery_Order2.Order.DateFinished,
                              e.Confectionery_Order2.Order.Notes,
                              Confectionery= new
                              {
                                  e.Confectionery.IdConfectionery,
                                  e.Confectionery.Name,
                                  e.Confectionery.PricePerItem,
                                  e.Confectionery_Order2.Confectionery_Order1.Quantity
                              }
                              
                          }).ToList();
                //return Ok(list);'


                return list;
               }
        }


    }
}
