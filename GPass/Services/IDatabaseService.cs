using System.Collections.Generic;
using GPass.Models;

namespace GPass.Services;

public interface IDatabaseService
{
    void CreateVault(string path, string password);
    void SaveVault(IEnumerable<Entry> entries);
    IEnumerable<Entry> OpenVault(string path, string password);
}