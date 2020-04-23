using System;

namespace Cythral.CloudFormation.ResourceSecurity
{
    public class SubTag
    {
        public SubTag(string value)
        {
            this.Value = value;
        }

        public string Value { get; set; }
    }
}