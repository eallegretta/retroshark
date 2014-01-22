using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;
using RetroShark.Application.Backend.Entities;

namespace RetroShark.Application.Backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpContextBase _httpContext;

        public AuthenticationService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public void Authenticate(string username, bool isAdmin)
        {
            var ticket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout),
                false,
                JsonConvert.SerializeObject(new User { Id = Guid.NewGuid(), Username = username, IsAdmin = isAdmin }));

            string encTicket = FormsAuthentication.Encrypt(ticket);

            if (_httpContext.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                _httpContext.Request.Cookies.Remove(FormsAuthentication.FormsCookieName);
            }

            _httpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public void SetPrincipal()
        {
            var cookie = _httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null)
            {
                _httpContext.User = new GenericPrincipal(new GenericIdentity(_httpContext.User.Identity.Name), null);
            }
            else
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);

                var user = JsonConvert.DeserializeObject<User>(ticket.UserData);

                _httpContext.User = new GenericPrincipal(new RetroSharkIdentity(_httpContext.User.Identity.Name, user), user.IsAdmin ? new[] { "Admin" } : null);
            }
        }

        public User GetLoggedUser()
        {
            var cookie = _httpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null)
            {
                return null;
            }

            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            return JsonConvert.DeserializeObject<User>(ticket.UserData);
        }

        public bool IsLoggedUserAdmin()
        {
            return GetLoggedUser().IsAdmin;
        }

        public void SetAdminRole(string username)
        {
            Authenticate(username, true);
        }
    }
}