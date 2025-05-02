using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GPass.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GPass.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private CryptoData? _cryptoData;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CredentialPools))]
    private PoolList? _selectedPoolList;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Credentials))]
    private CredentialPool? _selectedCredentialPool;

    [ObservableProperty]
    private bool _isPoolListEditing;

    [ObservableProperty]
    private bool _isCredPoolEditing;

    [ObservableProperty]
    private bool _isCredentialEditing;

    public MainViewModel()
    {
        CryptoData = CryptoData.GetTestPlaceholders();
    }

    public ObservableCollection<PoolList>? PoolLists => _cryptoData?.PoolLists ?? [];

    public ObservableCollection<CredentialPool>? CredentialPools => _selectedPoolList?.CredentialPools;

    public ObservableCollection<ICredential>? Credentials => _selectedCredentialPool?.Credentials;

    private static readonly Dictionary<string, Type> _credentialTypes = new()
    {
        { "CredTitle", typeof(CredTitle) },
        { "CredField", typeof(CredField) },
        { "CredSecretField", typeof(CredSecretField) },
        { "CredLine", typeof(CredLine) }
    };

    [RelayCommand]
    private void AddPoolList()
    {
        var newPoolOrder = PoolLists.Count + 1;
        var newPool = new PoolList
        {
            Name = $"New Pool {newPoolOrder}",
            Order = newPoolOrder,
            CredentialPools = new ObservableCollection<CredentialPool>()
        };

        _cryptoData.PoolLists.Add(newPool);
        SelectedPoolList = newPool;
    }

    [RelayCommand]
    private void AddCredentialPool()
    {
        var newPoolOrder = SelectedPoolList.CredentialPools.Count + 1;
        var newCreds = new CredentialPool
        {
            Name = $"New Credentials {newPoolOrder}",
            Order = newPoolOrder,
            Credentials = new ObservableCollection<ICredential>()
        };

        SelectedPoolList.CredentialPools.Add(newCreds);
        SelectedCredentialPool = newCreds;
    }

    [RelayCommand]
    private void AddCredential(string credentialTypeKey)
    {

        if (_selectedCredentialPool == null ||
            !_credentialTypes.TryGetValue(credentialTypeKey, out var credentialType))
        {
            return;
        }

        ICredential newCredential = Activator.CreateInstance(credentialType) as ICredential;

        if (newCredential != null)
        {
            newCredential.Order = _selectedCredentialPool.Credentials.Count + 1;
            _selectedCredentialPool.Credentials.Add(newCredential);
        }
    }

    [RelayCommand]
    private void TogglePoolListEdit()
    {
        IsCredentialEditing = false;
        IsCredPoolEditing = false;
        IsPoolListEditing = !IsPoolListEditing;
    }

    [RelayCommand]
    private void ToggleCredPoolEdit()
    {
        IsCredentialEditing = false;
        IsPoolListEditing = false;
        IsCredPoolEditing = !IsCredPoolEditing;
    }

    [RelayCommand]
    private void ToggleCredentialEdit()
    {
        IsPoolListEditing = false;
        IsCredPoolEditing = false;
        IsCredentialEditing = !IsCredentialEditing;
    }
}