using Linka.Cargo.BusinessLayer.Abstract;
using Linka.Cargo.DtoLayer.Dtos.CargoDetailDtos;
using Linka.Cargo.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoDetailsController : ControllerBase
    {
        private readonly ICargoDetailService _CargoDetailService;

        public CargoDetailsController(ICargoDetailService CargoDetailService)
        {
            _CargoDetailService = CargoDetailService;
        }

        [HttpGet]
        public IActionResult CargoDetailList()
        {
            var result = _CargoDetailService.TGetAll();
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateCargoDetail(CreateCargoDetailDto createCargoDetailDto)
        {
            CargoDetail CargoDetail = new CargoDetail()
            {
                Barcode= createCargoDetailDto.Barcode,
                CargoCompanyId = createCargoDetailDto.CargoCompanyId,
                ReceiverCustomer = createCargoDetailDto.ReceiverCustomer,
                SenderCustomer = createCargoDetailDto.SenderCustomer
            };
            _CargoDetailService.TInsert(CargoDetail);
            return Ok("Cargo Details Created Successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult RemoveCargoDetail(int id)
        {
            _CargoDetailService.TDelete(id);
            return Ok("Cargo Details Deleted Successfully");
        }
        [HttpGet("{id}")]
        public IActionResult GetCargoDetailById(int id)
        {
            var result = _CargoDetailService.TGetById(id);
            return Ok(result);
        }
        [HttpPut]
        public IActionResult UpdateCargoDetail(UpdateCargoDetailDto updateCargoDetailDto)
        {
            CargoDetail CargoDetail = new CargoDetail()
            {
                Barcode = updateCargoDetailDto.Barcode,
                CargoCompanyId = updateCargoDetailDto.CargoCompanyId,
                CargoDetailId= updateCargoDetailDto.CargoDetailId,
                ReceiverCustomer = updateCargoDetailDto.ReceiverCustomer,
                SenderCustomer = updateCargoDetailDto.SenderCustomer,
            };
            _CargoDetailService.TUpdate(CargoDetail);
            return Ok("Cargo Details Updated Successfully");
        }
    }
}
