using md5er;

namespace md5er.Tests;

public class MainViewModelTests
{
    [Fact]
    public async Task HashFileAsync_ValidFile_PopulatesHashes()
    {
        var path = Path.GetTempFileName();
        await File.WriteAllBytesAsync(path, Array.Empty<byte>());

        try
        {
            var vm = new MainViewModel();
            await vm.HashFileAsync(path);

            Assert.Equal("d41d8cd98f00b204e9800998ecf8427e", vm.MD5);
            Assert.Equal("da39a3ee5e6b4b0d3255bfef95601890afd80709", vm.SHA1);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task HashFileAsync_ValidFile_IsNotBusyWhenComplete()
    {
        var path = Path.GetTempFileName();
        try
        {
            var vm = new MainViewModel();
            await vm.HashFileAsync(path);

            Assert.False(vm.IsBusy);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public async Task HashFileAsync_Directory_SetsError()
    {
        var vm = new MainViewModel();
        await vm.HashFileAsync(Path.GetTempPath());

        Assert.NotNull(vm.Error);
        Assert.Null(vm.MD5);
    }

    [Fact]
    public async Task HashFileAsync_LockedFile_SetsError()
    {
        var path = Path.GetTempFileName();
        try
        {
            using var hold = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            var vm = new MainViewModel();
            await vm.HashFileAsync(path);

            Assert.NotNull(vm.Error);
            Assert.Null(vm.MD5);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
