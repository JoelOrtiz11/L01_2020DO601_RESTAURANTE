using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020DO601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020DO601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientesController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public clientesController(restauranteContext restauranteContexto) 
        {
            _restauranteContexto = restauranteContexto;
        }

        /// <summary>
        /// EndPoint que retorna el listado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Get() 
        {
            List<clientes> listadoCliente = (from e in _restauranteContexto.clientes select e).ToList();

            if (listadoCliente.Count == 0) 
            {
                return NotFound();
            }
            return Ok(listadoCliente);
        }
    }
}
