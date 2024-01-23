using Filio.Api.Domains;
using Filio.Api.Exceptions;

namespace Tests;

public class FileDomainTests
{
    [Theory]
    [InlineData(null)]//Null
    [InlineData("")]//Empty
    [InlineData("mb")]//Too short
    [InlineData("MYBUCKET")]//Full UPPER case
    [InlineData("Mybucket")]//Contains UPPER case character
    [InlineData("192.168.1.1")]//Can't like an IP Address
    [InlineData("xn--mybucket")]//Bad Prefix
    [InlineData("sthree-mybucket")]//Bad Prefix
    [InlineData("sthree-configurator.mybucket")]//BadPrefix
    [InlineData("mybucket-s3alias")]//Bad suffix
    [InlineData("mybucket--ol-s3")]//Bad Suffix
    [InlineData("mybucket.first")]//Dot Contains
    [InlineData("VsMC6AVLfXC0syNyD6rLUaVCJBZMDz9dlKMhWfAwCPiXVAyWbxU32KnlABRZADFd")]//Too long (64) length
    public void CreatingFile_WithInvalidBucketName_ShouldException(string bucketName)
    {
        Assert.Throws<DomainException>(() => new FileDomain(bucketName: bucketName,
                                    sizeInByte: 10,
                                    extension: ".dummy",
                                    originalName: "dummy.dummy",
                                    FileDomainType.Video,
                                    imageBlurhash: null));

    }
}