using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNetLightning.LDK.Adaptors;
using DotNetLightning.LDK.Handles;
using DotNetLightning.LDK.Interfaces;
using DotNetLightning.LDK.Tests.Utils;
using DotNetLightning.LDK.Utils;
using Xunit;

namespace DotNetLightning.LDK.Tests
{
    public class BroadcasterTests
    {
        [Fact]
        public void BroadcasterTestSimple()
        {
            var tb = new TestBroadcaster();
            var broadcaster = Broadcaster.Create(tb);
            // should not throw error.
            broadcaster.Broadcast();
            var expectedHex = "020000000001031cfbc8f54fbfa4a33a30068841371f80dbfe166211242213188428f437445c91000000006a47304402206fbcec8d2d2e740d824d3d36cc345b37d9f65d665a99f5bd5c9e8d42270a03a8022013959632492332200c2908459547bf8dbf97c65ab1a28dec377d6f1d41d3d63e012103d7279dfb90ce17fe139ba60a7c41ddf605b25e1c07a4ddcb9dfef4e7d6710f48feffffff476222484f5e35b3f0e43f65fc76e21d8be7818dd6a989c160b1e5039b7835fc00000000171600140914414d3c94af70ac7e25407b0689e0baa10c77feffffffa83d954a62568bbc99cc644c62eb7383d7c2a2563041a0aeb891a6a4055895570000000017160014795d04cc2d4f31480d9a3710993fbd80d04301dffeffffff06fef72f000000000017a91476fd7035cd26f1a32a5ab979e056713aac25796887a5000f00000000001976a914b8332d502a529571c6af4be66399cd33379071c588ac3fda0500000000001976a914fc1d692f8de10ae33295f090bea5fe49527d975c88ac522e1b00000000001976a914808406b54d1044c429ac54c0e189b0d8061667e088ac6eb68501000000001976a914dfab6085f3a8fb3e6710206a5a959313c5618f4d88acbba20000000000001976a914eb3026552d7e3f3073457d0bee5d4757de48160d88ac0002483045022100bee24b63212939d33d513e767bc79300051f7a0d433c3fcf1e0e3bf03b9eb1d70220588dc45a9ce3a939103b4459ce47500b64e23ab118dfc03c9caa7d6bfc32b9c601210354fd80328da0f9ae6eef2b3a81f74f9a6f66761fadf96f1d1d22b1fd6845876402483045022100e29c7e3a5efc10da6269e5fc20b6a1cb8beb92130cc52c67e46ef40aaa5cac5f0220644dd1b049727d991aece98a105563416e10a5ac4221abac7d16931842d5c322012103960b87412d6e169f30e12106bdf70122aabb9eb61f455518322a18b920a4dfa887d30700".ToUpper();
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));
            
            // running GC should not change the behavior
            object _garbage = null;
            for (var i = 0; i <= 10000000; i++) {_garbage = new object();} // lots of garbage
            _garbage = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            broadcaster.Broadcast();
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));
            
            // Running asynchronously should not change the behavior.
            var tasks = new Task[20];
            for (var i = 0; i < 20; i++)
            {
                tasks[i] = Task.Run(() => { broadcaster.Broadcast(); });
            }
            Task.WaitAll(tasks);
            Assert.Equal(22, tb.BroadcastedTxHex.Count);
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));
            
            broadcaster.Dispose();
            Assert.Throws<ObjectDisposedException>(() => broadcaster.Broadcast());
        }

        
        [Fact]
        public void BroadcasterTestWrapper()
        {
            var tb = new TestBroadcaster();
            var wrapper = BroadcasterWrapper.Create(tb);
            wrapper.Broadcast();
            Assert.Single(tb.BroadcastedTxHex);
            var expectedHex = "020000000001031cfbc8f54fbfa4a33a30068841371f80dbfe166211242213188428f437445c91000000006a47304402206fbcec8d2d2e740d824d3d36cc345b37d9f65d665a99f5bd5c9e8d42270a03a8022013959632492332200c2908459547bf8dbf97c65ab1a28dec377d6f1d41d3d63e012103d7279dfb90ce17fe139ba60a7c41ddf605b25e1c07a4ddcb9dfef4e7d6710f48feffffff476222484f5e35b3f0e43f65fc76e21d8be7818dd6a989c160b1e5039b7835fc00000000171600140914414d3c94af70ac7e25407b0689e0baa10c77feffffffa83d954a62568bbc99cc644c62eb7383d7c2a2563041a0aeb891a6a4055895570000000017160014795d04cc2d4f31480d9a3710993fbd80d04301dffeffffff06fef72f000000000017a91476fd7035cd26f1a32a5ab979e056713aac25796887a5000f00000000001976a914b8332d502a529571c6af4be66399cd33379071c588ac3fda0500000000001976a914fc1d692f8de10ae33295f090bea5fe49527d975c88ac522e1b00000000001976a914808406b54d1044c429ac54c0e189b0d8061667e088ac6eb68501000000001976a914dfab6085f3a8fb3e6710206a5a959313c5618f4d88acbba20000000000001976a914eb3026552d7e3f3073457d0bee5d4757de48160d88ac0002483045022100bee24b63212939d33d513e767bc79300051f7a0d433c3fcf1e0e3bf03b9eb1d70220588dc45a9ce3a939103b4459ce47500b64e23ab118dfc03c9caa7d6bfc32b9c601210354fd80328da0f9ae6eef2b3a81f74f9a6f66761fadf96f1d1d22b1fd6845876402483045022100e29c7e3a5efc10da6269e5fc20b6a1cb8beb92130cc52c67e46ef40aaa5cac5f0220644dd1b049727d991aece98a105563416e10a5ac4221abac7d16931842d5c322012103960b87412d6e169f30e12106bdf70122aabb9eb61f455518322a18b920a4dfa887d30700".ToUpper();
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));
            
            // running GC should not change the behavior
            object _garbage = null;
            for (var i = 0; i <= 10000000; i++) {  _garbage = new object();} // lots of garbage
            _garbage = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            wrapper.Broadcast();

            Assert.Equal(2, tb.BroadcastedTxHex.Count);
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));
            
            // Running asynchronously should not change the behavior.
            var tasks = new Task[20];
            for (var i = 0; i < 20; i++)
            {
                tasks[i] = Task.Run(() => { wrapper.Broadcast(); });
            }
            Task.WaitAll(tasks);
            Assert.Equal(22, tb.BroadcastedTxHex.Count);
            Assert.All(tb.BroadcastedTxHex, (item) => Assert.Equal( expectedHex,item));

            wrapper.Dispose();
            Assert.Throws<ObjectDisposedException>(() => wrapper.Broadcast());
        }
    }
}