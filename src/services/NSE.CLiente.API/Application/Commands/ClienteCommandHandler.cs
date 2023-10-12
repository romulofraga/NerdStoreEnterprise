using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Application.Events;
using NSE.Clientes.API.Models;
using NSE.Core.Messages;

namespace NSE.Clientes.API.Application.Commands;

public class ClienteCommandHandler : CommandHandler,
    IRequestHandler<RegistrarClienteCommand, ValidationResult>,
    IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
{
    private readonly IClienteRepository _clienteRepository;

    public ClienteCommandHandler(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var endereco = new Endereco(message.Logradouro, message.Numero, message.Complemento, message.Bairro,
            message.Cep, message.Cidade, message.Estado, message.ClientId);

        _clienteRepository.AdicionarEndereco(endereco);

        return await PersistirDados(_clienteRepository.UnityOfWork);
    }

    public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
    {
        if (!message.IsValid()) return message.ValidationResult;

        var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

        var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);
        if (clienteExistente != null)
        {
            AdicionarErro("Esse CPF Ja está em uso.");
            return ValidationResult;
        }

        _clienteRepository.AdicionarCliente(cliente);

        var clienteRegistrado = new ClienteRegistradoEvent(message.Id, message.Nome, message.Email, message.Cpf);
        cliente.AdicionaEvento(clienteRegistrado);

        return await PersistirDados(_clienteRepository.UnityOfWork);
    }
}