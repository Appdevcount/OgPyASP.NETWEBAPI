using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PayitDealerGlobalPayit.Common
{
    public class Hash
    {
        public Hash() { }

        public enum HashType : int
        {
            MD5,
            SHA1,
            SHA256,
            SHA512,
            HMACSHA256
        }

        public static string GetHash(string text, HashType hashType)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5:
                    hashString = GetMD5(text);
                    break;
                case HashType.SHA1:
                    hashString = GetSHA1(text);
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(text);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(text);
                    break;
                default:
                    hashString = "Invalid Hash Type";
                    break;
            }
            return hashString;
        }

        public static string GetHash(string text, HashType hashType, string key)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5:
                    hashString = GetMD5(text);
                    break;
                case HashType.SHA1:
                    hashString = GetSHA1(text);
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(text);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(text);
                    break;
                case HashType.HMACSHA256:
                    hashString = GetHMACSHA256(text, key);
                    break;
                default:
                    hashString = "Invalid Hash Type";
                    break;
            }
            return hashString;
        }

        public static bool CheckHash(string original, string hashString, HashType hashType)
        {
            string originalHash = GetHash(original, hashType);
            return (originalHash == hashString);
        }

        private static string GetMD5(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            MD5 hashString = new MD5CryptoServiceProvider();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA1(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA256(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA256Managed hashString = new SHA256Managed();

            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        //private static string GetHMACSHA256(string text,string key)
        //{
        //    UnicodeEncoding UE = new UnicodeEncoding();
        //    byte[] hashValue;
        //    byte[] message = UE.GetBytes(text);

        //    HMACSHA256 hashString = new HMACSHA256(UE.GetBytes(key));

        //    string hex = "";

        //    hashValue = hashString.ComputeHash(message);
        //    foreach (byte x in hashValue)
        //    {
        //        hex += String.Format("{0:x2}", x);
        //    }
        //    return hex;
        //}

        private static string GetHMACSHA256(string text, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(key);

            //HMACMD5 hmacmd5 = new HMACMD5(keyByte);
            //HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            HMACSHA256 hmac256Key = new HMACSHA256(keyByte);



            byte[] messageBytes = encoding.GetBytes(text);
            byte[] hasmmessage = hmac256Key.ComputeHash(messageBytes);

            return ByteToString(hasmmessage);

            //byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);

            //this.hmac1.Text = ByteToString(hashmessage);

            //byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
            //this.hmac2.Text = ByteToString(hashmessage); 




        }


        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }

        private static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}
