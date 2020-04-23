using System;

namespace Cythral.CloudFormation.ResourceSecurity
{
    public class ImportValueTag
    {
        public ImportValueTag(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}