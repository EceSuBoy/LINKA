using Linka.Cargo.DataAccessLayer.Abstract;
using Linka.Cargo.DataAccessLayer.Concrete;
using Linka.Cargo.DataAccessLayer.Repositories;
using Linka.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linka.Cargo.DataAccessLayer.EntityFramework
{
    public class EFCargoCustomerDal : GenericRepository<CargoCustomer>, ICargoCustomerDal
    {
        private readonly CargoContext _cargoContext;
        public EFCargoCustomerDal(CargoContext context, CargoContext cargoContext) : base(context)
        {
            _cargoContext = cargoContext;
        }

        public CargoCustomer GetCargoCustomerById(string id)
        {
            var values = _cargoContext.CargoCustomers.Where(x=>x.UserCustomerId == id).FirstOrDefault();
            return values;
        }
    }
}
