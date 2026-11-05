using System;
using System.Collections.Generic;
using System.Text;

namespace RemoteAccess.Domain.Models.Ecxeptions
{
    public static class ValidationErrorMessages
    {
        /// <summary>
        /// Требуется название продукта
        /// </summary>
        public const string DeviceOwnerIdRequired = "Требуется идентификатор пользователя устройства";
        public const string DeviceNameRequired = "Требуется название устройства";
        public const string DeviceInvalidPlatformType = "Недопустимый тип платформы устройства";
        public const string UserIdRequired = "Требуется идентификатор пользователя";
        public const string UserIdNotNull = "Идентификатор пользователя не может быть пустым";
        /// <summary>
        /// Превышена максимальная длина
        /// </summary>
        public const string ExceededMaxLength = "Максимальная длина не должна превышать ";

        /// <summary>
        /// Требуется пароль
        /// </summary>
        public const string PasswordRequired = "Требуется пароль";

        /// <summary>
        /// Требуется логин
        /// </summary>
        public const string LoginRequired = "Требуется логин";


    }
}
