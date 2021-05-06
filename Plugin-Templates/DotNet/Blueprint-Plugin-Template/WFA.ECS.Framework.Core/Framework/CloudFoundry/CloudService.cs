using System;
using System.Collections.Generic;
using System.Text;

namespace WFA.ECS.Framework.Core.Framework.CloudFoundry
{
    public class CloudService
    {
        public string ServiceName { get; set; }
        public string BindingName { get; set; }
        public string Label { get; set; }
        public List<string>  Tags { get; set; }
        public string Plan { get; set; }
        public Dictionary<string,string> Credentials { get; set; }
    }
}
