using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020DO601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020DO601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public platosController(restauranteContext restauranteContexto)
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
            List<platos> listadoPlato = (from e in _restauranteContexto.platos select e).ToList();

            if (listadoPlato.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPlato);
        }

        /// <sumary>
        /// EndPoint que retorna los registros de la tabla platos por una palabra del nombre del plato
        /// </sumary>
        /// <para name="id"></para>
        /// <returns></returns>
        [HttpGet]
        [Route("Buscar/{PalabraPlato}")]
        public IActionResult FindByPalabraPlato(string filtro)
        {
            platos? PalabraPlato = (from e in _restauranteContexto.platos
                                    where e.nombrePlato.Contains(filtro)
                                    select e).FirstOrDefault();

            if (PalabraPlato == null)
            {
                return NotFound();
            }

            return Ok(PalabraPlato);
        }
        /// Metodo para crear nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPlato([FromBody] platos plato)
        {
            try
            {
                _restauranteContexto.platos.Add(plato);
                _restauranteContexto.SaveChanges();
                return Ok(plato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Metodo para modificar un registro
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPlato(int id, [FromBody] platos platoModificar)
        {
            platos? platoActual = (from e in _restauranteContexto.platos
                                   where e.platoId == id
                                   select e).FirstOrDefault();

            if (platoActual == null)
            {
                return NotFound();
            }

            platoActual.nombrePlato = platoModificar.nombrePlato;
            platoActual.precio = platoModificar.precio;

            _restauranteContexto.Entry(platoActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(platoActual);


        }

        /// Metodo para eliminar un registro
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlato(int id)
        {
            platos? plato = (from e in _restauranteContexto.platos
                             where e.platoId == id
                             select e).FirstOrDefault();

            if (plato == null)
            {
                return NotFound();
            }

            _restauranteContexto.platos.Attach(plato);
            _restauranteContexto.platos.Remove(plato);
            _restauranteContexto.SaveChanges();

            return Ok(plato);
        }
    }
}
