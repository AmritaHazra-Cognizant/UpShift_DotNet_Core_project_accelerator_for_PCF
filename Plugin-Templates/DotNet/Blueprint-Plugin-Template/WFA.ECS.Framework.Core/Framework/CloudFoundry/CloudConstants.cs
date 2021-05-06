using System;
using System.Collections.Generic;
using System.Text;

namespace WFA.ECS.Framework.Core.Framework.CloudFoundry
{
    public static class CloudConstants
    {
        public static readonly string PORT_ENV_VARIABLE_NAME = "PORT";
        public static readonly string IP_ENV_VARIABLE_NAME = "CF_INSTANCE_IP";
        public static readonly string INSTANCE_GUID_ENV_VARIABLE_NAME = "INSTANCE_GUID";
        public static readonly string INSTANCE_INDEX_ENV_VARIABLE_NAME = "CF_INSTNACE_INDEX";
        public static readonly string BOUND_SERVICES_ENV_VARIABLE_NAME = "VCAP_SERVICES";
        public static readonly string NOT_ON_CLOUD_FOUNDRY_MESSAGE = "Not Runing in Cloud Foundry";
        public static readonly string APPLICATION_ENV_VARIALBLE_NAME = "VCAP_APPLICATION";
        public static readonly string ENV_VARIABLES = "ENV_VARIABLES";
    }
}
