using md5er;

namespace md5er.Tests;

public class FileValidatorTests
{
    [Fact]
    public void EnsureHashable_RegularFile_DoesNotThrow()
    {
        var path = Path.GetTempFileName();
        try
        {
            FileValidator.EnsureHashable(path);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void EnsureHashable_Directory_Throws()
    {
        var path = Path.GetTempPath();

        Assert.Throws<UnhashableFileException>(() => FileValidator.EnsureHashable(path));
    }
}
