using System.Security.Cryptography;
using System.Text;

namespace MainUnit.Helper
{
    public class Cryptography
    {
        public static string ComputeSha512Hash(string input)
        {
            // Convert the input string to a byte array and compute the hash.
            using (SHA512 sha512Hash = SHA512.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
