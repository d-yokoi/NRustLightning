using System;

namespace NRustLightning.Server.Utils
{
    public class NRustLightningException : Exception
    {
        public NRustLightningException(string msg): base (msg) {}
        public NRustLightningException(string msg, Exception inner): base (msg, inner) {}
    }
}