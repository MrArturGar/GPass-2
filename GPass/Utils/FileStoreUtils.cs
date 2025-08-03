using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace GPass.Utils;

class FileStoreUtils
{

    private const string KEY_FILE = "auth.key";

    public static T? Deserialize<T>(string path)
    {
        CreateIfNeed(path);

        var text = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(text);
    }

    public static void Serialize<T>(string path, T model)
    {
        CreateIfNeed(path);

        var text = JsonConvert.SerializeObject(model, Formatting.Indented);
        File.WriteAllText(path, text, Encoding.UTF8);
    }

    private static void CreateIfNeed(string path)
    {
        if (File.Exists(path))
            return;

        File.Create(path);
    }

    private async static Task<StorageFolder> GetStorageFolder()
    {
        var localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appFolderPath = Path.Combine(localAppDataPath, AppConsts.APP_NAME);

        if (!Directory.Exists(appFolderPath))
        {
            Directory.CreateDirectory(appFolderPath);
        }

        return await StorageFolder.GetFolderFromPathAsync(appFolderPath);
    }

    public static async Task SaveAuthKey(byte[] key)
    {
        StorageFolder localFolder = await GetStorageFolder();
        var file = await localFolder.CreateFileAsync(KEY_FILE, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteBytesAsync(file, key);
    }

    public static async Task<byte[]> GetAuthKey()
    {
        StorageFolder localFolder = await GetStorageFolder();
        var file = await localFolder.TryGetItemAsync(KEY_FILE) as StorageFile;

        if (file != null)
        {
            var key = await FileIO.ReadBufferAsync(file);
            return key.ToArray();
        }

        return Array.Empty<byte>();
    }

    public static async Task<byte[]> GetOrCreateAuthKey()
    {
        var key = await GetAuthKey();

        if (key.Any()) return key;

        key = CryptoUtils.GenerateAuthKey();
        await SaveAuthKey(key);
        return key;
    }
}
