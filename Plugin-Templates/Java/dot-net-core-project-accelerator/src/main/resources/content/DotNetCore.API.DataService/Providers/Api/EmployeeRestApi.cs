
/*
  * This class is used to show sample API layer example
  * To Do: For Actual project please remove this file 
  * after getting the idea of how to implement.
  */



using DotNetCore.API.DataService.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DotNetCore.Framework.RestService;
using RestSharp;

namespace DotNetCore.API.DataService.Providers.Api
{
    /// <summary>
    /// This Class will behave like proxy class for REST API
    /// </summary>
    public class EmployeeRestApi : IEmployeeRestApi
    {
        private readonly IApiClient _client;
        public EmployeeRestApi(IApiClient client)
        {
            _client = client;
        }
        public async Task<EmployeeResponse> ListEmployeeDataAsync()
        {

            var localVarPath = "/api/users?page=1";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(_client.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;
            //to determine the Content - Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
              };
            String localVarHttpContentType = _client.SelectHeaderContentType(localVarHttpContentTypes);
            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
                };
            String localVarHttpHeaderAccept = _client.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);
            // set "format" to json by default
            // e.g. /pet/{petId}. {format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
          
            IRestResponse localVarResponse = null;
            try
            {
                // make the HTTP request
                localVarResponse = (IRestResponse)await _client.CallApiAsync(localVarPath,
                Method.GET, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);
            }
            finally
            {
                _client.DisposeResources();
            }

            int localVarStatusCode = (int)localVarResponse.StatusCode;
            if (localVarStatusCode == 0 || localVarResponse.ResponseStatus == ResponseStatus.Error || localVarStatusCode != 200)
                throw new ApiException(localVarStatusCode, "Error calling API: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);

            return (EmployeeResponse)_client.Deserialize(localVarResponse, typeof(EmployeeResponse));
        }

        /// <summary>
        /// This is example of POST method
        /// </summary>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public async Task<EmployeeResponse> listPostAsync(dynamic requestBody)
        {
            if (requestBody == null)
                throw new ApiException(400, "Missing required parameter 'requestBody' when calling ConsolidatedReportsOperationsApi->listAssetClass");
            var localVarPath = "/";
            var localVarPathParams = new Dictionary<String, String>();
            var localVarQueryParams = new Dictionary<String, String>();
            var localVarHeaderParams = new Dictionary<String, String>(_client.Configuration.DefaultHeader);
            var localVarFormParams = new Dictionary<String, String>();
            var localVarFileParams = new Dictionary<String, FileParameter>();
            Object localVarPostBody = null;
            //to determine the Content - Type header
            String[] localVarHttpContentTypes = new String[] {
                "application/json"
              };
            String localVarHttpContentType = _client.SelectHeaderContentType(localVarHttpContentTypes);
            // to determine the Accept header
            String[] localVarHttpHeaderAccepts = new String[] {
                "application/json"
                };
            String localVarHttpHeaderAccept = _client.SelectHeaderAccept(localVarHttpHeaderAccepts);
            if (localVarHttpHeaderAccept != null)
                localVarHeaderParams.Add("Accept", localVarHttpHeaderAccept);
            // set "format" to json by default
            // e.g. /pet/{petId}. {format} becomes /pet/{petId}.json
            localVarPathParams.Add("format", "json");
            if (requestBody.GetType() != typeof(byte[]))
            {
                localVarPostBody = _client.Serialize(requestBody); // http body (model) parameter
            }
            else
            {
                localVarPostBody = requestBody; // byte array
            }
            IRestResponse localVarResponse = null;
            try
            {
                // make the HTTP request
                localVarResponse = (IRestResponse)await _client.CallApiAsync(localVarPath,
                Method.POST, localVarQueryParams, localVarPostBody, localVarHeaderParams, localVarFormParams, localVarFileParams,
                localVarPathParams, localVarHttpContentType);
            }
            finally
            {
                _client.DisposeResources();
            }
           
            int localVarStatusCode = (int)localVarResponse.StatusCode;
            if (localVarStatusCode ==0 || localVarResponse.ResponseStatus == ResponseStatus.Error || localVarStatusCode != 200)
                throw new ApiException(localVarStatusCode, "Error calling API: " + localVarResponse.ErrorMessage, localVarResponse.ErrorMessage);
           
            return (EmployeeResponse)_client.Deserialize(localVarResponse, typeof(EmployeeResponse));
        }

    }
}
