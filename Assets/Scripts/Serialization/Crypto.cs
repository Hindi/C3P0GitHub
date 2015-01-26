using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Crypto
{

    //ncrypt password
    public static string encryptMd5(string pass)
    {
        System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] bs = System.Text.Encoding.UTF8.GetBytes(pass);
        bs = md5Hasher.ComputeHash(bs);
        System.Text.StringBuilder s = new System.Text.StringBuilder();
        foreach (byte b in bs)
            s.Append(b.ToString("x2").ToLower());

        return s.ToString();
    }

    //Check password
    public static bool verifyMd5(string input, string hash)
    {
        string hashOfInput = encryptMd5(input);

        StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        if (0 == comparer.Compare(hashOfInput, hash))
            return true;
        else
            return false;
    }
}
