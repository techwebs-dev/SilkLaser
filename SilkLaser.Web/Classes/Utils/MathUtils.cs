// ============================================================
// 
// 	Asgard
// 	Asgard.Web.Public 
// 	MathUtils.cs
// 
// 	Created by: ykorshev 
// 	 at 03.08.2013 11:08
// 
// ============================================================

using System;

namespace SilkLaser.Web.Classes.Utils
{
    /// <summary>
    /// Математические утилиты
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Возвращает процентную строку
        /// </summary>
        /// <param name="value">Малое</param>
        /// <param name="total">Большее</param>
        /// <returns></returns>
        public static string GetPercentage(int value, int total)
        {
            double v = value, t = total;
            var res = v/t*100;
            return String.Format("{0:0}%", res);
        }

        /// <summary>
        /// Возвращает процентную строку
        /// </summary>
        /// <param name="value">Малое</param>
        /// <param name="total">Большее</param>
        /// <returns></returns>
        public static string GetPercentage(long value, long total)
        {
            double v = value, t = total;
            var res = v / t * 100;
            return String.Format("{0:0}%", res);
        }

        /// <summary>
        /// Определяет количество страниц получающееся при разбиении набора по странично
        /// </summary>
        /// <param name="count">количество</param>
        /// <param name="perPage">на странице</param>
        /// <returns></returns>
        public static int PagesCount(int count, int perPage)
        {
            if (count % perPage != 0)
            {
                return (int)Math.Floor((decimal)(count / perPage)) + 1;
            }
            else
            {
                return count / perPage;
            }
        }

		/// <summary>
		/// Преобразует число в относительное в формате 0..1
		/// </summary>
		/// <param name="inputValue">Входное число</param>
		/// <param name="totalValue">Максимальное число относительно которого необходимо провести трансформацию</param>
		/// <returns></returns>
	    public static double ConvertTo01Dimen(double inputValue, double totalValue)
		{
			return inputValue/totalValue;
		}

		/// <summary>
		/// Конвертирует число из относительного в формате 0..1 в абсолютное
		/// </summary>
		/// <param name="inputValue">Входящее число</param>
		/// <param name="totalValue">Общее число</param>
		/// <returns></returns>
	    public static double ConvertFrom01Dimen(double inputValue, double totalValue)
		{
			return inputValue*totalValue;
		}

	    public const double accuracy = 0.0000001;

	    /// <summary>
	    /// Проверяет равенство чисел с плавающей точкой до заданной точности.
	    /// </summary>
	    /// <param name="a">Первое число.</param>
	    /// <param name="b">Второе число.</param>
	    /// <param name="eps">Точность.</param>
	    /// <returns>Возвращает true, если числа равны. Иначе false.</returns>
	    public static bool Equals(double a, double b, double eps = accuracy)
	    {
		    return Math.Abs(a - b) > eps ? false : true;
	    }

	    /// <summary>
	    /// Проверяет, что первое число меньше второго с заданной точностью.
	    /// </summary>
	    /// <param name="a">Первое число.</param>
	    /// <param name="b">Второе число.</param>
	    /// <param name="eps">Точность.</param>
	    /// <returns>Возвращает true, если первое число меньше. Иначе false.</returns>
	    public static bool Less(double a, double b, double eps = accuracy)
	    {
		    return (a - b) < 0 && Math.Abs(a - b) > eps ? true : false;
	    }

	    /// <summary>
	    /// Проверяет, что первое число больше второго с заданной точностью.
	    /// </summary>
	    /// <param name="a">Первое число.</param>
	    /// <param name="b">Второе число.</param>
	    /// <param name="eps">Точность.</param>
	    /// <returns>Возвращает true, если первое число больше. Иначе false.</returns>
	    public static bool Great(double a, double b, double eps = accuracy)
	    {
		    return (a - b) > 0 && Math.Abs(a - b) > eps ? true : false;
	    }
    }
}