using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Core.PolindromHasher
{
    public class PolinomialHasher : IPasswordHasher
    {
        private const string SplitString = "-";

        private const int
            ByteLenght = 3,
            DoubleLenght = 24,
            MaxPasswordDiff = 4,
            CeilingConst = 10000;

        public string HashPassword(string userName, string password)
        {
            string constSalt = Guid
                .NewGuid()
                .ToString()
                .ToUpper()
                .Replace(SplitString, string.Empty);

            return HashPassword(userName, password, constSalt);
        }

        private string HashPassword(string userName, string password, string constSalt)
        {
            List<double> salt = InitSalt(constSalt, userName.ToUpper());
            List<double> hash = HashString(password, salt, constSalt);

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(FromHashToString(hash) + SplitString + constSalt));
        }

        private static List<double> InitSalt(string saltConst, string userName)
        {
            List<double> constSaltDouble =
                DoubleHasherHMACSHA1(userName, saltConst).ToList(),
                salt = new();

            for (int i = 1; i <= constSaltDouble.Count; i++)
            {
                double saltElement = Math.Ceiling(CeilingConst * (Math.Abs(constSaltDouble[i - 1]) + CeilingConst));
                double constCos = Math.Ceiling(CeilingConst * (2 + Math.Cos(Encoding.ASCII.GetBytes(saltConst).First())));

                salt.Add(saltElement * constCos);
            }

            return salt;
        }

        private static List<double> HashString(string str, List<double> salt, string saltConst)
        {
            double startCharHash = StringHasherHMACSHA1(new string(new char[] { str[0] }), saltConst);
            List<double> hash = new() { salt[0] * startCharHash };

            for (int i = 1; i < str.Length; i++)
            {
                hash.Add(hash[i - 1] - (salt[i] * StringHasherHMACSHA1(new string(new char[] { str[i] }), saltConst)));
            }

            return hash;
        }

        private static IEnumerable<double> DoubleHasherHMACSHA1(string value, string constSalt)
        {
            byte[] unicodeValue = new byte[value.Length * 2],
                   unicodeSalt = new byte[constSalt.Length * 2];

            _ = Encoding.ASCII.GetEncoder().GetBytes(value.ToCharArray(), 0, value.Length, unicodeValue, 0, true);
            _ = Encoding.ASCII.GetEncoder().GetBytes(constSalt.ToCharArray(), 0, constSalt.Length, unicodeSalt, 0, true);

            return new HMACSHA1(unicodeSalt)
                .ComputeHash(unicodeValue)
                .SelectMany(value => value.ToString("X2")
                    .Select(c => char.GetNumericValue(c)));
        }

        private static double StringHasherHMACSHA1(string path, string saltConst)
        {
            return DoubleHasherHMACSHA1(path, saltConst).Sum();
        }

        private static string FromHashToString(List<double> hash)
        {
            return string.Concat(hash.Select(d => FromDoubleToString(d)));
        }

        private static string FromDoubleToString(double value)
        {
            string result = string.Empty;
            foreach (byte bytes in BitConverter.GetBytes(value))
            {
                string strByte = bytes.ToString();
                if (strByte.Length < ByteLenght)
                {
                    strByte = 0 + strByte;
                }

                if (strByte.Length < ByteLenght)
                {
                    strByte = 0 + strByte;
                }

                result += strByte;
            }
            return result;
        }

        private static List<double> FromStringToHashList(string exitstString)
        {
            List<double> hashFromDb = new();
            for (int j = 0; j < exitstString.Length; j += DoubleLenght)
            {
                string hashChar = exitstString[j..(j + DoubleLenght)];
                List<byte> hashCharByte = new();

                for (int i = 0; i < hashChar.Length; i += ByteLenght)
                {
                    hashCharByte.Add(byte.Parse(hashChar[i..(i + ByteLenght)]));
                }

                hashFromDb.Add(BitConverter.ToDouble(hashCharByte.ToArray()));
            }

            return hashFromDb;
        }

        public bool ComparePassword(string userName, string hashPassword, string newPassword)
        {
            string[] hashSplit = Encoding.UTF8.GetString(Convert.FromBase64String(hashPassword)).Split(SplitString);

            List<double> salt = InitSalt(hashSplit.Last(), userName.ToUpper());

            List<double> newPasswordHash = HashString(newPassword, salt, hashSplit.Last());
            List<double> oldPasswordHash = FromStringToHashList(hashSplit.First());

            if (Math.Abs(oldPasswordHash.Count - newPasswordHash.Count) > MaxPasswordDiff)
            {
                return true;
            }

            return DiffCount(oldPasswordHash, newPasswordHash, salt) > MaxPasswordDiff;
        }

        private static int DiffCount(List<double> oldPasswordHash, List<double> newPasswordHash, List<double> salt)
        {
            List<int> rmIndexOld = new(), rmIndexNew = new();

            for (int i = 0; i < MaxPasswordDiff; i++)
            {
                for (int j = 0; j < MaxPasswordDiff; j++)
                {
                    Compare(oldPasswordHash, newPasswordHash, salt, rmIndexOld, rmIndexNew, j, i);
                }
            }

            rmIndexOld = rmIndexOld.Distinct().OrderByDescending(x => x).ToList();
            rmIndexNew = rmIndexNew.Distinct().OrderByDescending(x => x).ToList();

            foreach (int i in rmIndexOld)
            {
                oldPasswordHash.RemoveAt(i);
            }

            foreach (int i in rmIndexNew)
            {
                newPasswordHash.RemoveAt(i);
            }

            return newPasswordHash.Count;
        }

        private static void Compare(List<double> oldPasswordHash, List<double> newPasswordHash, List<double> salt, List<int> rmIndexOld, List<int> rmIndexNew, int offsetOldHash, int offsetNewHash)
        {
            for (; oldPasswordHash.Count - (offsetOldHash + 1) > 0 && newPasswordHash.Count - (offsetNewHash + 1) > 0; ++offsetNewHash, ++offsetOldHash)
            {
                if (CompareChar(newPasswordHash, oldPasswordHash, salt, offsetNewHash, offsetOldHash) &&
                    CompareChar(newPasswordHash, oldPasswordHash, salt, offsetNewHash + 1, offsetOldHash + 1))
                {
                    rmIndexOld.AddRange(new int[] { offsetOldHash, offsetOldHash + 1 });
                    rmIndexNew.AddRange(new int[] { offsetNewHash, offsetNewHash + 1 });
                }
            }
        }

        private static bool CompareChar(List<double> exist, List<double> comparable, List<double> salt, int i, int j)
        {
            return GetHash(exist, i, salt[i]) == GetHash(comparable, j, salt[j]);
        }

        private static double GetHash(List<double> h, int i, double salt)
        {
            return i > 0 ? (h[i - 1] - h[i]) / salt : h[i] / salt;
        }

        public PasswordVerificationResult VerifyHashedPassword(string userName, string hashedPassword, string providedPassword)
        {
            return (string.IsNullOrEmpty(hashedPassword) && string.IsNullOrEmpty(providedPassword)) ||
                (hashedPassword == HashPassword(userName, providedPassword, Encoding.UTF8.GetString(Convert.FromBase64String(hashedPassword)).Split(SplitString).Last())) ?
                PasswordVerificationResult.Success :
                PasswordVerificationResult.Failed;
        }
    }
}
