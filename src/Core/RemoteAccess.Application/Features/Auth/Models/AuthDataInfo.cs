using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess.Application.Features.Users.Models
{
    public class AuthDataInfo
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; } = string.Empty;

        public AuthDataInfo()
        {
        }

        public AuthDataInfo(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
