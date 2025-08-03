using GPass.Data;
using GPass.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPass.Repositories
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly AppDbContext _context;

        public CredentialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CredentialGroup>> GetGroupsAsync()
        {
            return await _context.CredentialGroups
                .Include(g => g.CredentialSets)
                .OrderBy(g => g.Order)
                .ToListAsync();
        }

        public async Task<CredentialGroup?> GetGroupAsync(int id)
        {
            return await _context.CredentialGroups
                .Include(g => g.CredentialSets)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<CredentialGroup> AddGroupAsync(CredentialGroup group)
        {
            _context.CredentialGroups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task UpdateGroupAsync(CredentialGroup group)
        {
            _context.Entry(group).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGroupAsync(int id)
        {
            var group = await _context.CredentialGroups.FindAsync(id);
            if (group != null)
            {
                _context.CredentialGroups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CredentialSet>> GetSetsAsync(int groupId)
        {
            return await _context.CredentialSets
                .Include(s => s.Credentials)
                .Where(s => s.GroupId == groupId)
                .OrderBy(s => s.Order)
                .ToListAsync();
        }

        public async Task<CredentialSet?> GetSetAsync(int id)
        {
            return await _context.CredentialSets
                .Include(s => s.Credentials)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<CredentialSet> AddSetAsync(CredentialSet set)
        {
            _context.CredentialSets.Add(set);
            await _context.SaveChangesAsync();
            return set;
        }

        public async Task UpdateSetAsync(CredentialSet set)
        {
            _context.Entry(set).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSetAsync(int id)
        {
            var set = await _context.CredentialSets.FindAsync(id);
            if (set != null)
            {
                _context.CredentialSets.Remove(set);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<CredentialBase>> GetCredentialsAsync(int setId)
        {
            return await _context.Credentials
                .Where(c => c.SetId == setId)
                .OrderBy(c => c.Order)
                .ToListAsync();
        }

        public async Task<CredentialBase?> GetCredentialAsync(int id)
        {
            return await _context.Credentials.FindAsync(id);
        }

        public async Task<CredentialBase> AddCredentialAsync(CredentialBase credential)
        {
            _context.Credentials.Add(credential);
            await _context.SaveChangesAsync();
            return credential;
        }

        public async Task UpdateCredentialAsync(CredentialBase credential)
        {
            _context.Entry(credential).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCredentialAsync(int id)
        {
            var credential = await _context.Credentials.FindAsync(id);
            if (credential != null)
            {
                _context.Credentials.Remove(credential);
                await _context.SaveChangesAsync();
            }
        }
    }
} 