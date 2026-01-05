namespace TCM.Infrastructure.Token
{
    public class JWTTokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public JWTTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region Generate Tokens
        public (string AccessToken, string RefreshToken) GenerateTokens(string username, string email, long userId, long userRoleId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim("RoleId", userRoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = GenerateAccessToken(claims);
            var refreshToken = GenerateRefreshToken(claims);

            return (accessToken, refreshToken);
        }
        #endregion

        #region Refresh Tokens
        public (string AccessToken, string RefreshToken) RefreshTokens(string refreshToken)
        {
            // Validate refresh token
            var principal = ValidateToken(refreshToken);
            var claims = principal.Claims.ToList();

            // Generate new tokens
            var newAccessToken = GenerateAccessToken(claims);
            var newRefreshToken = GenerateRefreshToken(claims);

            return (newAccessToken, newRefreshToken);
        }
        #endregion

        #region Access Token
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Refresh Token
        private string GenerateRefreshToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:RefreshTokenExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #region Validate Token
        public ClaimsPrincipal ValidateToken(string token, bool allowExpired = false)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = !allowExpired,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }
        #endregion
    }
}
