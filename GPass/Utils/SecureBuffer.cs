using System;
using System.Runtime.InteropServices;
using System.Security;

namespace GPass.Utils;

public sealed class SecureBuffer : IDisposable
{
    private readonly SecureString _secureString = new();
    private IntPtr _ptr = IntPtr.Zero;

    public void Append(string value)
    {
        foreach (var c in value)
            _secureString.AppendChar(c);
    }

    public byte[] ToByteArray()
    {
        _ptr = Marshal.SecureStringToBSTR(_secureString);
        var length = Marshal.ReadInt32(_ptr, -4);
        var result = new byte[length];
        Marshal.Copy(_ptr, result, 0, length);
        return result;
    }

    public void Dispose()
    {
        if (_ptr != IntPtr.Zero)
        {
            Marshal.ZeroFreeBSTR(_ptr);
            _ptr = IntPtr.Zero;
        }
        _secureString.Dispose();
    }
}