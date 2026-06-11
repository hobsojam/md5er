using md5er;

namespace md5er.Tests;

public class HashServiceTests
{
    private static Stream EmptyStream() => new MemoryStream(Array.Empty<byte>());

    [Fact]
    public void Compute_EmptyStream_ReturnsCorrectMD5()
    {
        var result = HashService.Compute(EmptyStream());

        Assert.Equal("d41d8cd98f00b204e9800998ecf8427e", result.MD5);
    }

    [Fact]
    public void Compute_EmptyStream_ReturnsCorrectSHA1()
    {
        var result = HashService.Compute(EmptyStream());

        Assert.Equal("da39a3ee5e6b4b0d3255bfef95601890afd80709", result.SHA1);
    }

    [Fact]
    public void Compute_EmptyStream_ReturnsCorrectSHA256()
    {
        var result = HashService.Compute(EmptyStream());

        Assert.Equal("e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855", result.SHA256);
    }

    [Fact]
    public void Compute_EmptyStream_ReturnsCorrectSHA512()
    {
        var result = HashService.Compute(EmptyStream());

        Assert.Equal("cf83e1357eefb8bdf1542850d66d8007d620e4050b5715dc83f4a921d36ce9ce47d0d13c5d85f2b0ff8318d2877eec2f63b931bd47417a81a538327af927da3e", result.SHA512);
    }

    [Fact]
    public void Compute_LargerThanBufferStream_MatchesDirectHash()
    {
        var data = new byte[200_000];
        var result = HashService.Compute(new MemoryStream(data));

        Assert.Equal(Hex(System.Security.Cryptography.MD5.HashData(data)),    result.MD5);
        Assert.Equal(Hex(System.Security.Cryptography.SHA1.HashData(data)),   result.SHA1);
        Assert.Equal(Hex(System.Security.Cryptography.SHA256.HashData(data)), result.SHA256);
        Assert.Equal(Hex(System.Security.Cryptography.SHA512.HashData(data)), result.SHA512);
    }

    private static string Hex(byte[] bytes) =>
        Convert.ToHexString(bytes).ToLowerInvariant();
}
