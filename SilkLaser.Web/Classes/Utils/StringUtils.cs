using System;
using System.Linq;
using System.Net;
using System.Text;

namespace SilkLaser.Web.Classes.Utils
{
    /// <summary>
    /// Статический класс содержащий утилитарные методы для работ со строками
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Генерирует строку, состояющую из указанного символа в указанном количестве
        /// </summary>
        /// <param name="character">Символ</param>
        /// <param name="count">Количество</param>
        /// <returns>Цельная строка</returns>
        public static string GenerateString(char character, int count)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < count; i++)
            {
                sb.Append(character);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Нормализует строку, содержащую номер телефона
        /// </summary>
        /// <param name="phone">Номер телефона</param>
        /// <returns>Нормализованный номер телефона</returns>
        public static string NormalizePhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
            {
                return "";
            }
            var str = new StringBuilder(phone);
            str.Replace("-", string.Empty).Replace(")", string.Empty).Replace("(", string.Empty).Replace("+7", "7").Replace(" ","");
            if (phone.StartsWith("8"))
            {
                str[0] = '7';
            }
            return str.ToString();
        }

        /// <summary>
        /// Форматирует номер телефона для удобного отображения на сайте
        /// </summary>
        /// <param name="phone">Номер телефона</param>
        /// <returns></returns>
        public static string FormatPhoneNumber(this string phone)
        {
            var ph = NormalizePhoneNumber(phone);
            var sb = new StringBuilder(ph);
            if (sb.Length == 11)
            {
                return sb.Insert(0, '+').Insert(2, '-').Insert(6, '-').Insert(10, '-').Insert(13, '-').ToString();
            }
            else if (sb.Length == 6)
            {
                return sb.Insert(2, "-").Insert(5, "-").ToString();
            }
            else return ph;
        }

        /// <summary>
        /// Форматирует число как цену, разбивая его на разряды
        /// </summary>
        /// <param name="price">Цена</param>
        /// <returns></returns>
        public static string FormatPrice(this double? price)
        {
            if (price == null)
            {
                return string.Empty;
            }
            return string.Format("{0:N}", (long)price.Value).Replace(",00","");
        }

        /// <summary>
        /// Возвращает отформатированную строку с размером файла с мегабайтами
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string FormatFileSize(this long size)
        {
            if (size > (1024 * 1024 * 1024))
            {
                double s = size, d = 1073741824.0;
                return string.Format("{0:0.0} Гб", s / d);
            }
            if (size > (1024*1024))
            {
                double s = size, d = 1048576.0;
                return string.Format("{0:0.0} Мб", s / d);
            }
            if (size > 1024)
            {
                double s = size, d = 1024;
                return string.Format("{0:0.0} Кб", s / d);
            }
            return string.Format("{0:0} байт", size);
        }

        /// <summary>
        /// Выполняет транслитерацию текста
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Transliterate(this string str)
        {
            return Transliteration.Front(str, TransliterationType.ISO);
        }

        /// <summary>
        /// Выдирает доменную зону из имени домена
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ExtractZone(this string str)
        {
            var parts = str.Split('.').ToList();
            parts.RemoveAt(0);
            return string.Join(".", parts);
        }

		/// <summary>
		/// Выполняет преобразование раскладки клавиатуры
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
	    public static string BadKeyboard(this string str)
		{
			var eng = "QWERTYUIOP{}ASDFGHJKL:\"|ZXCVBNM<>?qwertyuiop[]asdfghjkl;'\\zxcvbnm,./";
			var rus = "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭ/ЯЧСМИТЬБЮ,йцукенгшщзхъфывапролджэ\\ячсмитьбю.";
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			var newStr = new StringBuilder(str);
			for (var i = 0; i < str.Length; i++)
			{
				var ch = newStr[i];
				if (eng.Contains(ch))
				{
					var idx = eng.IndexOf(ch);
					var rch = rus[idx];
					newStr[i] = rch;
				}
				else if (rus.Contains(ch))
				{
					var idx = rus.IndexOf(ch);
					var ech = eng[idx];
					newStr[i] = ech;
				}
			}
			return newStr.ToString();
		}

		/// <summary>
		/// Конвертирует строку IP адрес в формат числа
		/// </summary>
		/// <param name="ipString">IP адрес как строка</param>
		/// <returns></returns>
		public static long GetIpAsUInt32(string ipString)
		{
			if (ipString == "::1")
			{
				return 0;
			}
			return (long)(uint)IPAddress.NetworkToHostOrder(
			 (int)IPAddress.Parse(ipString).Address);
		}

		/// <summary>
		/// Возвращает возраст со строкой
		/// </summary>
		/// <param name="_birthDate"></param>
		/// <returns></returns>
		public static string GetAgeString(DateTime? _birthDate)
		{
			if (!_birthDate.HasValue)
			{
				return null;
			}
			int _userAge = (int)Math.Floor((DateTime.Now - _birthDate.Value).TotalDays/365);
			var t1 = _userAge % 10;
			var t2 = _userAge % 100;
			if (t1 == 1 && t2 != 11)
			{
				return _userAge.ToString() + " год";
			}
			if (t1 >= 2 && t1 <= 4 && (t2 < 10 || t2 >= 20))
			{
				return _userAge.ToString() + " года";
			}
			else
			{
				return _userAge.ToString() + " лет";
			}

		}

		/// <summary>
		/// Ремапит некоторые интернационыльные символы ещ ASCII
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public static string RemapInternationalCharToAscii(char c)
		{
			string s = c.ToString().ToLowerInvariant();
			if ("àåáâäãåą".Contains(s))
			{
				return "a";
			}
			else if ("èéêëę".Contains(s))
			{
				return "e";
			}
			else if ("ìíîïı".Contains(s))
			{
				return "i";
			}
			else if ("òóôõöøőð".Contains(s))
			{
				return "o";
			}
			else if ("ùúûüŭů".Contains(s))
			{
				return "u";
			}
			else if ("çćčĉ".Contains(s))
			{
				return "c";
			}
			else if ("żźž".Contains(s))
			{
				return "z";
			}
			else if ("śşšŝ".Contains(s))
			{
				return "s";
			}
			else if ("ñń".Contains(s))
			{
				return "n";
			}
			else if ("ýÿ".Contains(s))
			{
				return "y";
			}
			else if ("ğĝ".Contains(s))
			{
				return "g";
			}
			else if (c == 'ř')
			{
				return "r";
			}
			else if (c == 'ł')
			{
				return "l";
			}
			else if (c == 'đ')
			{
				return "d";
			}
			else if (c == 'ß')
			{
				return "ss";
			}
			else if (c == 'Þ')
			{
				return "th";
			}
			else if (c == 'ĥ')
			{
				return "h";
			}
			else if (c == 'ĵ')
			{
				return "j";
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Produces optional, URL-friendly version of a title, "like-this-one". 
		/// hand-tuned for speed, reflects performance refactoring contributed
		/// by John Gietzen (user otac0n) 
		/// </summary>
		public static string URLFriendly(string title)
		{
			if (title == null) return "";

			const int maxlen = 80;
			int len = title.Length;
			bool prevdash = false;
			var sb = new StringBuilder(len);
			char c;

			for (int i = 0; i < len; i++)
			{
				c = title[i];
				if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
				{
					sb.Append(c);
					prevdash = false;
				}
				else if (c >= 'A' && c <= 'Z')
				{
					// tricky way to convert to lowercase
					sb.Append((char)(c | 32));
					prevdash = false;
				}
				else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
					c == '\\' || c == '-' || c == '_' || c == '=')
				{
					if (!prevdash && sb.Length > 0)
					{
						sb.Append('-');
						prevdash = true;
					}
				}
				else if ((int)c >= 128)
				{
					int prevlen = sb.Length;
					sb.Append(RemapInternationalCharToAscii(c));
					if (prevlen != sb.Length) prevdash = false;
				}
				if (i == maxlen) break;
			}

			if (prevdash)
				return sb.ToString().Substring(0, sb.Length - 1);
			else
				return sb.ToString();
		}

		/// <summary>
		/// Преобразует заголовок в заголовок пригодный для Seo
		/// </summary>
		/// <param name="title">Заголовок</param>
		/// <returns></returns>
		public static string TitleToSeoRoute(string title)
		{
			var processed = Transliterate(title);
			return URLFriendly(processed);
		}
    }


}