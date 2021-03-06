using System;
using System.Text;
using System.Transactions;
using Xunit;
using NRustLightning;
using NRustLightning.Adaptors;

namespace NRustLightning.Tests
{
    public class LastResultTests
    {
        [Fact]
        public void NativeErrorsShouldBecomeExceptions()
        {
            var nativeException = Assert.Throws<FFIException>(() => Interop.ffi_test_error());
            Assert.Equal("FFI against rust-lightning failed (InternalError), Error: A test error.", nativeException.Message);
        }

        [Fact]
        public void NativeErrorUsesDefaultErrorMessageWhenLastResultChangesToNewError()
        {
            var nativeResult = Interop.ffi_test_error(false);
            Interop.ffi_test_error(false);

            var nativeException = Assert.Throws<FFIException>(() => nativeResult.Check());
            Assert.Equal("FFI against rust-lightning failed with InternalError", nativeException.Message);
        }
        
        [Fact]
        public void NativeErrorsUseDefaultMessageWhenLastResultChangesToOk()
        {
            var nativeResult = Interop.ffi_test_error(false);
            Interop.ffi_test_ok();

            var nativeException = Assert.Throws<FFIException>(() => nativeResult.Check());
            Assert.Equal("FFI against rust-lightning failed with InternalError", nativeException.Message);
        }
    }
}
