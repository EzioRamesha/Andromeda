using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shared
{
    public class Hash
    {
        public static string HashFileName(string filename)
        {
            return string.Format("{0}_{1}{2}",
                Sha256(Path.GetFileNameWithoutExtension(filename)),
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                Path.GetExtension(filename)
            );
        }

        public static string Sha256(string source)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, source);
            }
        }

        public static bool Sha256Verify(string source, string hash)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return VerifyHash(sha256Hash, source, hash);
            }
        }

        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
}
