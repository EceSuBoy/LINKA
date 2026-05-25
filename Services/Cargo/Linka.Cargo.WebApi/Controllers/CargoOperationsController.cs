using Linka.Cargo.BusinessLayer.Abstract;
using Linka.Cargo.DtoLayer.Dtos.CargoOperationDtos;
using Linka.Cargo.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoOperationsController : ControllerBase
    {
        public class CargoCompaniesController : ControllerBase
        {
            private readonly ICargoOperationService _CargoOperationService;

            public CargoCompaniesController(ICargoOperationService CargoOperationService)
            {
                _CargoOperationService = CargoOperationService;
            }

            [HttpGet]
            public IActionResult CargoOperationList()
            {
                var result = _CargoOperationService.TGetAll();
                return Ok(result);
            }
            [HttpPost]
            public IActionResult CreateCargoOperation(CreateCargoOperationDto createCargoOperationDto)
            {
                CargoOperation CargoOperation = new CargoOperation()
                {
                    Barcode = createCargoOperationDto.Barcode,
                    Description = createCargoOperationDto.Description,
                    OperationDate = createCargoOperationDto.OperationDate
                };
                _CargoOperationService.TInsert(CargoOperation);
                return Ok("Cargo Operation Created Successfully");
            }
            [HttpDelete("{id}")]
            public IActionResult RemoveCargoOperation(int id)
            {
                _CargoOperationService.TDelete(id);
                return Ok("Cargo Operation Deleted Successfully");
            }
            [HttpGet("{id}")]
            public IActionResult GetCargoOperationById(int id)
            {
                var result = _CargoOperationService.TGetById(id);
                return Ok(result);
            }
            [HttpPut]
            public IActionResult UpdateCargoOperation(UpdateCargoOperationDto updateCargoOperationDto)
            {
                CargoOperation CargoOperation = new CargoOperation()
                {
                    Barcode = updateCargoOperationDto.Barcode,
                    CargoOperationId = updateCargoOperationDto.CargoOperationId,
                    Description = updateCargoOperationDto.Description,
                    OperationDate = updateCargoOperationDto.OperationDate,
                };
                _CargoOperationService.TUpdate(CargoOperation);
                return Ok("Cargo Operation Updated Successfully");
            }
        }
    }
}
