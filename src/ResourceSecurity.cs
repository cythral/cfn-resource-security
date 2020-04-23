using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

using YamlDotNet.Serialization;

namespace Cythral.CloudFormation.ResourceSecurity
{
    class ResourceSecurity
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Invalid input.  Usage: <command> <inputFile> <outputFile>");
                Environment.Exit(1);
            }

            var inputFile = args[0];
            var outputFile = args[1];
            var entries = GetEntries(inputFile);

            if (entries == null || entries.Count == 0) 
            {
                Console.Error.WriteLine("Could not read metadata file.");
                Environment.Exit(1);
            };

            var serializer = new SerializerBuilder()
            .WithTagMapping("!Sub", typeof(SubTag))
            .WithTagMapping("!ImportValue", typeof(ImportValueTag))
            .WithTypeConverter(new SubTagConverter())
            .WithTypeConverter(new ImportValueTagConverter())
            .Build();
            
            var template = serializer.Serialize(new 
            {
                Description = "Permissions needed for agent accounts to invoke custom resources in this account.",
                Resources = GetResources(entries),
            });

            File.WriteAllText(outputFile, template);
        }

        static List<ResourceEntry>? GetEntries(string filename) 
        {
            var contents = File.ReadAllText(filename);
            var deserializer = new Deserializer();
            return deserializer.Deserialize<List<ResourceEntry>>(contents);
        }


        static Dictionary<string, object> GetResources(List<ResourceEntry> entries)
        {
            var result = new Dictionary<string, object>();

            foreach (var entry in entries)
            {
                for (var i = 0; i < entry.Grantees.Count; i++)
                {
                    var grantee = entry.Grantees[i];

                    result.Add($"{entry.Name}Permission{i}", new {
                        Type = "AWS::Lambda::Permission",
                        Properties = new {
                            FunctionName = new ImportValueTag(entry.Export),
                            Principal = new ImportValueTag(grantee),
                            Action = "lambda:InvokeFunction"
                        }
                    });
                }
            }

            return result;
        }
    }
}
