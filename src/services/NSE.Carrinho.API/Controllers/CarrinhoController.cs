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

            return await ObterCarrinhoCliente() ?? new CarrinhoCliente();
        }

        [HttpPost("")]
        public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();

            if (carrinho == null) ManipularNovoCarrinho(item);

            else ManipularCarrinhoExistente(carrinho, item);

            if (!OperacaoValida()) return CustomResponse();

            await PersistirDados();

            return CustomResponse();
        }

        [HttpPut("{produtoId}")]
        public async Task<IActionResult> AtualizarCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await ObterCarrinhoCliente();
            var itemCarrinho = await ObterCarrinhoValidado(produtoId, carrinho, item);

            if (itemCarrinho == null) return CustomResponse();

            carrinho.AtualizarUnidades(item, item.Quantidade);

            _dbContext.CarrinhoItems.Update(itemCarrinho);
            _dbContext.CarrinhoCliente.Update(carrinho);

            await PersistirDados();

            return CustomResponse();

        }

        [HttpDelete("{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await ObterCarrinhoCliente();

            var itemCarrinho = await ObterCarrinhoValidado(produtoId, carrinho);

            if (itemCarrinho == null) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _dbContext.CarrinhoItems.Remove(itemCarrinho);
            _dbContext.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        private async Task<CarrinhoCliente> ObterCarrinhoCliente()
        {
            return await _dbContext.CarrinhoCliente
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.ClienteId == _user.ObterUserId());
        }

        private async Task<CarrinhoItem> ObterCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
        {
            if (item != null && produtoId != item.ProdutoId)
            {
                AdicionarErroProcessamento("O item nao corresponde ao informado");
                return null;
            }

            if (carrinho == null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado");
                return null;
            }

            var itemCarrinho = await _dbContext.CarrinhoItems.FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

            if (itemCarrinho != null && carrinho.CarrinhoItemExistente(itemCarrinho)) return itemCarrinho;

            AdicionarErroProcessamento("O item não está no carrinho");
            return null;
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

        private async Task PersistirDados()
        {
            var result = await _dbContext.SaveChangesAsync();

            if (result <= 0) AdicionarErroProcessamento("Não foi possível persistir no banco de dados");
        }

    }
}
