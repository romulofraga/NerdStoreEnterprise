﻿using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using NSE.WebApi.Core.Controllers;
using NSE.WebApi.Core.Usuario;

namespace NSE.Clientes.API.Controllers
{
    public class ClientesController : MainController
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IAspnetUser _user;

        public ClientesController(IMediatorHandler mediatorHandler, IClienteRepository clienteRepository, IAspnetUser user)
        {
            _mediatorHandler = mediatorHandler;
            _clienteRepository = clienteRepository;
            _user = user;
        }

        [HttpGet("cliente/endereco")]
        public async Task<IActionResult> ObterEndereco()
        {
            var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());

            return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClientId = _user.ObterUserId();
            return CustomResponse(await _mediatorHandler.EnviarComando(endereco));
        }
    }
}
