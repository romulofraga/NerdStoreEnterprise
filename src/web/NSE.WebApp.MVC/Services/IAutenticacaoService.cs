using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAutenticacaoService
    {
        Task<UsuarioResponse> Login(UsuarioLogin usuarioLogin);
        Task<UsuarioResponse> Registro(UsuarioRegistro usuarioRegistro);
    }
}
