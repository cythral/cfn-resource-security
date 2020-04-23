using System.Collections.Generic;
using System;

namespace Cythral.CloudFormation.ResourceSecurity
{
    public class ResourceEntry
    {
        public string Name { get; set; } = "";

        public string Export { get; set; } = "";

        public List<string> Grantees { get; set; } = new List<string>();
    }
}