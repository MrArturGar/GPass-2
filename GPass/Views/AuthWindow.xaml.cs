using GPass.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;

namespace GPass.Views;

public sealed partial class AuthWindow : Window
{
    public AuthWindow()
    {
        InitializeComponent();
        RootGrid.DataContext = new AuthViewModel(this);

        // Настройка окна
        Title = "GPass - Авторизация";
        
        // Получаем размер экрана
        var displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
        var scale = displayArea.WorkArea.Width / 1920.0; // Базовое разрешение 1920x1080

        // Устанавливаем размер окна
        //Width = 400 * scale;
        //Height = 400 * scale;

        // Центрируем окно
        //var centerX = (displayArea.WorkArea.Width - Width) / 2;
        //var centerY = (displayArea.WorkArea.Height - Height) / 2;
        //AppWindow.Move(new Windows.Graphics.PointInt32((int)centerX, (int)centerY));
    }
} 