using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.EntityFrameworkCore;
using GPass.Views;
using GPass.Data;
using GPass.Utils;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;

namespace GPass.ViewModels;

public partial class AuthViewModel : ObservableObject
{
    private const int MAX_LOGIN_ATTEMPTS = 3;
    private const int LOCKOUT_TIME_MINUTES = 5;
    private const string DB_FILE = "credentials.db";

    [ObservableProperty]
    private bool _isLoginEnabled = true;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private int _firstNumber;

    [ObservableProperty]
    private int _secondNumber;

    [ObservableProperty]
    private bool _isWindowsHelloAvailable;

    [ObservableProperty]
    private bool _isRegistration;

    [ObservableProperty]
    private ObservableCollection<string> _storageList = new();

    [ObservableProperty]
    private string _selectedStorage = string.Empty;

    private int _loginAttempts;
    private DateTime _lastFailedAttempt;
    private byte[]? _encryptionKey;
    private Window? _window;
    private AppDbContext? _dbContext;
    private DateTime? _lockoutEndTime;

    public AuthViewModel(Window window)
    {
        _window = window;
        GenerateCaptcha();
        CheckWindowsHello();
        CheckSavedKey();
        LoadStorageList();
    }

    private async void CheckSavedKey()
    {
        try
        {
            var key = await FileStoreUtils.GetAuthKey();
            
            if (key.Any())
            {
                //var key = await FileIO.ReadBufferAsync(file);
                //_encryptionKey = key.ToArray();
                
                if (IsWindowsHelloAvailable)
                {
                    await WindowsHello();
                }
                else
                {
                    ErrorMessage = "Требуется вход через Windows Hello";
                }
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку для отладки
            System.Diagnostics.Debug.WriteLine($"Ошибка при проверке ключа: {ex.Message}");
        }
    }

    private void GenerateCaptcha()
    {
        var random = new Random();
        FirstNumber = random.Next(1, 10);
        SecondNumber = random.Next(1, 10);
    }

    private async void CheckWindowsHello()
    {
        try
        {
            var keyCredentialAvailable = await KeyCredentialManager.IsSupportedAsync();
            IsWindowsHelloAvailable = keyCredentialAvailable;
        }
        catch
        {
            IsWindowsHelloAvailable = false;
        }
    }

    private async void LoadStorageList()
    {
        try
        {
            var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppConsts.APP_NAME);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var dbFiles = Directory.GetFiles(folderPath, "*.db").Select(Path.GetFileName).ToList();
            StorageList = new ObservableCollection<string>(dbFiles);
            if (StorageList.Count > 0)
                SelectedStorage = StorageList[0];
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке списка хранилищ: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task Login(string result)
    {
        if (!IsLoginEnabled)
        {
            ErrorMessage = "Слишком много попыток. Попробуйте позже.";
            return;
        }

#if DEBUG == false //TODO: Improve
        if (string.IsNullOrEmpty(result) || !int.TryParse(result, out int userResult))
        {
            ErrorMessage = "Введите правильный результат";
            return;
        }
#else
        var userResult = FirstNumber + SecondNumber;
#endif

        if (userResult != FirstNumber + SecondNumber)
        {
            ErrorMessage = "Неверный результат";
            _loginAttempts++;
            if (_loginAttempts >= MAX_LOGIN_ATTEMPTS)
            {
                _lockoutEndTime = DateTime.Now.AddMinutes(LOCKOUT_TIME_MINUTES);
                IsLoginEnabled = false;
                ErrorMessage = $"Слишком много попыток. Попробуйте через {LOCKOUT_TIME_MINUTES} минут";
                return;
            }
            ErrorMessage = $"Неверный результат. Осталось попыток: {MAX_LOGIN_ATTEMPTS - _loginAttempts}";
            GenerateCaptcha();
            return;
        }

        // Получаем имя файла БД
        var dbFileName = string.IsNullOrWhiteSpace(SelectedStorage) ? "credentials.db" : SelectedStorage;
        var folderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Utils.AppConsts.APP_NAME);
        var dbPath = System.IO.Path.Combine(folderPath, dbFileName);

        try
        {
            if (_encryptionKey == null)
            {
                _encryptionKey = CryptoUtils.GenerateAuthKey();
                await FileStoreUtils.SaveAuthKey(_encryptionKey);
            }

            // Инициализируем БД
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            _dbContext = new AppDbContext(options);
            await _dbContext.Database.EnsureCreatedAsync();

            ActivateMainWindow();
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка авторизации: " + ex.Message;
        }
    }

    [RelayCommand]
    private async Task WindowsHello()
    {
        try
        {
            var keyCredential = await KeyCredentialManager.RequestCreateAsync(AppConsts.APP_NAME, KeyCredentialCreationOption.ReplaceExisting);
            if (keyCredential.Status == KeyCredentialStatus.Success)
            {
                if (_encryptionKey == null)
                {
                    _encryptionKey = CryptoUtils.GenerateAuthKey();
                    await FileStoreUtils.SaveAuthKey(_encryptionKey);
                }

                // Инициализируем БД
                var dbFileName = string.IsNullOrWhiteSpace(SelectedStorage) ? "credentials.db" : SelectedStorage;
                var folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Utils.AppConsts.APP_NAME);
                var dbPath = Path.Combine(folderPath, dbFileName);
                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseSqlite($"Data Source={dbPath}")
                    .Options;

                _dbContext = new AppDbContext(options);
                await _dbContext.Database.EnsureCreatedAsync();

                ActivateMainWindow();
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка Windows Hello: " + ex.Message;
        }
    }

    [RelayCommand]
    private void ToggleRegistration()
    {
        IsRegistration = !IsRegistration;
        ErrorMessage = string.Empty;
        if (!IsRegistration)
        {
            LoadStorageList();
        }
        else
        {
            SelectedStorage = string.Empty;
        }
    }

    private void ActivateMainWindow()
    {
        var mainWindow = new MainWindow(_dbContext);
        mainWindow.Activate();

        if (_window != null)
        {
            _window.Close();
        }
    }
} 