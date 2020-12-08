using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PortaCapena.Authentication.NetCore.Abstraction;

namespace PortaCapena.Authentication.NetCore.Configuration
{
    public class TokenOptionsBuilder : ITokenOptionsBuilder
    {
        private readonly TokenOptions _tokenOptions;

        private TokenOptionsBuilder(TokenOptions tokenOptions)
        {
            _tokenOptions = tokenOptions;
        }

        /// <summary>
        /// Creates instance of builder.
        /// </summary>
        /// <param name="tokenName">Name of token</param>
        /// <returns><see cref="TokenOptionsBuilder"/></returns>
        public static TokenOptionsBuilder Create(string tokenName)
        {
            var tokenOptions = new TokenOptions { TokenName = tokenName };

            return new TokenOptionsBuilder(tokenOptions);
        }

        /// <summary>
        /// Sets the expiration time of token. Token will be valid for given time.
        /// </summary>
        /// <param name="expiration">Validation time</param>
        /// <returns>Self</returns>
        public ITokenOptionsBuilder SetExpiration(TimeSpan expiration)
        {
            _tokenOptions.Expiration = expiration;
            return this;
        }

        /// <summary>
        /// Sets the value of secret key stored in server side.
        /// </summary>
        /// <param name="key">Secret key</param>
        /// <returns>Self</returns>
        public ITokenOptionsBuilder SetSecretKey(string key)
        {
            _tokenOptions.SecretKey = key;
            return this;
        }

        /// <summary>
        /// Sets the algorithm to hash the key. By default uses <see cref="SecurityAlgorithms.HmacSha256"/>
        /// </summary>
        /// <param name="algorithm">Algorithm</param>
        /// <returns>Self</returns>
        public ITokenOptionsBuilder SetAlgorithm(string algorithm)
        {
            _tokenOptions.SecurityAlgorithm = algorithm;
            return this;
        }

        /// <summary>
        /// Enable token to auto refresh each request. By default value is set to true.
        /// </summary>
        /// <param name="key">Algorithm</param>
        /// <returns>Self</returns>
        public ITokenOptionsBuilder SetAutoRefresh(bool value)
        {
            _tokenOptions.AutoRefresh = value;
            return this;
        }

        /// <summary>
        /// Sets the name of exchanged token name applied in response header.
        /// By default header is named by "X-Access-Token"
        /// </summary>
        /// <param name="name">Name of the exchange token</param>
        /// <returns>Self</returns>
        public ITokenOptionsBuilder SetExchangeTokenName(string name)
        {
            _tokenOptions.ExchangeTokenName = name;
            return this;
        }

        /// <summary>
        /// Ends building token options
        /// </summary>
        /// <returns><see cref="TokenOptions"/> instance</returns>
        public TokenOptions Build()
        {
            if (string.IsNullOrEmpty(_tokenOptions.TokenName))
                throw new ArgumentException("Exception occurred while trying to build a token. Token name is required!");
            if (_tokenOptions.Expiration == default)
                throw new ArgumentException("Exception occurred while trying to build a token. Expiration is required!");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.SecretKey));

            _tokenOptions.SigningCredentials = new SigningCredentials(signingKey, string.IsNullOrEmpty(_tokenOptions.SecurityAlgorithm) ? SecurityAlgorithms.HmacSha256 : _tokenOptions.SecurityAlgorithm);

            return _tokenOptions;
        }
    }
}
