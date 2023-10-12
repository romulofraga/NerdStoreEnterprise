﻿namespace NSE.Pagamentos.API.Models;

public class CartaoCredito
{
    protected CartaoCredito()
    {
    }

    public CartaoCredito(string nomeCartao, string numeroCartao, string mesAnoVencimento, string cvv)
    {
        NomeCartao = nomeCartao;
        NumeroCartao = numeroCartao;
        MesAnoVencimento = mesAnoVencimento;
        CVV = cvv;
    }

    public string NomeCartao { get; set; }
    public string NumeroCartao { get; set; }
    public string MesAnoVencimento { get; set; }
    public string CVV { get; set; }
}