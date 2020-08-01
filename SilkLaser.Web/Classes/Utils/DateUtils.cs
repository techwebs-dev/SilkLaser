using System;
using System.Globalization;
using System.Linq;

namespace SilkLaser.Web.Classes.Utils
{
    /// <summary>
    /// Статический класс по работе с датами
    /// </summary>
    public static class DateUtils
    {
        public static string[] Names = new[]
                            {
                                "январь", "февраль", "март", "апрель", "май", "июнь", "июль", "август", "сентябрь",
                                "октябрь", "ноябрь", "декабрь"
                            };
        
        /// <summary>
        /// Возвращает название месяца по его номера
        /// </summary>
        /// <param name="number">Номер месяца</param>
        /// <returns></returns>
        public static string GetMonthName(int number)
        {
            return Names[number - 1];
        }

        /// <summary>
        /// Возвращает номер последнего дня в году
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns></returns>
        public static int LastDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).Day;
        }

        /// <summary>
        /// Возвращает первый день в году
        /// </summary>
        /// <param name="date">Дата</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static int GetDayNumberOnAWeek(DayOfWeek dayOfWeek)
        {
            switch(dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return 7;
                    break;
                case DayOfWeek.Monday:
                    return 1;
                    break;
                case DayOfWeek.Tuesday:
                    return 2;
                    break;
                case DayOfWeek.Wednesday:
                    return 3;
                    break;
                case DayOfWeek.Thursday:
                    return 4;
                    break;
                case DayOfWeek.Friday:
                    return 5;
                    break;
                case DayOfWeek.Saturday:
                    return 6;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("dayOfWeek");
            }
        }

        /// <summary>
        /// Возвращает дату начала недели для указанной даты
        /// </summary>
        /// <param name="dt">Дата и время</param>
        /// <returns></returns>
        public static DateTime WeekStart(this DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday)
            {
                return dt.Date;
            }
            else
            {
                return dt.AddDays(-1*(DateUtils.GetDayNumberOnAWeek(dt.DayOfWeek)-1));
            }
        }

		/// <summary>
		/// Преобзовывает количество миллисекунд от юникс время в дату и время
		/// </summary>
		/// <param name="unixTimeStamp"></param>
		/// <returns></returns>
		public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
		{
			// Unix timestamp is seconds past epoch
			System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		/// <summary>
		/// Определяет нахождение указанной даты и времени в определенном временном интервале
		/// </summary>
		/// <param name="dateTime">Дата и время</param>
		/// <param name="timeinterval">Временной интервал</param>
		/// <returns></returns>
	    public static bool IsDateTimeInStringTimeInterval(DateTime dateTime, string timeinterval)
		{
			// Парсим строку
			var parts = timeinterval.Split('-').Select(p => p.Trim()).ToList();
			if (parts.Count != 2)
			{
				return false;
			}

			// Парсим компоненты
			var startTimeStr = String.Format("{0} {1}",dateTime.FormatDate(),parts[0]);
			var endTimeStr = String.Format("{0} {1}", dateTime.FormatDate(), parts[1]);
			DateTime start, end;
			if (!DateTime.TryParseExact(startTimeStr, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out start))
			{
				return false;
			}
			if (!DateTime.TryParseExact(endTimeStr, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out end))
			{
				return false;
			}
			if (dateTime.Kind == DateTimeKind.Utc)
			{
				start = start.ToUniversalTime();
				end = end.ToUniversalTime();
			}
			return dateTime >= start && dateTime <= end;
		}
    }
}