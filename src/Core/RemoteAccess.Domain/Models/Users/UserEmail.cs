using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess.Domain.Models.Users
{
    public class UserEmail
    {
        public string Email { get; }
        public bool Confirmed { get; }
        public UserEmail(string email, bool confirmed = false)
        {
            Email = email.ToLowerInvariant();
            Confirmed = confirmed;
        }
    }
}
