﻿using FluentValidation.Results;
using NSE.Clientes.API.Facade;
using NSE.Core.Messages.Integration;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoFacade _pagamentoFacade;
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IPagamentoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository)
        {
            _pagamentoFacade = pagamentoFacade;
            _pagamentoRepository = pagamentoRepository;
        }

        public async Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento)
        {
            var transacao = await _pagamentoFacade.AutorizarPagamento(pagamento);

            var validationResult = new ValidationResult();

            if (transacao.Status != StatusTransacao.Autorizado)
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Pagamento recusado, entre em contato com sua operadora de cartão"));

                return new ResponseMessage(validationResult);
            }

            pagamento.AdicionarTransacao(transacao);

            _pagamentoRepository.AdicionarPagamento(pagamento);

            if (!await _pagamentoRepository.UnityOfWork.Commit())
            {
                validationResult.Errors.Add(new ValidationFailure("Pagamento", "Houve um erro ao realizar o pagamento"));

                //TODO: Comunicar com o gateway para realizar o estorno.

                return new ResponseMessage(validationResult);
            }

            return new ResponseMessage(validationResult);
        }
    }
}