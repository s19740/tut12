using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tut12.Services
{
    public interface IOrderDbService
    {
        IEnumerable getOrders(string name);
   
    }
}
