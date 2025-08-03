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
using static GPass.ViewModels.CredentialGroupViewModel;

namespace GPass.ViewModels
{
    public partial class CredentialSetViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        private CredentialSet? _selectedCredentialSet;
        private bool _isEditing;
        private CredentialGroup? _selectedGroup;

        private List<EditOperation<CredentialSet>> _setEditOperations = new();

        public CredentialSet? SelectedCredentialSet
        {
            get => _selectedCredentialSet;
            set => SetProperty(ref _selectedCredentialSet, value);
        }

        public CredentialGroup? SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        public bool IsEditing
        {
            get => _isEditing;
            set => SetProperty(ref _isEditing, value);
        }

        public ObservableCollection<CredentialSet> CredentialSets { get; } = new();

        public CredentialSetViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LoadSetsForGroup(CredentialGroup? group)
        {
            if (group == null)
            {
                CredentialSets.Clear();
                return;
            }

            var sets = _dbContext.CredentialSets
                .Include(s => s.Credentials)
                .Where(s => s.GroupId == group.Id)
                .OrderBy(s => s.Order)
                .ToList();

            CredentialSets.Clear();
            foreach (var set in sets)
            {
                CredentialSets.Add(set);
            }
        }

        [RelayCommand]
        private async Task AddCredentialSet()
        {
            if (!IsEditing || SelectedGroup == null) return;
            
            await AddCredentialSetForGroup(SelectedGroup);
        }

        public async Task AddCredentialSetForGroup(CredentialGroup group)
        {
            var newSetOrder = CredentialSets.Count + 1;
            var newSet = new CredentialSet
            {
                GroupId = group.Id,
                Name = $"New Set {newSetOrder}",
                Order = newSetOrder,
                Credentials = new List<CredentialBase>()
            };

            CredentialSets.Add(newSet);
            _setEditOperations.Add(new() { OperationType = OperationType.Add, Object = newSet });
            SelectedCredentialSet = newSet;
        }

        [RelayCommand]
        private void DeleteCredentialSet(CredentialSet credSet)
        {
            if (credSet == null || !IsEditing) return;

            CredentialSets.Remove(credSet);
            _setEditOperations.Add(new() { OperationType = OperationType.Delete, Object = credSet });
        }

        [RelayCommand]
        private async Task ToggleEdit()
        {
            if (SelectedGroup == null) return;

            if (!IsEditing)
            {
                _setEditOperations.Clear();
                foreach (var group in CredentialSets)
                {
                    _setEditOperations.Add(new()
                    {
                        OperationType = OperationType.Update,
                        Object = group,
                        OriginalObject = new()
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
            for (int i = 0; i < CredentialSets.Count; i++)
            {
                var group = CredentialSets[i];
                if (group.Order != i)
                {
                    group.Order = i;
                    var existingOperation = _setEditOperations.FirstOrDefault(op =>
                        op.OperationType == OperationType.Update && op.Object.Id == group.Id);
                    if (existingOperation != null)
                    {
                        existingOperation.Object.Order = i;
                    }
                }
            }

            foreach (var operation in _setEditOperations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        _dbContext.CredentialSets.Add(operation.Object);
                        break;
                    case OperationType.Delete:
                        var dbSet = await _dbContext.CredentialSets.FindAsync(operation.Object.Id);
                        if (dbSet != null)
                            _dbContext.CredentialSets.Remove(dbSet);
                        break;
                    case OperationType.Update:
                        var dbGroupToUpdate = await _dbContext.CredentialSets.FindAsync(operation.Object.Id);
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
            foreach (var operation in _setEditOperations)
            {
                switch (operation.OperationType)
                {
                    case OperationType.Add:
                        CredentialSets.Remove(operation.Object);
                        break;
                    case OperationType.Delete:
                        if (!CredentialSets.Contains(operation.Object))
                            CredentialSets.Add(operation.Object);
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
            _setEditOperations.Clear();

            var sortedSets = CredentialSets.OrderBy(g => g.Order).ToList();
            CredentialSets.Clear();
            foreach (var set in sortedSets)
            {
                CredentialSets.Add(set);
            }

            RefreshCollection();
            IsEditing = false;
        }

        private void RefreshCollection()
        {
            var tempList = CredentialSets.ToList();
            CredentialSets.Clear();
            foreach (var item in tempList)
            {
                CredentialSets.Add(item);
            }
        }
    }
} 