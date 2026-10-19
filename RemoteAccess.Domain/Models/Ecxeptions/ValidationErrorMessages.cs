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
        public const string ProductNameRequired = "Требуется название продукта";

        /// <summary>
        /// Требуется идентификатор клиента
        /// </summary>
        public const string ClientIdRequired = "Требуется идентификатор клиента";

        /// <summary>
        /// Требуется название api-ресурса
        /// </summary>
        public const string ApiResourceNameRequired = "Требуется название api-ресурса";

        /// <summary>
        /// Некорректный идентификатор клиента
        /// </summary>
        public const string ClientIdIncorrect = "Некорректный идентификатор клиента";

        /// <summary>
        /// Некорректный  секрет клиента
        /// </summary>
        public const string ClientSecretIncorrect = "Некорректный секрет клиента";

        /// <summary>
        /// Некорректное название api-ресурса
        /// </summary>
        public const string ApiResourceNameIncorrect = "Некорректное название api-ресурса";

        /// <summary>
        /// Требуется ссылка для перехода на продукт
        /// </summary>
        public const string ProductUrlRequired = "Требуется ссылка для перехода на продукт";

        /// <summary>
        /// Требуется краткое описание продукта
        /// </summary>
        public const string ShortDescriptionRequired = "Требуется краткое описание продукта";

        /// <summary>
        /// Требуется цвет подложки продукта
        /// </summary>
        public const string BackgroundHexColorRequired = "Требуется цвет подложки продукта";

        /// <summary>
        /// Некорректный цвет подложки
        /// </summary>
        public const string BackgroundHexColorIncorrect = "Некорректный цвет подложки";

        /// <summary>
        /// Одна из ссылок не является адресом
        /// </summary>
        public const string UrlIncorrect = "Одна из ссылок не является адресом";

        /// <summary>
        /// Необходима минимальная длина
        /// </summary>
        public const string RequiredMinLength = "Минимальная длина должна быть ";

        /// <summary>
        /// Превышена максимальная длина
        /// </summary>
        public const string ExceededMaxLength = "Максимальная длина не должна превышать ";

        /// <summary>
        /// Требуется название области действий
        /// </summary>
        public const string ScopeNameRequired = "Требуется название области действий";

        /// <summary>
        /// Требуется указать активен ли клиент
        /// </summary>
        public const string EnabledRequired = "Требуется указать активен ли клиент";

        /// <summary>
        /// Требуются области действий
        /// </summary>
        public const string ScopesRequired = "Требуются области действий";

        /// <summary>
        /// Требуется идентификатор продукта
        /// </summary>
        public const string ProductIdRequired = "Требуется идентификатор продукта";

        /// <summary>
        /// Требуется тип взаимодействия
        /// </summary>
        public const string AllowedGrantTypeRequired = "Требуется тип взаимодействия";

        /// <summary>
        /// Требуется пароль
        /// </summary>
        public const string PasswordRequired = "Требуется пароль";

        /// <summary>
        /// Требуется логин
        /// </summary>
        public const string LoginRequired = "Требуется логин";

        /// <summary>
        /// Требуется токен
        /// </summary>
        public const string TokenRequired = "Требуется токен";

        /// <summary>
        /// Превышена максимальная длина
        /// </summary>
        public const string InsufficientMinLength = "Минимальная длина не должна быть меньше ";
        /// <summary>
        /// Требуется название рекламы
        /// </summary>
        public const string AdvertisingNameRequired = "Требуется название рекламы";

        /// <summary>
        /// Требуется ссылка на описание продукта
        /// </summary>
        public const string DescriptionLinkRequired = "Требуется ссылка на описание продукта";

        /// <summary>
        /// Требуется приоритет рекламы
        /// </summary>
        public const string AdvertisingPriorityRequired = "Требуется приоритет рекламы";

        /// <summary>
        /// Требуется идентификатор рекламы
        /// </summary>
        public const string AdvertisingIdRequired = "Требуется идентификатор рекламы";

        /// <summary>
        /// Требуются идентификаторы продуктов
        /// </summary>
        public const string ProductIdentifiersRequired = "Требуются идентификаторы продуктов";

        /// <summary>
        /// Ссылка не является адресом описания продукта
        /// </summary>
        public const string DescriptionLinkIncorrect = "Некорректная ссылка для описания продукта";

        /// <summary>
        /// Некорректное значение приоритета рекламы
        /// </summary>
        public const string AdvertisingPriorityIncorrect = "Некорректное значение приоритета рекламы";

        /// <summary>
        /// Требуется идентификатор изображения
        /// </summary>
        public const string ImageIdRequired = "Требуется идентификатор изображения";

        /// <summary>
        /// Требуется минимальное разрешение изображения
        /// </summary>
        public const string MinResolutionRequired = "Требуется минимальное разрешение изображения";

        /// <summary>
        /// Требуется реклама для списка
        /// </summary>
        public const string AdvertisingRequired = "Требуется реклама для списка";

        /// <summary>
        /// Некорректная электронная почта
        /// </summary>
        public const string EmailIncorrect = "Некорректная электронная почта";

        /// <summary>
        /// Некорректный телефон
        /// </summary>
        public const string PhoneIncorrect = "Некорректный телефон";

        /// <summary>
        /// Требуется электронная почта
        /// </summary>
        public const string EmailRequired = "Требуется электронная почта";

        /// <summary>
        /// Требуется телефон
        /// </summary>
        public const string PhoneRequired = "Требуется телефон";

        /// <summary>
        /// Требуется название разрешения продукта
        /// </summary>
        public const string ProductPermissionNameRequired = "Требуется название разрешения продукта";

        /// <summary>
        /// Требуется код экосистемы
        /// </summary>
        public const string TenantIdRequired = "Требуется код экосистемы";

        /// <summary>
        /// Требуется наименование экосистемы
        /// </summary>
        public const string TenantNameRequired = "Требуется наименование экосистемы";

        /// <summary>
        /// Требуется код файла
        /// </summary>
        public const string FileIdRequired = "Требуется код файла";

        /// <summary>
        /// Требуется код группы продуктов
        /// </summary>
        public const string ProductGroupIdRequired = "Требуется код группы продуктов";

        /// <summary>
        /// Требуется наименование группы продуктов
        /// </summary>
        public const string ProductGroupNameRequired = "Требуется наименование группы продуктов";
    }
}
