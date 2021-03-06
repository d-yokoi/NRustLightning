using DotNetLightning.Payment;
using NBitcoin;
using NRustLightning.Interfaces;
using NRustLightning.Server.Repository;

namespace NRustLightning.Server.Interfaces
{
    public interface IKeysRepository : IKeysInterface
    {
        PubKey GetNodeId() => this.GetNodeSecret().PubKey;

        RepositorySerializer Serializer { get; set; }
    }
    
    public class KeysRepoMessageSigner : IMessageSigner
    {
        private readonly IKeysRepository _keysRepository;
        public KeysRepoMessageSigner(IKeysRepository keysRepository)
        {
            _keysRepository = keysRepository;
        }
        public byte[] SignMessage(uint256 obj0) =>
            _keysRepository.GetNodeSecret().SignCompact(obj0, false);
    }

    public static class IKeysRepositoryExtension
    {
        public static IMessageSigner AsMessageSigner(this IKeysRepository repo)
        {
            return new KeysRepoMessageSigner(repo);
        }
    }
}