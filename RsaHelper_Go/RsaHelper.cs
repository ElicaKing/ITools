using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RsaHelper_Go
{

        

    public static class RsaHelper
    {
        public static object RSA1 = CryptoConfig.CreateFromName("SHA1WithRSA");
        public static object RSA2 = CryptoConfig.CreateFromName("SHA256");
        #region 加签
        /// <summary>  
        /// 签名  
        /// </summary>  
        /// <param name="content">待签名字符串</param>  
        /// <param name="privateKey">私钥</param>  
        /// <param name="input_charset">编码格式</param>  
        /// <returns>签名后字符串</returns>  
        public static string sign(string content, string privateKey, string input_charset, object RSAType)
        {
            byte[] Data = Encoding.GetEncoding(input_charset).GetBytes(content);
            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            byte[] signData = rsa.SignData(Data, RSAType);
            return Convert.ToBase64String(signData);
        }//CryptoConfig.CreateFromName("SHA1WithRSA")
        //CryptoConfig.CreateFromName("SHA256")

        private static RSACryptoServiceProvider DecodePemPrivateKey(String pemstr)
        {
            byte[] pkcs8privatekey;
            pkcs8privatekey = Convert.FromBase64String(pemstr);
            if (pkcs8privatekey != null)
            {
                RSACryptoServiceProvider rsa = DecodePrivateKeyInfo(pkcs8privatekey);
                return rsa;
            }
            else
                return null;
        }
        private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            MemoryStream mem = new MemoryStream(pkcs8);
            int lenstream = (int)mem.Length;
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;

            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x02)
                    return null;

                twobytes = binr.ReadUInt16();

                if (twobytes != 0x0001)
                    return null;

                seq = binr.ReadBytes(15);
                if (!CompareBytearrays(seq, SeqOID))
                    return null;

                bt = binr.ReadByte();
                if (bt != 0x04)
                    return null;

                bt = binr.ReadByte();
                if (bt == 0x81)
                    binr.ReadByte();
                else
                    if (bt == 0x82)
                    binr.ReadUInt16();

                byte[] rsaprivkey = binr.ReadBytes((int)(lenstream - mem.Position));
                RSACryptoServiceProvider rsacsp = DecodeRSAPrivateKey(rsaprivkey);
                return rsacsp;
            }

            catch (Exception)
            {
                return null;
            }

            finally { binr.Close(); }
        }
        private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    return null;

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    return null;
                bt = binr.ReadByte();
                if (bt != 0x00)
                    return null;

                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);

                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                RSA.ImportParameters(RSAparams);
                return RSA;
            }
            catch (Exception)
            {
                return null;
            }
            finally { binr.Close(); }
        }
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }
        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }
        #endregion
        #region 分段加密
        /// <summary>
        /// RSA分段加密;用于对超长字符串加密
        /// </summary>
        /// <param name="toEncryptString">需要进行加密的字符串</param>
        /// <param name="publickKey">公钥</param>
        /// <returns>加密后的密文</returns>
        public static string SectionEncrypt(string toEncryptString, string privateKey)
        {
            string base64code = string.Empty;

            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            byte[] dataToEncrypt = ByteConverter.GetBytes(toEncryptString);

            RSACryptoServiceProvider rsa = DecodePemPrivateKey(privateKey);
            SHA1 sh = new SHA1CryptoServiceProvider();
            //rsa.FromXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(publickKey)));

            int MaxBlockSize = rsa.KeySize / 8 - 11;

            if (dataToEncrypt.Length <= MaxBlockSize)
            {
                byte[] encrytedData;

                encrytedData = RSAEncrypt(dataToEncrypt, rsa.ExportParameters(false), false);

                base64code = Convert.ToBase64String(encrytedData);

                return base64code;
            }

            MemoryStream plaiStream = new MemoryStream(dataToEncrypt);

            MemoryStream CrypStream = new MemoryStream();

            Byte[] Buffer = new Byte[MaxBlockSize];

            int BlockSize = plaiStream.Read(Buffer, 0, MaxBlockSize);

            while (BlockSize > 0)
            {
                Byte[] ToEncrypt = new Byte[BlockSize];
                Array.Copy(Buffer, 0, ToEncrypt, 0, BlockSize);

                Byte[] Cryptograph = RSAEncrypt(ToEncrypt, rsa.ExportParameters(false), false);
                CrypStream.Write(Cryptograph, 0, Cryptograph.Length);

                BlockSize = plaiStream.Read(Buffer, 0, MaxBlockSize);
            }

            base64code = Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);

            return base64code;
        }
        private static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(RSAKeyInfo);

            return RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
        }
        #endregion
        #region rsa解密
        /// <summary>
        /// rsa解密
        /// </summary>
        /// <param name="DataToDecrypt">需要解密的数据</param>
        /// <param name="RSAKeyInfo">私钥</param>
        /// <param name="DoOAEPPadding"></param>
        /// <returns></returns>
        private static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.ImportParameters(RSAKeyInfo);

            return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
        }
        #endregion
    }
}

