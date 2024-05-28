using System.Collections.Generic;
using UnityEditor;

public class CharacterScriptableObjectCreator : Editor
{
    [MenuItem("Editor/캐릭터 스크립터블 오브젝트 생성")]
    public static void CreateCharacterData()
    {
        CharacterTable characterTable = new CharacterTable();
        characterTable.Load("CharacterData");

        foreach (KeyValuePair<string, CharacterData> pair in characterTable.characterTable)
        {
            string key = pair.Key;
            CharacterData characterData = pair.Value;
            string path = $"Assets/Resources/ScriptableObject/CharacterInitialData/{key}.asset";

            //AssetDatabase.DeleteAsset(path);
            //AssetDatabase.CreateAsset(characterData, path);
        }
        //AssetDatabase.SaveAssets();
    }
}
