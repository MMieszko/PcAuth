using System;
using Microsoft.IdentityModel.Tokens;
using PortaCapena.Authentication.NetCore.Configuration;

namespace PortaCapena.Authentication.NetCore.Abstraction
{
    public interface ITokenOptionsBuilder
    {
        /// <summary>
        /// Sets the token name which will be passed in http requests header
        /// </summary>
        /// <param name="tokenName">Name of token</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetTokenName(string tokenName);
        /// <summary>
        /// Sets the expiration time of token. Token will be valid for given time.
        /// </summary>
        /// <param name="expiration">Validation time</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetExpiration(TimeSpan expiration);
        /// <summary>
        /// Sets the value of secret key stored in server side.
        /// </summary>
        /// <param name="key">Secret key</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetSecretKey(string key);
        /// <summary>
        /// Sets the algorithm to hash the key. By default uses <see cref="SecurityAlgorithms.HmacSha256"/>
        /// </summary>
        /// <param name="key">Algorithm</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetAlgorithm(string algorithm);
        /// <summary>
        /// Enable token to autorefersh each request. By default value is set to true.
        /// </summary>
        /// <param name="key">Algorithm</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetAutoRefresh(bool value);
        /// <summary>
        /// Sets the name of exchanged token name applied in response header.
        /// By default header is named by "X-Access-Token"
        /// </summary>
        /// <param name="name">Name of the exchange token</param>
        /// <returns>Self</returns>
        ITokenOptionsBuilder SetExchangeTokenName(string name);
        /// <summary>
        /// Ends building token options
        /// </summary>
        /// <returns><see cref="TokenOptions"/> instance</returns>
        TokenOptions Build();
    }
}
