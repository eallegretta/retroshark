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
        private HttpContextBase HttpContext { get { return new HttpContextWrapper(System.Web.HttpContext.Current); } }

        public void Authenticate(string username, bool isAdmin)
        {
            SetAuthenticationTicket(username, isAdmin);
        }

        public void SetPrincipal()
        {
            var cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null)
            {
                HttpContext.User = new GenericPrincipal(new GenericIdentity(HttpContext.User.Identity.Name), null);
            }
            else
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);

                var user = JsonConvert.DeserializeObject<User>(ticket.UserData);

                HttpContext.User = new GenericPrincipal(new RetroSharkIdentity(HttpContext.User.Identity.Name, user), user.IsAdmin ? new[] { "Admin" } : null);
            }
        }

        public User GetLoggedUser()
        {
            var ticket = GetDecryptedTicket();

            if (ticket == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<User>(ticket.UserData);
        }

        public bool IsLoggedUserAdmin()
        {
            return GetLoggedUser().IsAdmin;
        }

        public void SetAdminRole(string username)
        {
            SetAuthenticationTicket(username, true);
        }

        private void SetAuthenticationTicket(string username, bool isAdmin)
        {
            var ticket = GetDecryptedTicket();

            if (ticket == null)
            {
                ticket = new FormsAuthenticationTicket(
                        1,
                        username,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(System.Web.HttpContext.Current.Session.Timeout),
                        false,
                        JsonConvert.SerializeObject(new User { Id = Guid.NewGuid(), Username = username, IsAdmin = isAdmin }));
            }
            else
            {
                var currentUser = GetLoggedUser();
                currentUser.Username = username;
                currentUser.IsAdmin = isAdmin;
                ticket = new FormsAuthenticationTicket(
                    1,
                    username,
                    ticket.IssueDate,
                    ticket.Expiration,
                    ticket.IsPersistent,
                    JsonConvert.SerializeObject(currentUser));
            }

            string encTicket = FormsAuthentication.Encrypt(ticket);

            HttpContext.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        private FormsAuthenticationTicket GetDecryptedTicket()
        {
            var cookie = HttpContext.Request.Cookies.Get(FormsAuthentication.FormsCookieName);

            if (cookie == null)
            {
                return null;
            }

            return FormsAuthentication.Decrypt(cookie.Value);
        }
    }
}