using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;

namespace NSE.Catalogo.API.Controllers
{
    [ApiController]
    public class CatalogoController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet("catalogo/produtos")]
        public async Task<IEnumerable<Produto>> Index()
        {
            return await _produtoRepository.ObterTodos();
        }

        [HttpGet("catalogo/produtos/{id}")]
        public async Task<Produto> ProdutoDetalhe(Guid id)
        {
            return await _produtoRepository.ObterPorId(id);
        }

        //// POST api/<CatalogoController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CatalogoController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CatalogoController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
