﻿namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        public CarrinhoCliente() { }

        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Items { get; set; } = new List<CarrinhoItem>();

        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }
    }
}
