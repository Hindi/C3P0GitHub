using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>Class that contains the function for password encryption and verification.</summary>
static class Crypto
{
    /// <summary>Encrypt a password with md5 algorithm.</summary>
    /// <param name="pass">The password to be encrypted</param>
    /// <returns>string : the encrypted password</returns>
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



    /// <summary>Checks that the password is correct.</summary>
    /// <param name="input">The password (clear) of the client</param>
    /// <param name="hash">The password (hash) registered for this client</param>
    /// <returns>bool : True if they correspond.</returns>
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
