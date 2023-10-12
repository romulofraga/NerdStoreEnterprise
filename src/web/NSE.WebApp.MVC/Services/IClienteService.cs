using NSE.Core.Comunication;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services;

public interface IClienteService
{
    Task<EnderecoViewModel> ObterEndereco();
    Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
}