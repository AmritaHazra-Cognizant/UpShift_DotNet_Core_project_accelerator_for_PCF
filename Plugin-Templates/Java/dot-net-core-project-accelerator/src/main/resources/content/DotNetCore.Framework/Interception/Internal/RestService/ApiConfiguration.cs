﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetCore.Framework.RestService
{

    /// <summary>
    // Represents a set of configuration settings
    /// </summary>
    public class ApiConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the Configuration class with different settings
        /// </summary>
        /// <param name = "defaultHeader" > Dictionary of default HTTP header</param>
        /// <param name="username">Username</param>
        ///<param name="password"> Password</param>
        /// <param name="accessToken">accessToken</param>
        /// <param name="apiKey">Dictionary of API key</param>
        /// <param name="apiKeyPrefix">Dictionary of API key prefix</param>
        /// <param name="tempFolderPath">Temp folder path</param>
        /// <param name = "dateTimeFormat" > DateTime format string</param>
        ///<param name="timeout"> HTTP connection timeout (in milliseconds)</param>
        /// <param name="userAgent">HTTP user agent</param>
        public ApiConfiguration(
        Dictionary<string, string> defaultHeader = null,
        string username = null,
        string password = null,
        string accessToken = null,
        Dictionary<string, string> apiKey = null,
        Dictionary<string, string> apiKeyPrefix = null,
        string tempFolderPath = null,
        string dateTimeFormat = null,
        int timeout = 100000,
        string userAgent = "DotNetCore.API"
        )
        {
            Username = username;
            Password = password;
            AccessToken = accessToken;
            UserAgent = userAgent;
            if (defaultHeader != null)
                DefaultHeader = defaultHeader;
            if (apiKey != null)
                ApiKey = apiKey;
            if (apiKeyPrefix != null)
                ApiKeyPrefix = apiKeyPrefix;
            TempFolderPath = tempFolderPath;
            DateTimeFormat = dateTimeFormat;
            Timeout = timeout;
        }
        /// <summary>
        /// Version of the package.
        /// </summary>
        /// <value > Version of the package. </value>
        public const string Version = "1.0.0";
        /// <summary>
        /// Gets or sets the default Configuration.
        /// </summary>
        ///<value> Configuration.</value>
        /// <summary>
        /// Gets or sets the HTTP timeout (milliseconds) of ApiClient.Default to 100000 milliseconds.
        /// </summary>
        /// <value>Timeout.</value>
        public int Timeout
        {
            get; set;
        }
        private Dictionary<String, String> _defaultHeaderMap = new Dictionary<String, String>();
        /// <summary>
        /// Gets or sets the default header.
        /// </summary>
        public Dictionary<String, String> DefaultHeader
        {
            get { return _defaultHeaderMap; }
            set
            {
                _defaultHeaderMap = value;

            }
        }
        /// <summary>
        /// Add default header.
        /// </summary>
        /// <param name="key">Header field name.</param>
        /// <param name="value">Header field value.</param>
        /// <returns></returns
        public void AddDefaultHeader(string key, string value)
        {
            _defaultHeaderMap.Add(key, value);
        }
        /// <summary>
        /// Gets or sets the HTTP user agent.
        /// </summary>
        /// <value>Http user agent.</value>
        public String UserAgent { get; set; }
        /// <summary>
        // Gets or sets the username (HTTP basic authentication).
        /// </summary>
        /// <value>The username.</value>
        public String Username { get; set; }

        /// <summary>
        /// Gets or sets the password(HTTP basic authentication).
        /// </summary>
        /// <value>The password.</value>
        public String Password { get; set; }
        /// <summary>
        /// Gets or sets the access token for OAuth2 authentication.
        ///token for OAuth2 authentication.
        /// </summary>
        /// <value>The access token.</value>
        public String AccessToken { get; set; }
        /// <summary>
        /// Gets or sets the API key based on the authentication name.
        ///</summary>
        /// <value> The API key.</value>
        private Dictionary<String, String> ApiKey = new Dictionary<String, String>();
        /// <summary>
        /// Gets or sets the prefix(e.g.Token) of the API key based on the authentication name.
        /// </summary>
        /// <value> The prefix of the API key.</value>
        private Dictionary<String, String> ApiKeyPrefix = new Dictionary<String, String>();
        /// <summary>
        ///Get the API key with prefix.
        /// </summary>
        /// <param name="apiKeyIdentifier">API key identifier (authentication scheme). </param>
        /// <returns >API key with prefix.</returns>
        public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        {
            var apiKeyValue = "";
            ApiKey.TryGetValue(apiKeyIdentifier, out apiKeyValue);
            var apiKeyPrefix = "";
            if (ApiKeyPrefix.TryGetValue(apiKeyIdentifier, out apiKeyPrefix))
                return apiKeyPrefix + " " + apiKeyValue;
            else
                return apiKeyValue;
        }
        private string _tempFolderPath = Path.GetTempPath();
        /// <summary>
        /// Gets or sets the temporary folder path to store the files downloaded from the server.
        ///</summary>
        /// <value>Folder path.</value>
        public String TempFolderPath
        {
            get { return _tempFolderPath; }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    _tempFolderPath = value;
                    return;
                }
                // create the directory if it does not exist
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);
                // check if the path contains directory separator at the end
                if (value[value.Length - 1] == Path.DirectorySeparatorChar)
                    _tempFolderPath = value;
                else
                    _tempFolderPath = value + Path.DirectorySeparatorChar;
            }
        }
        private const string ISO8601_DATETIME_FORMAT = "0";
        private string _dateTimeFormat = ISO8601_DATETIME_FORMAT;
        ///<summary>
        /// Gets or sets the the date time format used when serializing in the ApiClient
        /// By default, it's set to ISO 8601 - "0", for others see:
        /// https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs. 110).aspx
        /// and https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs. 110).aspx
        /// No validation is done to ensure that the string you're providing is valid
        /// </summary>
        /// <value> The DateTimeFormat string</value>
        public String DateTimeFormat
        {
            get
            {
                return _dateTimeFormat;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    // Never allow a blank or null string, go back to the default
                    _dateTimeFormat = ISO8601_DATETIME_FORMAT;
                    return;
                }
                // Caution, no validation when you choose date time format other than ISO 8601
                // Take a look at the above links
                _dateTimeFormat = value;
            }
        }

        /// <summary>
        /// Returns a string with essential information for debugging.I
        /// </summary>
        public static String ToDebugReport()
        {
            String report = "C# SDK (DotNetCore.API) Debug Report:\n";
            report += "    OS: " + Environment.OSVersion + "\n";
            report += "      .NET Framework Version: " + Assembly
            .GetExecutingAssembly()
            .GetReferencedAssemblies()
            .First(x => x.Name == "System.Core").Version.ToString() + "\n";
            report += "   Version of the API: 1.0\n";
            report += "     SDK Package Version: 1.0.0\n";
            return report;
        }

    }
}
