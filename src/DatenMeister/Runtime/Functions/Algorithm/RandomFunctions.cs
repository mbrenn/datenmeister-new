using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace DatenMeister.Runtime.Functions.Algorithm
{
    /// <summary>
    /// Stores all functions that are connected with random functions
    /// </summary>
    public class RandomFunctions
    {
        public static string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";

            return GetRandomString(length, alphanumericCharacters);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/38632735/rngcryptoserviceprovider-in-net-core
        /// </summary>
        /// <param name="length"></param>
        /// <param name="characterSet"></param>
        /// <returns></returns>
        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", nameof(length));
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", nameof(length));
            if (characterSet == null)
                throw new ArgumentNullException(nameof(characterSet));
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", nameof(characterSet));
            
            var bytes = new byte[length * 8];
            RandomNumberGenerator.Create().GetBytes(bytes);
            var result = new char[length];
            for (var i = 0; i < length; i++)
            {
                var value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }

            return new string(result);
        }

    }
}