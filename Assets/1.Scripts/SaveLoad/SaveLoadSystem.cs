using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class SaveLoadSystem : MonoBehaviour
{
    public static readonly string[] SaveFileName =
    {
        "SaveAuto.sav",
        "SaveAuto1.sav",
        "Save2.sav",
        "Save3.sav"
    };

    public static SaveData1 CurrentData { get; set; }
    public static int currentSlot = 1;
    public static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }

    public static bool Save(int slot = -1, SaveData data = null)
    {
        if (data == null)
            data = CurrentData;

        if (slot == -1)
            slot = currentSlot;

        if (slot < 0 || slot >= SaveFileName.Length)
            return false;

        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();

            serializer.Converters.Add(new ColorConverter());
            serializer.Converters.Add(new Vector3Converter());
            serializer.Converters.Add(new Vector2Converter());
            serializer.Converters.Add(new QuaternionConverter());
            serializer.Converters.Add(new CharacterInfoConverter());

            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Serialize(writer, data);
        }

        CurrentData = data as SaveData1;
        currentSlot = slot;
        return true;
    }
    public static SaveData Load(int slot = -1, SaveData data = null)
    {
        if (data == null)
            data = CurrentData;

        if (slot == -1)
            slot = currentSlot;

        if (slot < 0 || slot >= SaveFileName.Length)
            return null;

        if (!Directory.Exists(SaveDirectory))
            return null;

        try
        {
            var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
            using (var writer = new JsonTextReader(new StreamReader(path)))
            {
                var serializer = new JsonSerializer();
                serializer.TypeNameHandling = TypeNameHandling.All;
                serializer.Converters.Add(new ColorConverter());
                serializer.Converters.Add(new Vector3Converter());
                serializer.Converters.Add(new Vector2Converter());
                serializer.Converters.Add(new QuaternionConverter());
                serializer.Converters.Add(new CharacterInfoConverter());
                var saveData = serializer.Deserialize<SaveData>(writer);

                CurrentData = saveData as SaveData1;
                currentSlot = slot;
                return saveData;
            }
        }
        catch(FileNotFoundException ex)
        {
            //Debug.Log($"세이브 파일을 찾을 수 없습니다 " + ex);
            return null;
        }
        
    }

}
