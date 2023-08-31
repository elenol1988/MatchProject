using Shared.JWToken;

namespace JWTAuthentication
{
    public interface IJWTokenManager
    {
        JWTokenResult GenerateToken(TokenGenerationRequest request);
    }
}
