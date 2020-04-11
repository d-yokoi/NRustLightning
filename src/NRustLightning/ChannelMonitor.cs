using System;
using System.Runtime.InteropServices;
using System.Text;
using NRustLightning.Adaptors;
using NRustLightning.Utils;
using NRustLightning.Handles;
using NRustLightning.Interfaces;

namespace NRustLightning
{
    public sealed class ChannelMonitor : IDisposable
    {
        internal readonly ChannelMonitorHandle Handle;
        
        internal ChannelMonitor(
            ChannelMonitorHandle handle
            )
        {
            Handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }
        public static ChannelMonitor Create(
            IChainWatchInterface chainWatchInterface,
            IBroadcaster broadcaster,
            ILogger logger,
            IFeeEstimator feeEstimator
            )
        {
            Interop.create_ffi_channel_monitor(
                ref chainWatchInterface.InstallWatchTx,
                ref chainWatchInterface.InstallWatchOutPoint,
                ref chainWatchInterface.WatchAllTxn,
                ref chainWatchInterface.GetChainUtxo,
                ref chainWatchInterface.FilterBlock,
                ref broadcaster.BroadcastTransaction,
                ref logger.Log,
                ref feeEstimator.getEstSatPer1000Weight,
                out var handle);
            return new ChannelMonitor(handle);
        }

        public void Dispose()
        {
            Handle.Dispose();
        }
    }
}