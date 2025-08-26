using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace SAS.Gateway.Infrastructure.DependencyInjection
{
    public static class AuthenticationDependencyInjection
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException("JwtSettings:Issuer");
            var audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException("JwtSettings:Audience");
            var publicKeyPath = configuration["JwtSettings:PublicKeyPath"] ?? throw new ArgumentNullException("JwtSettings:PublicKeyPath");

            if (!File.Exists(publicKeyPath))
                throw new FileNotFoundException($"Public key file not found at {publicKeyPath}");

            // Read public key (PEM or raw base64)
            var publicKeyText = File.ReadAllText(publicKeyPath).Trim();

            // If PEM format, strip headers/footers
            if (publicKeyText.StartsWith("-----BEGIN PUBLIC KEY-----"))
            {
                publicKeyText = publicKeyText
                    .Replace("-----BEGIN PUBLIC KEY-----", string.Empty)
                    .Replace("-----END PUBLIC KEY-----", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty)
                    .Trim();
            }

            var keyBytes = Convert.FromBase64String(publicKeyText);

            var rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
            var publicKey = new RsaSecurityKey(rsa);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = publicKey,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
