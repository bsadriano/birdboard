using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Birdboard.API.Test.Helper;

public static class JwtTokenProvider
{
    // fake issuer - can be anything
    public static string Issuer { get; } = "Sample_Auth_Server_For_Authentication";

    // random signing key - used to sign and validate the tokens
    public static SecurityKey SecurityKey { get; } =
        new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes("This_is_a_super_secure_key_that_has_a_very_long_string_of_characters_and_you_know_it")
        );

    // the signing credentials used by the token handler to sign tokens
    public static SigningCredentials SigningCredentials { get; } =
        new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha512Signature);

    // the token handler used to actually issue tokens
    public static readonly JwtSecurityTokenHandler JwtSecurityTokenHandler = new();
}
