using System.IO;
using System.Security.Cryptography;

namespace md5er;

public record HashResult(string MD5, string SHA1, string SHA256, string SHA512);

public class HashService
{
    public HashResult Compute(Stream stream)
    {
        using var md5    = IncrementalHash.CreateHash(HashAlgorithmName.MD5);
        using var sha1   = IncrementalHash.CreateHash(HashAlgorithmName.SHA1);
        using var sha256 = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        using var sha512 = IncrementalHash.CreateHash(HashAlgorithmName.SHA512);

        byte[] buffer = new byte[81920];
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            md5.AppendData(buffer, 0, bytesRead);
            sha1.AppendData(buffer, 0, bytesRead);
            sha256.AppendData(buffer, 0, bytesRead);
            sha512.AppendData(buffer, 0, bytesRead);
        }

        return new HashResult(
            MD5:    Hex(md5.GetHashAndReset()),
            SHA1:   Hex(sha1.GetHashAndReset()),
            SHA256: Hex(sha256.GetHashAndReset()),
            SHA512: Hex(sha512.GetHashAndReset())
        );
    }

    private static string Hex(byte[] bytes) =>
        Convert.ToHexString(bytes).ToLowerInvariant();
}
