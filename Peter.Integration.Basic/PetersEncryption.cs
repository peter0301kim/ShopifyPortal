using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Peter.Integration.Basic;

public class PetersEncryption
{
    public const string PetersLibraryInternalDll = @"lib\Peters.Library.Internal.dll";

    [DllImport(PetersLibraryInternalDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr encrypt(string plainText);

    [DllImport(PetersLibraryInternalDll, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr decrypt(string cipherText);

    public static string EncryptString(string plainText)
    {
        var ptrCipherText = encrypt(plainText);
        var cipherText = Marshal.PtrToStringAnsi(ptrCipherText) ?? "";

        return cipherText;
    }

    public static string DecryptString(string cipherText)
    {
        var ptrPlainText = decrypt(cipherText);
        var plainText = Marshal.PtrToStringAnsi(ptrPlainText) ?? "";

        return plainText;
    }

}
