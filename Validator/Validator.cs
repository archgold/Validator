﻿using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Validator
{
    public partial class Validator
    {
        public static bool IsAlpha(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z]+$");
        }

        public static bool IsLowercase(string input)
        {
            return input == input.ToLower();
        }

        public static bool IsUppercase(string input)
        {
            return input == input.ToUpper();
        }

        public static bool IsInt(string input)
        {
            return IsNumeric(input);
        }

        public static bool IsFloat(string input)
        {
            float value;
            return float.TryParse(input, out value);
        }

        public static bool IsDivisibleBy(string input, int @by)
        {
            int value;
            if (!int.TryParse(input, out value))
            {
                return false;
            }
            return value % @by == 0;
        }

        public static bool IsLength(string input, int min, int max)
        {
            if (input.Length < min)
            {
                return false;
            }

            if (input.Length > max)
            {
                return false;
            }

            return true;
        }

        public static bool IsAscii(string input)
        {
            return input.Select(c => (int)c).All(c => c <= 127);
        }

        public static bool IsMultiByte(string input)
        {
            return Regex.IsMatch(input, "[^\x00-\x7F]");
        }

        public static bool IsHalfWidth(string input)
        {
            return Regex.IsMatch(input, "[\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");
        }

        public static bool IsFullWidth(string input)
        {
            return Regex.IsMatch(input, "[^\u0020-\u007E\uFF61-\uFF9F\uFFA0-\uFFDC\uFFE8-\uFFEE0-9a-zA-Z]");
        }

        public static bool IsVariableWidth(string input)
        {
            return IsHalfWidth(input) && IsFullWidth(input);
        }

        public static bool IsSurrogatePair(string input)
        {
            return Regex.IsMatch(input, "[\uD800-\uDBFF][\uDC00-\uDFFF]");
        }

        public static bool IsIn(string input, string[] values)
        {
            return values.Any(value => value == input);
        }

        public static bool IsIp(string input, IpVersion version)
        {
	        const string ipv4MaybePattern = @"^(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)\.(\d?\d?\d)$";
			const string ipv6Pattern = @"^::|^::1|^([a-fA-F0-9]{1,4}::?){1,7}([a-fA-F0-9]{1,4})$";

	        if (version == IpVersion.Four)
	        {
				if (!Validator.Matches(input, ipv4MaybePattern))
				{
					return false;
				}

		        var parts = input.Split('.').Select(p => Convert.ToInt32(p));
		        return parts.Max() <= 255;
	        }
	        else
	        {
		        return Validator.Matches(input, ipv6Pattern);
	        }
        }

        public static bool IsEmail(string input)
        {
            try
            {
                return new MailAddress(input).Address == input;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsHexadecimal(string input)
        {
            return Regex.IsMatch(input, "^[0-9a-fA-F]+$");
        }

        public static bool IsAlphanumeric(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9]+$");
        }

        public static bool IsHexColor(string input)
        {
            return Regex.IsMatch(input, "^#?(?:[0-9a-fA-F]{3}){1,2}$");
        }

        public static bool Equals(string input, string comparison)
        {
            return input.Equals(comparison);
        }

        public static bool IsDate(string input)
        {
            DateTime date;
            return DateTime.TryParse(input, out date);
        }

        public static bool IsAfter(string input, DateTime date)
        {
            DateTime inputDate;
            if (!DateTime.TryParse(input, out inputDate))
            {
                return false;
            }
            return inputDate > date;
        }

        public static bool IsBefore(string input, DateTime date)
        {
            DateTime inputDate;
            if (!DateTime.TryParse(input, out inputDate))
            {
                return false;
            }

            return inputDate < date;
        }

        public static bool IsJson(string input)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Deserialize<dynamic>(input);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        public static bool IsNull(string input)
        {
            return input == null;
        }

        public static bool Contains(string input, string element)
        {
            return input.Contains(element);
        }

        public static bool Matches(string input, string pattern, RegexOptions options = RegexOptions.None)
        {
            return Regex.IsMatch(input, pattern, options);
        }

        public static bool IsMongoId(string input)
        {
            return input.Length == 24 && IsHexadecimal(input);
        }

        public static bool IsByteLength(string input, int min, int max = int.MaxValue)
        {
            return input.Length >= min && input.Length <= max;
        }
    }
}