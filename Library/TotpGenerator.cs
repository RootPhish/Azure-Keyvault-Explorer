using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public static class TotpGenerator
    {
        public static string GenerateTotp(
            string base32Secret,
            int digits = 6,
            int timestepSeconds = 30,
            DateTime? timestamp = null)
        {
            byte[] key = Base32Decode(base32Secret);

            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime time = timestamp?.ToUniversalTime() ?? DateTime.UtcNow;

            long timestep = (long)(time - unixEpoch).TotalSeconds / timestepSeconds;

            byte[] timestepBytes = BitConverter.GetBytes(timestep);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(timestepBytes);

            using var hmac = new HMACSHA1(key);
            byte[] hash = hmac.ComputeHash(timestepBytes);

            int offset = hash[^1] & 0x0F;

            int binaryCode =
                ((hash[offset] & 0x7F) << 24) |
                ((hash[offset + 1] & 0xFF) << 16) |
                ((hash[offset + 2] & 0xFF) << 8) |
                (hash[offset + 3] & 0xFF);

            int otp = binaryCode % (int)Math.Pow(10, digits);

            return otp.ToString(new string('0', digits));
        }

        private static byte[] Base32Decode(string base32)
        {
            const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

            base32 = base32.TrimEnd('=').ToUpperInvariant();

            var bits = base32
                .Select(c => alphabet.IndexOf(c))
                .Where(i => i >= 0)
                .SelectMany(i => Convert.ToString(i, 2).PadLeft(5, '0'))
                .ToArray();

            var bytes = Enumerable
                .Range(0, bits.Length / 8)
                .Select(i => Convert.ToByte(
                    string.Concat(bits.Skip(i * 8).Take(8)), 2))
                .ToArray();

            return bytes;
        }
    }
}
