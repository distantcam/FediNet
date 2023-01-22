using System.Reflection;
using System.Text.Json;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FediNet.ActivityStreams.Tests;

#nullable disable

public class SerializationTests
{
    [Theory]
    [MemberData(nameof(GetExamples))]
    public void RoundTripExamples(Example example)
    {
        var obj = JsonSerializer.Deserialize(example.Json, ActivityStreamsJsonContext.Default.IObjectOrLink);
        var outJson = JsonSerializer.Serialize(obj, ActivityStreamsJsonContext.Default.IObjectOrLink);

        var expected = JToken.Parse(example.Json);
        var actual = JToken.Parse(outJson);

        actual.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> GetExamples()
    {
        var assembly = Assembly.GetExecutingAssembly();

        var examples = assembly.GetManifestResourceNames()
            .Where(n => n.StartsWith("FediNet.ActivityStreams.Tests.Examples."));

        foreach (var example in examples)
        {
            var json = assembly.GetManifestResourceStream(example).ReadToEnd();
            yield return new[] { new Example { Name = example, Json = json } };
        }
    }

    [Serializable]
    public class Example : IXunitSerializable
    {
        public string Json { get; set; }
        public string Name { get; set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Json = info.GetValue<string>("json");
            Name = info.GetValue<string>("name");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("json", Json);
            info.AddValue("name", Name);
        }

        public override string ToString() => string.Join('.', Name.Split('.')[^2..]);
    }
}
