using BoostStudio.Application.Exvs.Tbl.Commands;

namespace BoostStudio.Application.FunctionalTests.Tbl;

using static Testing;

public class SerializationTests : BaseTestFixture
{
    [Test]
    public async Task Tbl_SerializationOperations_ShouldReturnSameBinary()
    {
        var directory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "..",
            "..",
            "..",
            "Tbl",
            "Binary"
        );
        var originalBinaries = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);

        foreach (var binaryPaths in originalBinaries)
        {
            var file = await File.ReadAllBytesAsync(binaryPaths);
            var deserialize = new DeserializeTblCommand(File: file);
            var deserializedMetadata = await SendAsync(deserialize);

            var serialize = new SerializeTblCommand(
                deserializedMetadata.CumulativeFileInfoCount,
                deserializedMetadata.FileMetadata,
                deserializedMetadata.PathOrder
            );
            var serializedFile = await SendAsync(serialize);
            serializedFile.Should().NotBeNullOrEmpty();
            serializedFile.Should().BeEquivalentTo(file);
        }
    }
}
