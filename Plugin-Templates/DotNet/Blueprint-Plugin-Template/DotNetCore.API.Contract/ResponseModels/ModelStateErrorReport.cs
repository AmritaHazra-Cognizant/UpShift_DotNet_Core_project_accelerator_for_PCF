using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
namespace DotNetCore.API.Contract.ResponseModels
{
    public interface IModelStateErrorReport
    {
        IEnumerable<string> ErrorMessages { get; }
        IEnumerable<string> InvalidFieldNames { get; }
        string Summarization { get; }
        Dictionary<string, List<string>> ErrorDictionary { get; }
    }
    /// <summary>
    ///This is an optional way to uniformly package the response message
    ///for a 400 (BadRequest) condition.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ModelStateErrorReport : IModelStateErrorReport
    {
        [JsonPropertyName("errortiessages")]
        public IEnumerable<string> ErrorMessages { get; set; }
        [JsonPropertyName("invalidfieldNames")]
        public IEnumerable<string> InvalidFieldNames { get; set; }
        [JsonPropertyName("summarization")]
        public string Summarization { get; set; }
        [JsonPropertyName("errorDictionary")]
        public Dictionary<string, List<string>> ErrorDictionary { get; set; }


    }
}
