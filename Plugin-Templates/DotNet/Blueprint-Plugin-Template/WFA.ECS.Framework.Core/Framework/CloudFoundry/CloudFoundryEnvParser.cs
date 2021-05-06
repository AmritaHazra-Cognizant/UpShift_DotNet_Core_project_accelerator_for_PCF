using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WFA.ECS.Framework.Core.Framework.CloudFoundry
{
    /// <summary>
    /// //Class containing static fileds for interacting with environment variable by PCF
    /// </summary>

    public static class CloudFoundryEnvParser
    {
        public static bool isRuningOnCF = false;
        /// <summary>
        /// LOcal path to UPS json
        /// </summary>
        public static string _localCloudFoundryJson = string.Empty;
        /// <summary>
        /// Applciation Info
        /// </summary>
        public static CloudApplicationInfo ApplicationInfo { get; set; }
        /// <summary>
        /// services bound to this application
        /// </summary>
        public static List<CloudService> Services { get; set; }

        static CloudFoundryEnvParser()
        {
            Services = new List<CloudService>();
            if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(CloudConstants.APPLICATION_ENV_VARIALBLE_NAME)))
            {
                isRuningOnCF = true;
            }
            else
            {
                if (File.Exists("cloudfoundry.local.json"))
                {
                    //Read Mock Json 
                    _localCloudFoundryJson = File.ReadAllText("cloudfoundry.local.json");
                }
            }
            ParseApplication();
            ParseService();
        }
        private static void ParseApplication()
        {
            ApplicationInfo = new CloudApplicationInfo();
            dynamic jsonApplication = null;
            if (isRuningOnCF)
            {
                jsonApplication = JObject.Parse(GetEnvVariable(CloudConstants.APPLICATION_ENV_VARIALBLE_NAME));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_localCloudFoundryJson))
                {
                    jsonApplication = JObject.Parse(_localCloudFoundryJson)[CloudConstants.APPLICATION_ENV_VARIALBLE_NAME].ToObject<JObject>();
                }
            }
            //service
            if (jsonApplication != null)
            {
                FillApplicationInfo(jsonApplication);
            }
        }
        private static void FillApplicationInfo(dynamic jsonApplication)
        {
            ApplicationInfo.AppName = jsonApplication.name;
            ApplicationInfo.ID = jsonApplication.application_id;
            ApplicationInfo.AppVersion = jsonApplication.application_version;
            ApplicationInfo.Port =isRuningOnCF?  GetEnvVariable(CloudConstants.PORT_ENV_VARIABLE_NAME):"0";
            ApplicationInfo.Index = isRuningOnCF ? int.Parse(GetEnvVariable(CloudConstants.INSTANCE_INDEX_ENV_VARIABLE_NAME)) : 0; ;
            ApplicationInfo.IP = isRuningOnCF ?  GetEnvVariable(CloudConstants.IP_ENV_VARIABLE_NAME) : "localhost";
            //CF limits & space details
            ApplicationInfo.DiskLimit = jsonApplication.limits != null ? (jsonApplication.limits.disk ?? 0) : 0;
            ApplicationInfo.MemoryLimit= jsonApplication.limits != null ? (jsonApplication.limits.mem ?? 0) : 0;
            ApplicationInfo.SpaceName = jsonApplication.space_name;
            ApplicationInfo.SpaceID = jsonApplication.space_id;
            //uri
            ApplicationInfo.Uris = new List<string>();
            if (jsonApplication.appplication_uris != null)
            {
                foreach (string thisUri in jsonApplication.appplication_uris)
                {
                    ApplicationInfo.Uris.Add(thisUri);
                }
            }

        }

        private static string GetEnvVariable(string key)
        {
            string value = string.Empty;
            if (isRuningOnCF)
            {
                value = Environment.GetEnvironmentVariable(key);
                value = string.IsNullOrWhiteSpace(key) ? string.Empty : value;
            }
            else
            {
                var jsonEnvVariables = JObject.Parse(_localCloudFoundryJson)[CloudConstants.APPLICATION_ENV_VARIALBLE_NAME].ToArray();
                if (jsonEnvVariables != null && jsonEnvVariables.Length > 0)
                {
                    var variable = jsonEnvVariables.FirstOrDefault(env => env["key"].ToString() == key);
                    if (variable != null)
                    {
                        value = variable["value"].ToString();
                    }
                }
            }
            return value;

        }

        private static void ParseService()
        {
            JObject jsonServices = null;
            if (isRuningOnCF)
            {
                string serviceVariable = GetEnvVariable(CloudConstants.BOUND_SERVICES_ENV_VARIABLE_NAME);
                if (!string.IsNullOrWhiteSpace(serviceVariable))
                {
                    jsonServices = JObject.Parse(serviceVariable);
                }

            }
            else
            {
                if (!string.IsNullOrWhiteSpace(_localCloudFoundryJson))
                {
                    jsonServices = JObject.Parse(_localCloudFoundryJson)[CloudConstants.BOUND_SERVICES_ENV_VARIABLE_NAME].ToObject<JObject>();

                }
            }
            //services
            if (jsonServices != null)
            {
                FillJsonServices(jsonServices);
            }
        }

        private static void FillJsonServices(JObject jsonServices)
        {
            //service level
            foreach (var j in jsonServices["user-provided"] )
            {
                //parse the service
                var thisService = new CloudService();
                dynamic thisJsonService = j;
                thisService.ServiceName = thisJsonService.name;
                thisService.BindingName = thisJsonService.binding_name;
                thisService.Label = thisJsonService.label;
                thisService.Plan = thisJsonService.plan;

                //credential
                JObject credentials = thisJsonService.credentials;
                if (credentials != null)
                {
                    thisService.Credentials = new Dictionary<string, string>();
                    foreach (KeyValuePair<string,JToken> thisProperty in credentials)
                    {
                        if (thisProperty.Value.GetType() == typeof(JObject))
                        {
                            var obj = (JObject)thisProperty.Value;
                            if (obj.Count > 0 && obj.First != null)
                            {
                                thisService.Credentials.Add(((JProperty)obj.First).Name, ((JProperty)obj.First).Value.ToString());
                            }
                            else
                            {
                                thisService.Credentials.Add(thisProperty.Key, thisProperty.Value.Value<string>());
                            }
                        }
                    }
                }
                //tags
                if(thisJsonService.tags!=null)
                {
                    thisService.Tags = new List<string>();
                    foreach (string  s in thisJsonService.tags)
                    {
                        thisService.Tags.Add(s);

                    }
                }
                Services.Add(thisService);

            }
        }

        public static CloudService GetServiceByName(string name)
        {
            return Services.First(s => s.ServiceName == name);
        }
        public static CloudService GetServiceByBindingName(string name)
        {
            return Services.First(s => s.BindingName == name);
        }
        public static bool isRuningOnCloudFoundry(string name)
        {
            return isRuningOnCF;
        }
    }
}
