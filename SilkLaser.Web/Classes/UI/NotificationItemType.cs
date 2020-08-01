using SilkLaser.Web.Classes.Enums;

namespace SilkLaser.Web.Classes.UI
{
    /// <summary>
    /// Тип элемента нотификации
    /// </summary>
    public enum NotificationItemType
    {
        /// <summary>
        /// Сообщение об успехе
        /// </summary>
        [EnumDescription("success-notification")]
        Success,

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [EnumDescription("error-notification")]
        Error,

        /// <summary>
        /// предупреждение
        /// </summary>
        [EnumDescription("warning-notification")]
        Warning
    }
}