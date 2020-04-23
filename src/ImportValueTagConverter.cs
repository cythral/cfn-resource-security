using System;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Cythral.CloudFormation.ResourceSecurity
{    
    public class ImportValueTagConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type) =>
            type == typeof(ImportValueTag);

        public object ReadYaml(IParser parser, Type type) =>
            throw new Exception("Unsupported Operation");

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            var result = (ImportValueTag?)value;
            emitter.Emit(new Scalar(
                null,
                "!ImportValue",
                $"{result?.Value ?? ""}",
                ScalarStyle.Plain,
                false,
                false
            ));
        }
    }
}