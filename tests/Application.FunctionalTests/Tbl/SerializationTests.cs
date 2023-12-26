using BoostStudio.Application.Formats.TblFormat.Commands;
using static BoostStudio.Application.FunctionalTests.Testing;

namespace BoostStudio.Application.FunctionalTests.Tbl;

public class SerializationTests : BaseTestFixture
{
    [Test]
    public async Task Tbl_SerializationOperations_ShouldReturnSameBinary()
    {
        var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Tbl", "Binary");
        var originalBinaries = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);

        foreach (var binaryPaths in originalBinaries)
        {
            var file = await File.ReadAllBytesAsync(binaryPaths);
            var deserialize = new DeserializeTbl(File: file);
            var deserializedMetadata = await SendAsync(deserialize);

            var serialize = new SerializeTbl(TblMetadata: deserializedMetadata);
            var serializedFile = await SendAsync(serialize);
            serializedFile.Should().NotBeNullOrEmpty();
            serializedFile.Should().BeEquivalentTo(file);
        }
    }
}
