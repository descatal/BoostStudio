using BoostStudio.Application.Common.Interfaces.Formats.FhmFormat;
using BoostStudio.Domain.Constants;
using Kaitai;

namespace BoostStudio.Infrastructure.Formats.FhmFormat;

public class FhmPacker : IFhmPacker
{

    #region Unpack

    public async Task UnpackAsync(Fhm data, string outputDirectory, CancellationToken cancellationToken)
    {
        if (data.Body?.FileContent is not Fhm.FhmBody fhmBody)
            return;

        await UnpackFhm(fhmBody, outputDirectory, cancellationToken);
    }

    private static async Task UnpackFhm(Fhm.FhmBody fhmBody, string fhmFolder, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(fhmFolder);
        for (int i = 1; i <= fhmBody.Files.Count; i++)
        {
            var fileBody = fhmBody.Files[i - 1];
            switch (fileBody.Body.FileContent)
            {
                // Recursively extract files for nested Fhm
                case Fhm.FhmBody nestedFhmBody:
                    {
                        // For some reason the folder has an asset load type on model fhm, the folder that contains the model assets have this 03 asset load type enum
                        var extension = fileBody.AssetLoadType == Fhm.AssetLoadEnum.Unknown ? $".{FhmAssetExtensions.Unknown}" : string.Empty;
                        var nestedFolder = Path.Combine(fhmFolder, $"{i:D3}{extension}");
                        await UnpackFhm(nestedFhmBody, nestedFolder, cancellationToken);
                        break;
                    }
                case Fhm.GenericBody genericBody:
                    {
                        var fileExtension = fileBody.AssetLoadType switch
                        {
                            Fhm.AssetLoadEnum.Image => FhmAssetExtensions.Image,
                            Fhm.AssetLoadEnum.Model => FhmAssetExtensions.Model,
                            Fhm.AssetLoadEnum.Unknown => FhmAssetExtensions.Unknown,
                            _ => FhmAssetExtensions.Binary
                        };
                        
                        var filePath = Path.Combine(fhmFolder, $"{i:D3}.{fileExtension}");
                        var fileMagic = BitConverter.GetBytes((uint)fileBody.Body.FileMagic);
                        Array.Reverse(fileMagic);
                        byte[] body = [..fileMagic, ..genericBody.Body];
                        await File.WriteAllBytesAsync(filePath, body, cancellationToken);
                        break;
                    }
            }
        }
    }

    #endregion

    #region Pack

    public async Task<Fhm> PackAsync(Stream stream, string inputDirectory, CancellationToken cancellationToken)
    {
        return await PackFhmAsync((uint)stream.Length, stream, inputDirectory, cancellationToken);
    }

    private async Task<Fhm> PackFhmAsync(uint fhmSize, Stream stream, string directory, CancellationToken cancellationToken)
    {
        // Use the passed in stream so that the stream and their child can be disposed later
        var fhm = new Fhm(fhmSize, new KaitaiStream(stream), write: true);
        var fileBody = new Fhm.FileBody(fhmSize, new KaitaiStream(new MemoryStream()), p__parent: fhm, p__root: fhm, write: true);
        var fhmBody = new Fhm.FhmBody(fhmSize, new KaitaiStream(new MemoryStream()), p__parent: fileBody, p__root: fhm, write: true);

        fileBody.FileContent = fhmBody;
        fhm.Body = fileBody;

        var fileSystemEntries = Directory.GetFileSystemEntries(directory, "*", SearchOption.TopDirectoryOnly);

        fhmBody.Files = [];
        foreach (var fileSystemEntry in fileSystemEntries)
        {
            Fhm.FhmFile newFhmFile;
            if (File.GetAttributes(fileSystemEntry).HasFlag(FileAttributes.Directory))
            {
                // Create a new stream for this scope
                using var newStream = new MemoryStream();

                // Recursively pack, and copy the contents of FileBody to a new FhmFile
                var newFhm = await PackFhmAsync((uint)newStream.Length, newStream, fileSystemEntry, cancellationToken);
                newFhmFile = CreateFhmBodyFhmFile(newFhm.Body.FileContent, fhmBody, fhm);
            }
            else
            {
                if (!File.Exists(fileSystemEntry))
                    continue;

                var fileData = await File.ReadAllBytesAsync(fileSystemEntry, cancellationToken);
                newFhmFile = CreateGenericBodyFhmFile(fileData, fhmBody, fhm);
            }
            
            var fileExtension = Path.GetExtension(fileSystemEntry).Replace(".", "").ToLower();
            var assetLoadType = fileExtension switch
            {
                FhmAssetExtensions.Model => Fhm.AssetLoadEnum.Model,
                FhmAssetExtensions.Image => Fhm.AssetLoadEnum.Image,
                FhmAssetExtensions.Unknown => Fhm.AssetLoadEnum.Unknown,
                _ => Fhm.AssetLoadEnum.Normal,
            };
            
            newFhmFile.AssetLoadType = assetLoadType;
            fhmBody.Files.Add(newFhmFile);
        }

        return fhm;
    }

    private Fhm.FhmFile CreateGenericBodyFhmFile(
        byte[] fileData,
        Fhm.FhmBody parentFhmBody,
        Fhm parentFhm)
    {
        // Get the first 4 byte of the file as magic
        var magicByteArray = fileData.AsSpan()[..4].ToArray();
        Array.Reverse(magicByteArray);
        var magic = BitConverter.ToInt32(magicByteArray);

        // Rest of the file content after the first 4 bytes
        var data = fileData.AsSpan()[4..].ToArray();

        // Creates an FhmFile object wrapper
        var newFhmFile = new Fhm.FhmFile(0,new KaitaiStream(new MemoryStream()), p__parent: parentFhmBody, p__root: parentFhm, write: true)
        {
            AssetLoadType = Fhm.AssetLoadEnum.Normal, UnkType = Fhm.UnkEnum.Unknown
        };

        // Creates a FileBody object wrapper, with magic specified
        var newFileBody = new Fhm.FileBody((uint)data.Length, new KaitaiStream(new MemoryStream()), p__parent: newFhmFile, p__root: parentFhm, write: true)
        {
            FileMagic = (Fhm.FileMagicEnums)magic
        };

        // The actual object that holds the data array
        var genericBody = new Fhm.GenericBody(new KaitaiStream(new MemoryStream()), p__parent: newFileBody, p__root: parentFhm, write: true)
        {
            Body = data
        };

        newFileBody.FileContent = genericBody;
        newFhmFile.Body = newFileBody;

        return newFhmFile;
    }

    private Fhm.FhmFile CreateFhmBodyFhmFile(
        KaitaiStruct newFhmBody,
        Fhm.FhmBody parentFhmBody,
        Fhm parentFhm)
    {
        // Creates an FhmFile object wrapper
        var newFhmFile = new Fhm.FhmFile(0, new KaitaiStream(new MemoryStream()), p__parent: parentFhmBody, p__root: parentFhm, write: true)
        {
            AssetLoadType = Fhm.AssetLoadEnum.Normal, UnkType = Fhm.UnkEnum.Unknown
        };

        // Creates a FileBody object wrapper, with magic specified
        var newFileBody = new Fhm.FileBody(0,new KaitaiStream(new MemoryStream()), p__parent: newFhmFile, p__root: parentFhm, write: true)
        {
            FileMagic = Fhm.FileMagicEnums.Fhm, FileContent = newFhmBody
        };

        newFhmFile.Body = newFileBody;
        return newFhmFile;
    }

    #endregion

}
