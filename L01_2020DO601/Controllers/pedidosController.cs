using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020DO601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020DO601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContexto;

        public pedidosController(restauranteContext restauranteContexto)
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
            List<pedidos> listadoPedido = (from e in _restauranteContexto.pedidos select e).ToList();

            if (listadoPedido.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoPedido);
        }

        /// <summary>
        /// EndPoint que retorna los registros de la tabla pedidos por medio de clienteId
        /// </summary>
        /// <para name="id"></para>
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindByClienteId(int filtro)
        {
            pedidos? ClienteId = (from e in _restauranteContexto.pedidos
                                  where e.clienteId == filtro
                                  select e).FirstOrDefault();

            if (ClienteId == null) 
            {
                return NotFound();
            }

            return Ok(ClienteId);
        }

        /// <summary>
        /// EndPoint que retorna los registros de la tabla pedidos por medio de motoristaId
        /// </summary>
        /// <para name="id"></para>
        [HttpGet]
        [Route("Find/{filtro}")]

        public IActionResult FindByMotoristaId(int filtro)
        {
            pedidos? MotoristaId = (from e in _restauranteContexto.pedidos
                                  where e.motoristaId == filtro
                                  select e).FirstOrDefault();

            if (MotoristaId == null)
            {
                return NotFound();
            }

            return Ok(MotoristaId);
        }

        /// Metodo para crear nuevo registro
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] pedidos pedido)
        {
            try
            {
                _restauranteContexto.pedidos.Add(pedido);
                _restauranteContexto.SaveChanges();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// Metodo para modificar un registro
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPedido(int id, [FromBody] pedidos pedidoModificar)
        {
            pedidos? pedidoActual = (from e in _restauranteContexto.pedidos
                                       where e.pedidoId == id
                                       select e).FirstOrDefault();

            if (pedidoActual == null)
            {
                return NotFound();
            }

            pedidoActual.motoristaId = pedidoModificar.motoristaId;
            pedidoActual.clienteId = pedidoModificar.clienteId;
            pedidoActual.platoId = pedidoModificar.platoId;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;

            _restauranteContexto.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteContexto.SaveChanges();

            return Ok(pedidoActual);


        }

        /// Metodo para eliminar un registro
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPedido(int id)
        {
            pedidos? pedido = (from e in _restauranteContexto.pedidos
                                 where e.pedidoId == id
                                 select e).FirstOrDefault();

            if (pedido == null)
            {
                return NotFound();
            }

            _restauranteContexto.pedidos.Attach(pedido);
            _restauranteContexto.pedidos.Remove(pedido);
            _restauranteContexto.SaveChanges();

            return Ok(pedido);
        }
    }
}
