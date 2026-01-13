using System.Collections.Concurrent;

namespace BB.CR.Rest.Services
{
    public class TokenService : ITokenService
    {
        private readonly ConcurrentDictionary<string, DateTime> _revokedTokens = new();

        public bool IsTokenRevoked(string token)
        {
            return _revokedTokens.ContainsKey(token);
        }

        public void RevokeToken(string token)
        {
            _revokedTokens.TryAdd(token, DateTime.Now);
        }
    }
}
