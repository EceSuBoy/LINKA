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
    public class EFCargoOperationDal : GenericRepository<CargoOperation>, ICargoOperationDal
    {
        public EFCargoOperationDal(CargoContext context) : base(context)
        {
        }
    }
}
