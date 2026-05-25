using Linka.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Cargo.DataAccessLayer.Abstract
{
    public interface ICargoCustomerDal : IGenericDal<CargoCustomer>
    {
        CargoCustomer GetCargoCustomerById(string id);
    }
}
