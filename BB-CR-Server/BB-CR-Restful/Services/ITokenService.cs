namespace BB.CR.Rest.Services
{
    public interface ITokenService
    {
        void RevokeToken(string token);
        bool IsTokenRevoked(string token);
    }
}
