using System;
using System.Runtime.InteropServices;

namespace DotNetLightning.LDK.Adaptors
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void InstallWatchTx(ref FFIChainWatchInterface self, ref FFISha256dHash txid, ref FFIScript spk);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void InstallWatchOutPoint(ref FFIChainWatchInterface self, ref FFIOutPoint spk, ref FFIScript outScript);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void WatchAllTxn(ref FFIChainWatchInterface self);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void GetChainUtxo(ref FFIChainWatchInterface self, ref FFISha256dHash genesisHash, ulong utxoId, ref ChainError error, ref FFITxOut txout);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void FilterBlock(ref FFIChainWatchInterface self, ref FFIBlock block);
    
    [StructLayout(LayoutKind.Sequential)]
    internal ref struct FFIChainWatchInterface
    {
        internal InstallWatchTx InstallWatchTx;
        internal InstallWatchOutPoint InstallWatchOutPoint;
        internal WatchAllTxn WatchAllTxn;
        internal GetChainUtxo GetChainUtxo;
        internal FilterBlock FilterBlock;
    }
}