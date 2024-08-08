using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Peter.Integration.Encryption
{
    /// <summary>
    /// Class to Handle Symmetric Encryption Used for Password encryption
    /// </summary>
    public class SymmetricEncryption
    {
        private static Logger Log = LogManager.GetCurrentClassLogger();
        private const string INITVECTOR = "OVE6DptjjihBypu9";
        private const string PASSPHRASE = "5chhtX0ANBc3bh3PuoaH!";
        // This constant is used to determine the keysize of the encryption algorithm
        private const int KEYSIZE = 256;
        /// <summary>
        /// Encrypts the string {plainText}
        /// </summary>
        /// <param name="plainText">The text to encrypt</param>
        /// <param name="passPhrase">The pass phrase used for the encryption</param>
        /// <returns>The encrypted string</returns>
        public static string EncryptString(string plainText, string passPhrase = null)
        {
            if (plainText == null || plainText == "")
                return "";
            if (passPhrase == null)
                passPhrase = PASSPHRASE;
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(INITVECTOR);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(KEYSIZE / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                byte[] cipherTextBytes = memoryStream.ToArray();
                memoryStream.Close();
                cryptoStream.Close();

                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }

        }

        /// <summary>
        /// Decrypts the string {cipherText}
        /// </summary>
        /// <param name="cipherText">The text to decrypt</param>
        /// <param name="passPhrase">The pass phrase to use in the encryption</param>
        /// <returns>The decrypted string</returns>
        public static string DecryptString(string cipherText, string passPhrase = null)
        {
            if (cipherText == null || cipherText == "")
                return "";
            if (passPhrase == null)
                passPhrase = PASSPHRASE;
            try
            {
                byte[] initVectorBytes = Encoding.UTF8.GetBytes(INITVECTOR);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
                byte[] keyBytes = password.GetBytes(KEYSIZE / 8);
                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();

                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            }
            catch (Exception e)
            {
                Log.Error(e);
                return null;
            }
        }
    }
}
