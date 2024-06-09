namespace io.radston12.fakerank.Helpers
{

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class StringSanitze
    {

        public static readonly string InvalidCharacters = "[]?&^$@,:;<>’\'-_*\"µ´`\\";
        public static readonly string InvalidASCCIRegex = @"[^\u0000-\u007F]+";

        public static string strapoutInvalidCharaters(string original, int maxLength)
        {
            string sanitizedString = Regex.Replace(original, InvalidASCCIRegex, string.Empty);
            
            foreach(char invalidChar in InvalidCharacters) 
                sanitizedString = sanitizedString.Replace(invalidChar.ToString(), string.Empty);
            

            if (sanitizedString.Length > maxLength)
                sanitizedString = sanitizedString.Substring(0, maxLength);

            return sanitizedString;
        }
    }
}