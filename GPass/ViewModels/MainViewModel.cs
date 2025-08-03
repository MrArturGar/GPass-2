using CommunityToolkit.Mvvm.ComponentModel;
using GPass.Data;
using GPass.Models;

namespace GPass.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public CredentialGroupViewModel GroupViewModel { get; }
        public CredentialSetViewModel SetViewModel { get; }
        public CredentialViewModel CredentialViewModel { get; }

        public MainViewModel(AppDbContext dbContext)
        {
            GroupViewModel = new CredentialGroupViewModel(dbContext);
            SetViewModel = new CredentialSetViewModel(dbContext);
            CredentialViewModel = new CredentialViewModel(dbContext);

            // Subscribe to group selection changes
            GroupViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(GroupViewModel.SelectedCredentialGroup))
                {
                    SetViewModel.SelectedGroup = GroupViewModel.SelectedCredentialGroup;
                    SetViewModel.LoadSetsForGroup(GroupViewModel.SelectedCredentialGroup);
                    SetViewModel.SelectedCredentialSet = null;
                }
            };

            // Subscribe to set selection changes
            SetViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(SetViewModel.SelectedCredentialSet))
                {
                    CredentialViewModel.SelectedSet = SetViewModel.SelectedCredentialSet;
                    CredentialViewModel.LoadCredentialsForSet(SetViewModel.SelectedCredentialSet);
                }
            };
        }
    }
}