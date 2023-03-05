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

        /// <sumary>
        /// EndPoint que retorna los registros de la tabla clientes por una plabara de la direccion
        /// </sumary>
        /// <para name="id"></para>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{filtro}")]
        public IActionResult FindByPalabraDireccion(string filtro)
        {
            clientes? PalabraDireccion = (from e in _restauranteContexto.clientes
                                    where e.direccion.Contains(filtro)
                                    select e).FirstOrDefault();

            if (PalabraDireccion == null)
            {
                return NotFound();
            }

            return Ok(PalabraDireccion);
        }
        /// Metodo para crear nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCliente([FromBody] clientes cliente) 
        {
            try 
            {
                _restauranteContexto.clientes.Add(cliente);
                _restauranteContexto.SaveChanges();
                return Ok(cliente);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// Metodo para modificar un registro
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarCliente(int id, [FromBody] clientes clienteModificar) 
        {
            clientes? clienteActual = (from e in _restauranteContexto.clientes
                                       where e.clienteId == id
                                       select e).FirstOrDefault();

            if (clienteActual == null) 
            {
                return NotFound();
            }

            clienteActual.nombreCliente = clienteModificar.nombreCliente;
            clienteActual.direccion = clienteModificar.direccion;

            _restauranteContexto.Entry(clienteActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(clienteActual);


        }

        /// Metodo para eliminar un registro
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarCliente(int id) 
        {
            clientes? cliente = (from e in _restauranteContexto.clientes
                                 where e.clienteId == id
                                 select e).FirstOrDefault();

            if (cliente == null) 
            {
                return NotFound();
            }

            _restauranteContexto.clientes.Attach(cliente);
            _restauranteContexto.clientes.Remove(cliente);
            _restauranteContexto.SaveChanges();

            return Ok(cliente);
        }

    }
}
