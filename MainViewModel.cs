using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace md5er;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly HashService _hashService = new();

    private string? _fileName;
    private string? _md5;
    private string? _sha1;
    private string? _sha256;
    private string? _sha512;
    private string? _error;
    private bool _isBusy;

    public string? FileName  { get => _fileName;  private set { if (Set(ref _fileName,  value)) { Notify(nameof(HasFile), nameof(ShowDropHint)); } } }
    public string? MD5       { get => _md5;        private set { if (Set(ref _md5,       value)) { Notify(nameof(HasResult)); } } }
    public string? SHA1      { get => _sha1;       private set => Set(ref _sha1,   value); }
    public string? SHA256    { get => _sha256;     private set => Set(ref _sha256,  value); }
    public string? SHA512    { get => _sha512;     private set => Set(ref _sha512,  value); }
    public string? Error     { get => _error;      private set { if (Set(ref _error,     value)) { Notify(nameof(HasError)); } } }
    public bool    IsBusy    { get => _isBusy;     private set => Set(ref _isBusy,  value); }

    public bool HasFile      => FileName != null;
    public bool ShowDropHint => !HasFile;
    public bool HasResult    => MD5 != null;
    public bool HasError     => Error != null;

    public async Task HashFileAsync(string path)
    {
        IsBusy = true;
        FileName = Path.GetFileName(path);
        MD5 = SHA1 = SHA256 = SHA512 = Error = null;

        try
        {
            FileValidator.EnsureHashable(path);

            var result = await Task.Run(() =>
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                return _hashService.Compute(stream);
            });

            MD5    = result.MD5;
            SHA1   = result.SHA1;
            SHA256 = result.SHA256;
            SHA512 = result.SHA512;
        }
        catch (UnhashableFileException ex)     { Error = ex.Message; MD5 = null; }
        catch (IOException ex)                 { Error = ex.Message; MD5 = null; }
        catch (UnauthorizedAccessException ex) { Error = ex.Message; MD5 = null; }
        finally
        {
            IsBusy = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        return true;
    }

    private void Notify(params string[] names)
    {
        foreach (var name in names)
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
