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
        public const string DeviceIdRequired = "Требуется идентификатор устройства";
        public const string DeviceNameRequired = "Требуется название устройства";
        public const string DeviceInvalidPlatformType = "Недопустимый тип платформы устройства";
        public const string DeviceOwnerNotFound = "Владелец устройства не найден";
        public const string DeviceOwnerMismatch = "Владелец устройства не совпадает с авторизованным пользователем";
        public const string DeviceNotOwnedByUser = "Устройство не принадлежит пользователю";
        public const string UserIdRequired = "Требуется идентификатор пользователя";
        public const string UserIdNotNull = "Идентификатор пользователя не может быть пустым";
        /// <summary>
        /// Превышена максимальная длина
        /// </summary>
        public const string ExceededMaxLength = "Максимальная длина не должна превышать ";
        public const string DeviceNameMaxLength = ExceededMaxLength + "64 для названия устройства";
        public const string LoginMaxLength = ExceededMaxLength + "64 для логина";
        public const string PasswordMaxLength = ExceededMaxLength + "64 для пароля";

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
