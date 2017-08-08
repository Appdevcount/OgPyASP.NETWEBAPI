﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace PayitDealerGlobalPayit.Common
{
    public class Security
    {
       static string master_key = "NANNUEVARUHACKCHEYYALERUMEMUTAPA";
       static string master_IV = "SHARHARIRIYATAIY";

        public static string encrypt(string plaintext)
        {


            string original = plaintext;

            // Create a new instance of the Aes 
            // class.  This generates a new key and initialization  
            // vector (IV). 
            using (Aes myAes = Aes.Create())
            {

                myAes.Key = System.Text.Encoding.UTF8.GetBytes(master_key);
                myAes.IV = System.Text.Encoding.UTF8.GetBytes(master_IV);
                // Encrypt the string to an array of bytes. 
                byte[] encrypted = EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

                String entext = Convert.ToBase64String(encrypted);

                return entext;

            }



        }

        public static string decrypt(string ciphertext)
        {


            string original = ciphertext;

            // Create a new instance of the Aes 
            // class.  This generates a new key and initialization  
            // vector (IV). 
            using (Aes myAes = Aes.Create())
            {

                myAes.Key = System.Text.Encoding.UTF8.GetBytes(master_key);
                myAes.IV = System.Text.Encoding.UTF8.GetBytes(master_IV);


                // Decrypt the bytes to a string. 
                string roundtrip = DecryptStringFromBytes_Aes(Convert.FromBase64String(original), myAes.Key, myAes.IV);


                Console.WriteLine("Round Trip: {0}", roundtrip);

                return roundtrip;
            }


        }



        static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream. 
            return encrypted;

        }

        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}
