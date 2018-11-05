using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PortaCapena.Authentication.NetCore.Configuration
{
    public class TokenOptionsBuilder : ITokenOptionsBuilder
    {
        private readonly TokenOptions _tokenOptions;

        public TokenOptionsBuilder()
        {
            _tokenOptions = new TokenOptions();
        }

        ///<inheritdoc />
        public ITokenOptionsBuilder SetExpiration(TimeSpan expiration)
        {
            _tokenOptions.Expiration = expiration;
            return this;
        }
        ///<inheritdoc />
        public ITokenOptionsBuilder SetSecretKey(string key)
        {
            _tokenOptions.SecretKey = key;
            return this;
        }
        ///<inheritdoc />
        public ITokenOptionsBuilder SetTokenName(string tokenName)
        {
            _tokenOptions.TokenName = tokenName;
            return this;
        }
        ///<inheritdoc />
        public ITokenOptionsBuilder SetAlgorithm(string algorithm)
        {
            _tokenOptions.SecurityAlgorithm = algorithm;
            return this;
        }
        ///<inheritdoc />
        public ITokenOptionsBuilder SetAutoRefresh(bool value)
        {
            _tokenOptions.AutoRefresh = value;
            return this;
        }
        ///<inheritdoc />
        public ITokenOptionsBuilder SetExchangeTokenName(string name)
        {
            _tokenOptions.ExchangeTokenName = name;
            return this;
        }

        ///<inheritdoc />
        public TokenOptions Build()
        {
            if (string.IsNullOrEmpty(_tokenOptions.TokenName))
                throw new ArgumentException("Exception occured while trying to build a token. Token name is required!");
            if (_tokenOptions.Expiration == default(TimeSpan))
                throw new ArgumentException("Exception occured while trying to build a token. Expiration is required!");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.SecretKey));

            _tokenOptions.SigningCredentials = new SigningCredentials(signingKey, string.IsNullOrEmpty(_tokenOptions.SecurityAlgorithm) ? SecurityAlgorithms.HmacSha256 : _tokenOptions.SecurityAlgorithm);

            return _tokenOptions;
        }
    }
}
