﻿/*
 *      Alessandro Cagliostro, 2019
 *      
 *      https://github.com/alecgn
 */

using CryptHash.Net.Hash.HashResults;
using CryptHash.Net.Hash.Base;

namespace CryptHash.Net.Hash
{
    public class HMAC_SHA_512 : HMACBase
    {
        public HMACHashResult HashBytes(byte[] bytesToBeHashed, byte[] key = null)
        {
            return base.ComputeHMAC(Enums.HashAlgorithm.SHA512, bytesToBeHashed, key);
        }

        public HMACHashResult HashString(string stringToBeHashed, byte[] key = null)
        {
            return base.ComputeHMAC(Enums.HashAlgorithm.SHA512, stringToBeHashed, key);
        }

        public HMACHashResult HashFile(string sourceFilePath, byte[] key = null)
        {
            return base.ComputeFileHMAC(Enums.HashAlgorithm.SHA512, sourceFilePath, key);
        }

        public HMACHashResult VerifyHMAC(string base64HMACString, string stringToVerifyHMAC, byte[] key)
        {
            return base.VerifyHMAC(Enums.HashAlgorithm.SHA512, base64HMACString, stringToVerifyHMAC, key);
        }

        public HMACHashResult VerifyHMAC(byte[] hmacBytes, byte[] bytesToVerifyHMAC, byte[] key)
        {
            return base.VerifyHMAC(Enums.HashAlgorithm.SHA512, hmacBytes, bytesToVerifyHMAC, key);
        }

        public HMACHashResult VerifyFileHMAC(string base64HMACString, string sourceFilePath, byte[] key)
        {
            return base.VerifyFileHMAC(Enums.HashAlgorithm.SHA512, base64HMACString, sourceFilePath, key);
        }

        public HMACHashResult VerifyFileHMAC(byte[] hmacBytes, string sourceFilePath, byte[] key)
        {
            return base.VerifyFileHMAC(Enums.HashAlgorithm.SHA512, hmacBytes, sourceFilePath, key);
        }
    }
}