using System.Windows;
using System.Windows.Forms;
using Application      = System.Windows.Application;
using Clipboard        = System.Windows.Clipboard;
using DataFormats      = System.Windows.DataFormats;
using DragDropEffects  = System.Windows.DragDropEffects;
using DragEventArgs    = System.Windows.DragEventArgs;

namespace md5er;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();
    private readonly NotifyIcon _trayIcon;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
        _trayIcon = BuildTrayIcon();
    }

    private NotifyIcon BuildTrayIcon()
    {
        var iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/md5er.ico"))!.Stream;

        var menu = new ContextMenuStrip();
        menu.Items.Add("Open", null, (_, _) => Restore());
        menu.Items.Add("Exit", null, (_, _) => { _trayIcon.Visible = false; Application.Current.Shutdown(); });

        var icon = new NotifyIcon
        {
            Icon = new System.Drawing.Icon(iconStream),
            Text = "md5er",
            Visible = false,
            ContextMenuStrip = menu
        };
        icon.DoubleClick += (_, _) => Restore();
        return icon;
    }

    private void Restore()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
        _trayIcon.Visible = false;
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            Hide();
            _trayIcon.Visible = true;
        }
        base.OnStateChanged(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        _trayIcon.Dispose();
        base.OnClosed(e);
    }

    private async void Window_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0)
            return;

        await _vm.HashFileAsync(files[0]);
    }

    private void Window_DragOver(object sender, DragEventArgs e)
    {
        e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
            ? DragDropEffects.Copy
            : DragDropEffects.None;
        e.Handled = true;
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string hash })
            Clipboard.SetText(hash);
    }
}
