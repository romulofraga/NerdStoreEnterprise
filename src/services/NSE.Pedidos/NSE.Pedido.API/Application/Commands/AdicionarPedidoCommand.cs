using FluentValidation;
using NSE.Core.Messages;
using NSE.Pedidos.API.Application.DTO;

namespace NSE.Pedidos.API.Application.Commands;

public class AdicionarPedidoCommand : Command
{
    //Pedido
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<PedidoItemDTO> PedidoItems { get; set; }

    //Voucher
    public string VoucherCodigo { get; set; }
    public bool VoucherUtilizado { get; set; }
    public decimal Desconto { get; set; }

    //Endereco
    public EnderecoDTO Endereco { get; set; }

    //Cartao
    public string NumeroCartao { get; set; }
    public string NomeCartao { get; set; }
    public string ExpiracaoCartao { get; set; }
    public string CVVCartao { get; set; }

    public override bool IsValid()
    {
        ValidationResult = new AdicionarPedidoValidation().Validate(this);
        return ValidationResult.IsValid;
    }

    public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
    {
        public AdicionarPedidoValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("ID do cliente invalido");

            RuleFor(c => c.PedidoItems.Count())
                .GreaterThan(0)
                .WithMessage("O pedido precisa ter no minimo um item");

            RuleFor(c => c.ValorTotal)
                .GreaterThan(0)
                .WithMessage("O valor do pedido deve ser maior que zero");

            RuleFor(c => c.NumeroCartao)
                .CreditCard()
                .WithMessage("Numero de cartao invalido");

            RuleFor(c => c.NomeCartao)
                .NotNull()
                .WithMessage("Nome do portador do cartao e obrigatorio");

            RuleFor(c => c.CVVCartao.Length)
                .GreaterThan(2)
                .LessThan(5)
                .WithMessage("CVV do cartao deve ter tres ou quatro digitos");
        }
    }
}