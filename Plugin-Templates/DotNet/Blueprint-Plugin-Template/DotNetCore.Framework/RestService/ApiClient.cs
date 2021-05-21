using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;
namespace DotNetCore.Framework.RestService
{
    public class ApiClient : IApiClient
    {
        public ApiClient(ApiConfiguration config, string basePath, X509Certificate2 certificate=null)
        {
            if (string.IsNullOrWhiteSpace(basePath))
                throw new ArgumentException("basePath cannot be empty");
            RestClient = new RestClient(basePath);
            if (certificate != null)
            {
                RestClient.ClientCertificates = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
                RestClient.ClientCertificates.Add(certificate);
            }
            Configuration =
            config;
        }
        public ApiConfiguration Configuration { get; set; }

        public RestClient RestClient { get; set; }
        // Creates and sets up a RestRequest prior to a call.
        private RestRequest PrepareRequest(
        String path, RestSharp.Method method, Dictionary<String, String> queryParams, Object postBody,
        Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
        Dictionary<String, FileParameter> fileParams, Dictionary<String, String> pathParams,
        String contentType)
        {
            var request = new RestRequest(path, method);

            // add path parameter, if any
            foreach (var param in pathParams)
                request.AddParameter(param.Key, param.Value, ParameterType.UrlSegment);
            // add header parameter, if any
            foreach (var param in headerParams)
                request.AddHeader(param.Key, param.Value);
            // add query parameter, if any
            foreach (var param in queryParams)
                request.AddQueryParameter(param.Key, param.Value);
            // add form parameter, if any
            foreach (var param in formParams)
                request.AddParameter(param.Key, param.Value);

            if (postBody != null) // http body (model or byte[]) parameter
            {
                if (postBody.GetType() == typeof(String))
                {
                    request.AddParameter("application/json", postBody, ParameterType.RequestBody);
                }
                else if (postBody.GetType() == typeof(byte[]))
                {
                    request.AddParameter(contentType, postBody, ParameterType.RequestBody);
                }
            }
            return request;
        }
        public Object CallApi(
        String path, RestSharp.Method method, Dictionary<String, String> queryParams, Object postBody,
        Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
        Dictionary<String, FileParameter> fileParams, Dictionary<String, String> pathParams,
        String contentType)
        {
            var request = PrepareRequest(
            path, method, queryParams, postBody, headerParams, formParams, fileParams,
            pathParams, contentType);
            // set timeout
            RestClient.Timeout = Configuration.Timeout;
            // set user agent
            RestClient.UserAgent = Configuration.UserAgent;
            var response = RestClient.Execute(request);
            return (Object)response;
        }
        public async System.Threading.Tasks.Task<Object> CallApiAsync(
       String path, RestSharp.Method method, Dictionary<String, String> queryParams, Object postBody,
       Dictionary<String, String> headerParams, Dictionary<String, String> formParams,
       Dictionary<String, FileParameter> fileParams, Dictionary<String, String> pathParams,
       String contentType)
        {
            var request = PrepareRequest(
            path, method, queryParams, postBody, headerParams, formParams, fileParams,
            pathParams, contentType);
            var response = await RestClient.ExecuteAsync(request);
            return (Object)response;
        }
        public string EscapeString(string str)
        {
            return UrlEncode(str);
        }

        public FileParameter ParameterToFile(string name, Stream stream)
        {
            if (stream is FileStream)
                return FileParameter.Create(name, ReadAsBytes(stream), Path.GetFileName(((FileStream)stream).Name));
            else
                return FileParameter.Create(name, ReadAsBytes(stream), "no_file_name_provided");
        }
        public string ParameterToString(object obj)
        {
            if (obj is DateTime)
                return ((DateTime)obj).ToString(Configuration.DateTimeFormat);
            else if (obj is DateTimeOffset)
                return ((DateTimeOffset)obj).ToString(Configuration.DateTimeFormat);
            else if (obj is IList)
            {
                var flattenedString = new StringBuilder();
                foreach (var param in (IList)obj)
                {
                    if (flattenedString.Length > 0)
                        flattenedString.Append(",");
                    flattenedString.Append(param);
                }
                return flattenedString.ToString();
            }
            else
                return Convert.ToString(obj);
        }
        public object Deserialize(IRestResponse response, Type type)
        {
            IList<Parameter> headers = response.Headers;
            if (type == typeof(byte[])) // return byte array
            {
                return response.RawBytes;
            }
            if (type == typeof(Stream))
            {
                if (headers != null)
                {
                    var filePath = String.IsNullOrEmpty(Configuration.TempFolderPath)
                    ? Path.GetTempPath()
                    : Configuration.TempFolderPath;
                    var regex = new Regex(@"Content-Disposition=.*filename=['""]?([^'""\s]+)['""]?$");
                    foreach (var header in headers)
                    {
                        var match = regex.Match(header.ToString());
                        if (match.Success)
                        {
                            string fileName = filePath + SanitizeFilename(match.Groups[1].Value.Replace("\"", "").Replace("'", ""));
                            File.WriteAllBytes(fileName, response.RawBytes);
                            return new FileStream(fileName, FileMode.Open);
                        }
                    }
                }
                var stream = new MemoryStream(response.RawBytes);
                return stream;

            }
            if (type.Name.StartsWith("System.Nullable`1[[System.DateTime")) // return a datetime object
            {
                return DateTime.Parse(response.Content, null, System.Globalization.DateTimeStyles.RoundtripKind);
            }
            if (type == typeof(String) || type.Name.StartsWith("System.Nullable")) // return primitive type
            {
                return ConvertType(response.Content, type);
            }
            // at this point, it must be a model (json)
            try
            {
                return JsonConvert.DeserializeObject(response.Content, type);
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }
        /// <summary>
        /// Serialize an input(model) into JSON string
        /// </summary>
        /// <param name="obj">Object.</param>
        ///<returns> JSON string.</ returns
        public String Serialize(object obj)
        {
            try

            {
                return obj != null ? JsonConvert.SerializeObject(obj) : null;
            }
            catch (Exception e)
            {
                throw new ApiException(500, e.Message);
            }
        }
        public String SelectHeaderContentType(String[] contentTypes)
        {
            if (contentTypes.Length == 0)
                return null;
            if (contentTypes.Contains("application/json", StringComparer.OrdinalIgnoreCase))
                return "application/json";
            return contentTypes[0]; // use the first content type specified in 'consumes'
        }
        public String SelectHeaderAccept(String[] accepts)
        {
            if (accepts.Length == 0)
                return null;
            if (accepts.Contains("application/json", StringComparer.OrdinalIgnoreCase))
                return "application/json";
            return String.Join(",", accepts);
        }
        public static string Base64Encode(string text)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));
        }
        public static dynamic ConvertType(dynamic source, Type dest)
        {
            return Convert.ChangeType(source, dest);
        }
        public static byte[] ReadAsBytes(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public static string UrlEncode(string input)
        {
            const int maxlength = 32766;

            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (input.Length <= maxlength)
            {
                return Uri.EscapeDataString(input);
            }
            StringBuilder sb = new StringBuilder(input.Length * 2);
            int index = 0;
            while (index < input.Length)
            {
                int length = Math.Min(input.Length - index, maxlength);
                string substring = input.Substring(index, length);
                sb.Append(Uri.EscapeDataString(substring));
                index += substring.Length;
            }
            return sb.ToString();
        }
        public static string SanitizeFilename(string filename)
        {
            Match match = Regex.Match(filename, @".*[/\](.*)$");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return filename;
            }
        }
        public void DisposeResources()
        {
            RestClient = null;
            // GC.Collect();
        }

    }
}
