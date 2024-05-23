using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
    public abstract SaveData VersionDown();
}

public class SaveData1 : SaveData
{
    // 플레이어가 보유중인 플레이어 데이터
    public List<CharacterInfo> characterDataList;
    public int currentStage = 0;
    public int tryCount = 0;

    public SaveData1()
    {
        Version = 1;
        characterDataList = new List<CharacterInfo>();
    }
    public override SaveData VersionDown()
    {
        return null;
    }

    public override SaveData VersionUp()
    {
        return null;
        // return new SaveData2
        // {
        //     Gold = Gold
        // };
    }
}
