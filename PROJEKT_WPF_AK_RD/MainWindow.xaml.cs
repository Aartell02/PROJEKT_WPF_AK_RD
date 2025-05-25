using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using Forms = System.Windows.Forms;


namespace PROJEKT_WPF_AK_RD;

public partial class MainWindow : Window
{
    private NotifyIcon _notifyIcon;
    private bool _isExit;
    public MainWindow()
    {
        InitializeComponent();
        _notifyIcon = new NotifyIcon();
        _notifyIcon.Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ikona.ico"));
        _notifyIcon.Visible = true;
        _notifyIcon.Text = "Moja aplikacja WPF";
        _notifyIcon.DoubleClick += (s, args) => ShowWindow();
        _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip.Items.Add("Pokaż", null, (s, e) => ShowWindow());
        _notifyIcon.ContextMenuStrip.Items.Add("Zamknij", null, (s, e) => ExitApplication());
    }
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void Close_Click(object sender, RoutedEventArgs e) => System.Windows.Application.Current.Shutdown();

    private void ShowWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private void ExitApplication()
    {
        _isExit = true;
        _notifyIcon.Dispose();
        Close();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!_isExit)
        {
            e.Cancel = true;
            Hide();
        }
        else
        {
            base.OnClosing(e);
        }
    }

    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            Hide();
        }
        base.OnStateChanged(e);
    }
}