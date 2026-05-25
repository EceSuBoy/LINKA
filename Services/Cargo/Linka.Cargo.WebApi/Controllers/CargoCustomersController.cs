using Linka.Cargo.BusinessLayer.Abstract;
using Linka.Cargo.DtoLayer.Dtos.CargoCustomerDtos;
using Linka.Cargo.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Linka.Cargo.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CargoCustomersController : ControllerBase
    {
        private readonly ICargoCustomerService _cargoCustomerService;

        public CargoCustomersController(ICargoCustomerService cargoCustomerService)
        {
            _cargoCustomerService = cargoCustomerService;
        }
        [HttpGet]
        public IActionResult CargoCustomerList()
        {
            var result = _cargoCustomerService.TGetAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetCargoCustomerById(int id)
        {
            var result = _cargoCustomerService.TGetById(id);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult CreateCargoCustomer(CreateCargoCustomerDto createCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                Name = createCargoCustomerDto.Name,
                Surname = createCargoCustomerDto.Surname,
                Email = createCargoCustomerDto.Email,
                Phone = createCargoCustomerDto.Phone,
                District = createCargoCustomerDto.District,
                City = createCargoCustomerDto.City,
                Address = createCargoCustomerDto.Address,
                UserCustomerId = createCargoCustomerDto.UserCustomerId
            };
            _cargoCustomerService.TInsert(cargoCustomer);
            return Ok("Cargo Customer Created Successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult RemoveCargoCustomer(int id)
        {
            _cargoCustomerService.TDelete(id);
            return Ok("Cargo Customer Deleted Successfully");
        }
        [HttpPut]
        public IActionResult UpdateCargoCustomer(UpdateCargoCustomerDto updateCargoCustomerDto)
        {
            CargoCustomer cargoCustomer = new CargoCustomer()
            {
                CargoCustomerId = updateCargoCustomerDto.CargoCustomerId,
                Name = updateCargoCustomerDto.Name,
                Surname = updateCargoCustomerDto.Surname,
                Email = updateCargoCustomerDto.Email,
                Phone = updateCargoCustomerDto.Phone,
                District = updateCargoCustomerDto.District,
                City = updateCargoCustomerDto.City,
                Address = updateCargoCustomerDto.Address
            };
            _cargoCustomerService.TUpdate(cargoCustomer);
            return Ok("Cargo Customer Updated Successfully");
        }
        [HttpGet("GetCargoCustomerById")]
        public IActionResult GetCargoCustomerById(string id)
        {
            return Ok(_cargoCustomerService.TGetCargoCustomerById(id));
        }
    }
}
