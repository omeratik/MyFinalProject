using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InovaController : ControllerBase
    {
        IInovaService _inovaService;

        public InovaController(IInovaService inovaService)
        {
            _inovaService = inovaService;

        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {

            var result = _inovaService.GetAll();
            if (result.Success) { 

                return Ok(result);
            }
            return BadRequest();
        }

        [HttpPost("add")]

        public ActionResult Add(Inova inova)
        {
            var result = _inovaService.Add(inova);
            if (result.Success)
            {

                return Ok(result);
            }

            return BadRequest(result);
        }



    }
}
