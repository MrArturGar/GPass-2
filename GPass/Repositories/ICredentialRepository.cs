using GPass.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GPass.Repositories
{
    public interface ICredentialRepository
    {
        Task<IEnumerable<CredentialGroup>> GetGroupsAsync();
        Task<CredentialGroup?> GetGroupAsync(int id);
        Task<CredentialGroup> AddGroupAsync(CredentialGroup group);
        Task UpdateGroupAsync(CredentialGroup group);
        Task DeleteGroupAsync(int id);

        Task<IEnumerable<CredentialSet>> GetSetsAsync(int groupId);
        Task<CredentialSet?> GetSetAsync(int id);
        Task<CredentialSet> AddSetAsync(CredentialSet set);
        Task UpdateSetAsync(CredentialSet set);
        Task DeleteSetAsync(int id);

        Task<IEnumerable<CredentialBase>> GetCredentialsAsync(int setId);
        Task<CredentialBase?> GetCredentialAsync(int id);
        Task<CredentialBase> AddCredentialAsync(CredentialBase credential);
        Task UpdateCredentialAsync(CredentialBase credential);
        Task DeleteCredentialAsync(int id);
    }
} 