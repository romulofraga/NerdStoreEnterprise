#nullable disable
using FluentValidation.Results;
using NSE.Carrinho.API.Models.Validations;

namespace NSE.Carrinho.API.Models
{
    public class CarrinhoCliente
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;

        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }

        public ValidationResult ValidationResult { get; set; }

        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }

        public Voucher Voucher { get; set; }

        // EF Relation
        public List<CarrinhoItem> Itens { get; set; }

        public CarrinhoCliente(Guid clienteId) : this()
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        // EF Constructor
        public CarrinhoCliente()
        {
            Itens = new List<CarrinhoItem>();
        }

        public void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorTotal();
        }

        internal void CalcularValorTotalDesconto()
        {
            if (!VoucherUtilizado) return;

            decimal desconto = 0;

            var valor = ValorTotal;

            if (Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if (Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;

                    valor -= desconto;
                }
            }
            else
            {
                if (Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;

                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }

        internal void CalcularValorTotal()
        {
            ValorTotal = Itens.Sum(ci => ci.CalcularValorTotal());
            CalcularValorTotalDesconto();
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(ci => ci.ProdutoId == item.ProdutoId);
        }

        internal CarrinhoItem ObterPorProdutoId(Guid produtoId)
        {
            return Itens.FirstOrDefault(ci => ci.ProdutoId == produtoId);
        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            if (CarrinhoItemExistente(item))
            {
                var itemExistente = ObterPorProdutoId(item.ProdutoId);
                itemExistente!.AdicionarUnidades(item.Quantidade);

                item = itemExistente;
                Itens.Remove(itemExistente);
            }

            Itens.Add(item);
            CalcularValorTotal();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterPorProdutoId(item.ProdutoId);

            Itens.Remove(itemExistente!);
            Itens.Add(item);

            CalcularValorTotal();
        }

        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidades(unidades);
            AtualizarItem(item);
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            var itemExistente = ObterPorProdutoId(item.ProdutoId);
            Itens.Remove(itemExistente!);

            CalcularValorTotal();
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(i => new ItemCarrinhoValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);

            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }
    }
}
