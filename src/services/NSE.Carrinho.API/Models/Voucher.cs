namespace NSE.Carrinho.API.Models
{
    public enum TipoDescontoVoucher
    {
        Porcentagem,
        Valor
    }

    public class Voucher
    {
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Codigo { get; set; }
        public TipoDescontoVoucher TipoDesconto { get; set; }
    }
}
