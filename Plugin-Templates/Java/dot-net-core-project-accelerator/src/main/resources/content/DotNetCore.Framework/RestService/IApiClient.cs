using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Framework.RestService
{
    public interface IApiClient
    {
        ApiConfiguration Configuration { get; set; }
        RestClient RestClient { get; set; }
        object CallApi(string path, Method method, Dictionary<string, string> queryParams,
        object postBody, Dictionary<string, string> headerParams,
        Dictionary<string, string> formParams, Dictionary<string, FileParameter> fileParams,
        Dictionary<string, string> pathParams, string contentType);
        Task<object> CallApiAsync(string path, Method method, Dictionary<string, string> queryParams,
object postBody, Dictionary<string, string> headerParams, Dictionary<string, string> formParams,
Dictionary<string, FileParameter> fileParams, Dictionary<string, string> pathParams, string contentType);
 object Deserialize(IRestResponse response, Type type);
        string EscapeString(string str);
        FileParameter ParameterToFile(string name, Stream stream);
        string ParameterToString(object obj);
        string SelectHeaderAccept(string[] accepts);
        string SelectHeaderContentType(string[] contentTypes);
        string Serialize(object obj);
        void DisposeResources();
    }
}
