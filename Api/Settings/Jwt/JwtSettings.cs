namespace Filio.Api.Settings.Jwt;

public sealed class JwtSettings
{
    public string SigningSecret { get; init; } = null!;
    public string EncryptionSecret { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpirationMinutes { get; init; } = default!;
}