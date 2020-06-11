using System;
using System.Runtime.InteropServices;
using NBitcoin.RPC;
using NRustLightning.Adaptors;
using NRustLightning.Interfaces;
using NRustLightning.Server.Extensions;

namespace NRustLightning.Server.FFIProxies
{
    /// <summary>
    ///  TODO: use cache
    /// </summary>
    public class BitcoinCoreFeeEstimator : IFeeEstimator
    {
        private readonly RPCClient rpc;
        private GetEstSatPer1000Weight _getEstSatPer1000Weight;
        public BitcoinCoreFeeEstimator(RPCClient rpc)
        {
            this.rpc = rpc;
            _getEstSatPer1000Weight = target =>
            {
                var blockCountTarget =
                    target switch
                    {
                        FFIConfirmationTarget.Background => 30,
                        FFIConfirmationTarget.Normal => 6,
                        FFIConfirmationTarget.HighPriority => 1,
                        _ => throw new Exception("Unreachable!")
                    };
                var feeRate = rpc.EstimateSmartFee(blockCountTarget, EstimateSmartFeeMode.Conservative).FeeRate;
                var virtualWeight = 1000;
                var h = feeRate.GetFee(virtualWeight);
                return (ulong)h.Satoshi;
            };
        }

        public ref GetEstSatPer1000Weight getEstSatPer1000Weight => ref _getEstSatPer1000Weight;
    }
}