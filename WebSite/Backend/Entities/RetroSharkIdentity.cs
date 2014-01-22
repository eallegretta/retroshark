using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace RetroShark.Application.Backend.Entities
{
    public class RetroSharkIdentity: GenericIdentity
    {
        public User User { get; private set; }

        public RetroSharkIdentity(string username, User user): base(username)
        {
            User = user;
        }
    }
}