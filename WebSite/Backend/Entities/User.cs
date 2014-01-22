using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RetroShark.Application.Backend.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
    }
}