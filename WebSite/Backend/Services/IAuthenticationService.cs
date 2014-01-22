using RetroShark.Application.Backend.Entities;
namespace RetroShark.Application.Backend.Services
{
    public interface IAuthenticationService
    {
        void Authenticate(string username, bool isAdmin);
        bool IsLoggedUserAdmin();
        void SetAdminRole(string username);
        User GetLoggedUser();
        void SetPrincipal();
    }
}