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

namespace GPass.ViewModels
{
    public partial class CredentialGroupViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        private CredentialGroup? _selectedCredentialGroup;
        private bool _isEditing;

        private List<EditOperation<CredentialGroup>> _groupEditOperations = new();

        public CredentialGroup? SelectedCredentialGroup
        {
            get => _selectedCredentialGroup;
            set => SetProperty(ref _selectedCredentialGroup, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public ObservableCollection<CredentialGroup> CredentialGroups { get; } = new();

        public CredentialGroupViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            LoadData();
        }

        private async void LoadData()
        {
            var groups = await _dbContext.CredentialGroups
                .Include(g => g.CredentialSets)
                .OrderBy(g => g.Order)
                .ToListAsync();

            CredentialGroups.Clear();
            foreach (var group in groups)
            {
                CredentialGroups.Add(group);
            }
        }

        [RelayCommand]
        private void AddCredentialGroup()
        {
            if (!IsEditing) return;

            var newGroupOrder = CredentialGroups.Count + 1;
            var newGroup = new CredentialGroup
            {
                Name = $"New Group {newGroupOrder}",
                Order = newGroupOrder,
                CredentialSets = new List<CredentialSet>()
            };

            CredentialGroups.Add(newGroup);
            _groupEditOperations.Add(new() { OperationType = OperationType.Add, Object = newGroup });
            SelectedCredentialGroup = newGroup;
        }

        [RelayCommand]
        private void DeleteCredentialGroup(CredentialGroup group)
        {
            if (group == null || !IsEditing) return;

            CredentialGroups.Remove(group);
            _groupEditOperations.Add(new() { OperationType = OperationType.Delete, Object = group });
        }

        [RelayCommand]
        private async Task ToggleEdit()
        {
            if (!IsEditing)
            {
                _groupEditOperations.Clear();
                foreach (var group in CredentialGroups)
                {
                    _groupEditOperations.Add(new()
                    {
                        OperationType = OperationType.Update,
                        Object = group,
                        OriginalObject = new CredentialGroup
                        {
                            Id = group.Id,
                            Name = group.Name,
                            Order = group.Order
                        }
                    });
                }
            }
            else
            {
                await SaveEdit();
            }
            IsEditing = !IsEditing;
        }

        private async Task SaveEdit()
        {
            for (int i = 0; i < CredentialGroups.Count; i++)
            {
                var group = CredentialGroups[i];
                if (group.Order != i)
                {
                    group.Order = i;
                    var existingOperation = _groupEditOperations.FirstOrDefault(op => 
                        op.OperationType == OperationType.Update && op.Object.Id == group.Id);
                    if (existingOperation != null)
                    {
                        existingOperation.Object.Order = i;
                    }
                }
            }

            foreach (var operation in _groupEditOperations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        _dbContext.CredentialGroups.Add(operation.Object);
                        break;
                    case OperationType.Delete:
                        var dbGroup = await _dbContext.CredentialGroups.FindAsync(operation.Object.Id);
                        if (dbGroup != null)
                            _dbContext.CredentialGroups.Remove(dbGroup);
                        break;
                    case OperationType.Update:
                        var dbGroupToUpdate = await _dbContext.CredentialGroups.FindAsync(operation.Object.Id);
                        if (dbGroupToUpdate != null)
                        {
                            dbGroupToUpdate.Order = operation.Object.Order;
                            dbGroupToUpdate.Name = operation.Object.Name;
                        }
                        break;
                }
            }
            await _dbContext.SaveChangesAsync();
            RefreshCollection();
        }

        [RelayCommand]
        private void CancelEdit()
        {
            foreach (var operation in _groupEditOperations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        CredentialGroups.Remove(operation.Object);
                        break;
                    case OperationType.Delete:
                        if (!CredentialGroups.Contains(operation.Object))
                            CredentialGroups.Add(operation.Object);
                        break;
                    case OperationType.Update:
                        if (operation.OriginalObject != null)
                        {
                            operation.Object.Name = operation.OriginalObject.Name;
                            operation.Object.Order = operation.OriginalObject.Order;
                        }
                        break;
                }
            }
            _groupEditOperations.Clear();
            
            // Sort collection by Order after restoration
            var sortedGroups = CredentialGroups.OrderBy(g => g.Order).ToList();
            CredentialGroups.Clear();
            foreach (var group in sortedGroups)
            {
                CredentialGroups.Add(group);
            }
            
            RefreshCollection();
            IsEditing = false;
        }

        private void RefreshCollection()
        {
            var tempList = CredentialGroups.ToList();
            CredentialGroups.Clear();
            foreach (var item in tempList)
            {
                CredentialGroups.Add(item);
            }
        }
    }
} 