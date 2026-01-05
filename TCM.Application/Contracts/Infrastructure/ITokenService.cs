namespace TCM.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        #region GenerateTokens
        /// <summary>
        /// Generate Access Token and Refresh Token
        /// </summary>
        public (string AccessToken, string RefreshToken) GenerateTokens(string username, string email, long userId, long userRoleId);
        #endregion

        #region GenerateTokens
        /// <summary>
        /// Generate a new access + refresh token from previous refresh token
        /// </summary>
        public (string AccessToken, string RefreshToken) RefreshTokens(string refreshToken);
        #endregion
    }
}
