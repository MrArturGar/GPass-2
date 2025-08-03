using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GPass.Data;
using GPass.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GPass.ViewModels
{
    public partial class CredentialViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        private bool _isEditing;
        private CredentialSet? _selectedSet;

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public CredentialSet? SelectedSet
        {
            get => _selectedSet;
            set => SetProperty(ref _selectedSet, value);
        }

        public ObservableCollection<CredentialBase> Credentials { get; } = new();

        private static readonly Dictionary<string, Type> _credentialTypes = new()
        {
            { "Title", typeof(CredTitle) },
            { "Field", typeof(CredField) },
            { "SecretField", typeof(CredSecretField) },
            { "Line", typeof(CredLine) }
        };

        public CredentialViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LoadCredentialsForSet(CredentialSet? set)
        {
            if (set == null)
            {
                Credentials.Clear();
                return;
            }

            var credentials = _dbContext.Credentials
                .Where(c => c.SetId == set.Id)
                .OrderBy(c => c.Order)
                .ToList();

            Credentials.Clear();
            foreach (var credential in credentials)
            {
                Credentials.Add(credential);
            }
        }

        [RelayCommand]
        private async Task AddCredential(string credentialTypeKey)
        {
            if (SelectedSet != null)
            {
                await AddCredentialForSet(SelectedSet, credentialTypeKey);
            }
        }

        public async Task AddCredentialForSet(CredentialSet set, string credentialTypeKey)
        {
            if (!_credentialTypes.TryGetValue(credentialTypeKey, out var credentialType))
            {
                return;
            }

            CredentialBase newCredential = credentialTypeKey switch
            {
                "Title" => new CredTitle { Title = "New Title" },
                "Field" => new CredField { Field = "New Field" },
                "SecretField" => new CredSecretField { SecretField = "New Secret" },
                "Line" => new CredLine(),
                _ => throw new ArgumentException($"Unknown credential type: {credentialTypeKey}")
            };

            newCredential.SetId = set.Id;
            newCredential.Type = credentialTypeKey;
            newCredential.Order = Credentials.Count + 1;

            _dbContext.Credentials.Add(newCredential);
            await _dbContext.SaveChangesAsync();

            Credentials.Add(newCredential);
        }

        [RelayCommand]
        private async Task DeleteCredential(CredentialBase cred)
        {
            if (cred == null) return;

            Credentials.Remove(cred);
            _dbContext.Credentials.Remove(cred);
            await _dbContext.SaveChangesAsync();
        }

        [RelayCommand]
        private void ToggleEdit()
        {
            IsEditing = !IsEditing;
        }
    }
} 