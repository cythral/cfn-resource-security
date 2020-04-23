using System;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Cythral.CloudFormation.ResourceSecurity
{    
    public class SubTagConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) =>
            type == typeof(SubTag);

        public object ReadYaml(IParser parser, Type type) =>
            throw new Exception("Unsupported Operation");

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var result = (SubTag?)value;
            emitter.Emit(new Scalar(
                null,
                "!Sub",
                $"{result?.Value ?? ""}",
                ScalarStyle.Plain,
                false,
                false
            ));
        }
    }
}