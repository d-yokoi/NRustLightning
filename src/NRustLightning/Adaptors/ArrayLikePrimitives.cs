/*
 * You can think these are just a type alias for Span<T>
 */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NRustLightning.Adaptors;
namespace NRustLightning.Adaptors
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFIScript
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFISecretKey
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFIPublicKey
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFISha256dHash
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;

        public FFISha256dHash(IntPtr ptr, UIntPtr len)
        {
            this.ptr = ptr;
            this.len = len;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFIOutPoint
    {
        internal readonly FFIScript script_pub_key;
        internal readonly ushort index;
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFITxOut
    {
        internal readonly ulong value;
        internal readonly FFIScript script_pub_key;
    }

    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFITransaction
    {
        public readonly IntPtr ptr;
        public readonly UIntPtr len;
    }
    [StructLayout(LayoutKind.Sequential)]
    public readonly ref struct FFIBlock
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;
    }
    
    #region internal members
   
    [StructLayout(LayoutKind.Sequential)]
    internal readonly ref struct FFIRoute
    {
        internal readonly IntPtr ptr;
        internal readonly UIntPtr len;

        public FFIRoute(IntPtr ptr, UIntPtr len)
        {
            this.ptr = ptr;
            this.len = len;
        }
    }
    # endregion
}
