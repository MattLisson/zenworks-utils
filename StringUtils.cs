using System;
using System.Text;

namespace Zenworks.Utils {
    public static class StringUtils {
        public static string Base64Encode(string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64) {
            byte[] data = Convert.FromBase64String(base64);
            return Encoding.UTF8.GetString(data);
        }
    }
}
