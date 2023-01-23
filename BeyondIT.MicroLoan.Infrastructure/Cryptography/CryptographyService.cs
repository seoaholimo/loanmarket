﻿using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace BeyondIT.MicroLoan.Infrastructure.Cryptography
{
    public class CryptographyService : ICryptographyService
    {
        public string CreateSalt()
        {
            return CreateRandomString(24);
        }

        public string CreatePasswordHash(string password, string salt)
        {
            var passwordAndSalt = string.Concat(password, salt);

            HashAlgorithm hashAlg = new SHA256CryptoServiceProvider();

            byte[] bytValue = System.Text.Encoding.UTF8.GetBytes(passwordAndSalt);

            byte[] bytHash = hashAlg.ComputeHash(bytValue);

            return Convert.ToBase64String(bytHash);
        }

        public string GetRandomPassword()
        {
            return CreateRandomString(8);
        }

        private static string CreateRandomString(int length)
        {
            var rng = new RNGCryptoServiceProvider();

            var buff = new byte[length * 3];

            rng.GetBytes(buff);

            var regex = new Regex("[^a-zA-Z0-9]");

            var str = Convert.ToBase64String(buff);

            str = regex.Replace(str, "");

            return str.Substring(0, length);
        }
    }
}
