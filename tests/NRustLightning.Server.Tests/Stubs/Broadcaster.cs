using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NBitcoin;
using NRustLightning.Adaptors;
using NRustLightning.Handles;
using NRustLightning.Interfaces;
using NRustLightning.Utils;

namespace NRustLightning.Server.Tests.Stubs
{
        internal class TestBroadcaster : IBroadcaster
        {
            public ConcurrentBag<string> BroadcastedTxHex { get; } = new ConcurrentBag<string>();

            public TestBroadcaster()
            {
            }

            public void BroadcastTransaction(Transaction tx)
            {
                var hex = tx.ToHex();
                 BroadcastedTxHex.Add(hex);
            }
        }

}