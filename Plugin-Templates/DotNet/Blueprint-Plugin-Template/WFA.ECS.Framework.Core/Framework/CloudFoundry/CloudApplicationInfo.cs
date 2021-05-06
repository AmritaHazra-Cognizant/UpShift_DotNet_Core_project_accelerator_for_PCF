using System;
using System.Collections.Generic;
using System.Text;

namespace WFA.ECS.Framework.Core.Framework.CloudFoundry
{
   public class CloudApplicationInfo
    {

        /// <summary>
        /// Application Name provided by developer during push
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// Application ID - GUID provided by PCF 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The instance id of the app 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// IP address of the cell runing the application 
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// The port number of cell
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// Routes being driven to the apps 
        /// </summary>
        public List<string> Uris { get; set; }

        /// <summary>
        /// Cloud Foundry assigned app version
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Memory limit assigned to Instance
        /// </summary>
        public int MemoryLimit { get; set; }

        /// <summary>
        /// Disk limit assigned to Instance
        /// </summary>
        public int DiskLimit { get; set; }

        /// <summary>
        /// Name of the Space
        /// </summary>
        public int SpaceName { get; set; }

        /// <summary>
        /// GUID from the space
        /// </summary>
        public int SpaceID { get; set; }

    }
}
