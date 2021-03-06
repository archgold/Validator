﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Validator
{
	public partial class Validator
	{
		private static Dictionary<string, Regex> LocaleMobilePhoneRegexes = new Dictionary<string, Regex>
		{
			{ "zh-CN", new Regex(@"^(\+?0?86\-?)?1[345789][0-9]{9}$", RegexOptions.Compiled) },
			{ "en-ZA", new Regex(@"^(\+?27|0)(\d{9})$", RegexOptions.Compiled) },
			// source validator.js does not have trailing $ on this one, so i'm leaving it off too
			{ "en-AU", new Regex(@"^(\+?61|0)4(\d{8})", RegexOptions.Compiled) },
			{ "fr-FR", new Regex(@"^(\+?33|0)(6|7)\d{8}$", RegexOptions.Compiled) }
		};
		
		/// <summary>
		/// Determines whether the given phone number is a mobile phone number or not.
		/// </summary>
		/// <param name="phoneNumber">The phone number to check.</param>
		/// <param name="locale">The locale to look in.</param>
		/// <returns>True if it is a mobile phone number, false otherwise.</returns>
		/// <remarks>
		/// Relies on locales that use specific blocks of numbers for mobile phone numbers.
		/// </remarks>
		public static bool IsMobilePhone(string phoneNumber, string locale)
		{
			Regex localeRegex;
			if (LocaleMobilePhoneRegexes.TryGetValue(locale, out localeRegex))
			{
				return localeRegex.IsMatch(phoneNumber);
			}
			return false;
		}
	}
}
