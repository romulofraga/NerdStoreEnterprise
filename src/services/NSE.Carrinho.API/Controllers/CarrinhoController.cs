using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Model;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Usuario;

namespace NSE.Carrinho.API.Controllers
{
    [Authorize]
    [Route("carrinho")]
    public class CarrinhoController : MainController
    {
        private readonly IAspnetUser _user;
        private readonly CarrinhoContext _dbContext;

        public CarrinhoController(CarrinhoContext dbContext, IAspnetUser user)
        {
            _dbContext = dbContext;
            _user = user;
        }
        [HttpGet("")]
        public async Task<CarrinhoCliente> ObterCarrinho()
        {

            return await ObterCarrinho() ?? new CarrinhoCliente();
        }

        [HttpPost("")]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();

            if (carrinho == null) ManipularNovoCarrinho(item);

            else ManipularCarrinhoExistente(carrinho, item);

            if (!OperacaoValida()) return CustomResponse();

            var result = await _dbContext.SaveChangesAsync().ConfigureAwait(true);
            if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir os dados no banco");

            return CustomResponse();
        }

        private void ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_user.ObterUserId());

            carrinho.AdicionarItem(item);

            _dbContext.CarrinhoCliente.Add(carrinho);
        }

        private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoItemExistente = carrinho.CarrinhoItemExistente(item);

            carrinho.AdicionarItem(item);

            if (produtoItemExistente)
            {
                _dbContext.CarrinhoItems.Update(carrinho.ObterProdutoId(item.ProdutoId));
            }
            else
            {
                _dbContext.CarrinhoItems.Add(item);
            }

            _dbContext.CarrinhoCliente.Update(carrinho);
        }

        [HttpPost("{produtoId}")]
        public async Task<IActionResult> AtualizarCarrinho(Guid produtoId, CarrinhoItem item)
        {
            return CustomResponse();
        }

        [HttpDelete("{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            return CustomResponse();
        }

        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _dbContext.CarrinhoCliente
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClienteId == _user.ObterUserId());
        }

    }
}
