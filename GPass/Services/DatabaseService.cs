using System.IO;
using System.Collections.Generic;
using GPass.Models;
using System.Security.Cryptography;

namespace GPass.Services;

public class DatabaseService : IDatabaseService
{
    private readonly IEncryptionService _encryption;
    private string _currentVaultPath;

    public DatabaseService(IEncryptionService encryption)
    {
        _encryption = encryption;
    }

    public void CreateVault(string path, string password)
    {
        using var fs = File.Create(path);
        var salt = RandomNumberGenerator.GetBytes(16);
        var header = new Vault { Salt = salt };
        WriteHeader(fs, header, password);
        _currentVaultPath = path;
    }

    public void SaveVault(IEnumerable<Entry> entries)
    {
        using var fs = File.OpenWrite(_currentVaultPath);
        fs.SetLength(0);
        WriteHeader(fs, ReadHeader(fs), "");
        WriteEntries(fs, entries);
    }

    public IEnumerable<Entry> OpenVault(string path, string password)
    {
        using var fs = File.OpenRead(path);
        var header = ReadHeader(fs);
        _currentVaultPath = path;
        return ReadEntries(fs, header, password);
    }

    private void WriteHeader(FileStream fs, Vault header, string password)
    {
        using var bw = new BinaryWriter(fs);
        bw.Write(header.MagicNumber);
        bw.Write(header.Version);
        bw.Write(header.Salt);
    }

    private Vault ReadHeader(FileStream fs)
    {
        using var br = new BinaryReader(fs);
        return new Vault
        {
            MagicNumber = br.ReadBytes(4),
            Version = br.ReadDouble(),
            Salt = br.ReadBytes(16)
        };
    }

    private void WriteEntries(FileStream fs, IEnumerable<Entry> entries)
    {
        // Implementation for writing encrypted entries
    }

    private IEnumerable<Entry> ReadEntries(FileStream fs, Vault header, string password)
    {
        // Implementation for reading encrypted entries
        yield break;
    }
}