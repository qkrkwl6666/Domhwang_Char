using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [field: SerializeField] public int Id { get; set; }
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public string Tier { get; set; }
    [field: SerializeField] public int Atk { get; set; }
    [field: SerializeField] public int Atk_Up { get; set; }
    [field: SerializeField] public int Run { get; set; }
    [field: SerializeField] public int Run_Up { get; set; }
    [field: SerializeField] public int Skill_Id { get; set; }
    [field: SerializeField] public int InstanceId { get; set; }
    [field: SerializeField] public int Texture { get; set; }
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public int BattleAttack { get; set; }
    [field: SerializeField] public int Atk_Effect_Id { get; set; }
    [field: SerializeField] public int Cry_Effect_Id { get; set; }
    [field: SerializeField] public int Run_Effect_Id { get; set; }

    [field: SerializeField] public CharacterSkillData CharacterSkillData { get; set; }

    public Sprite characterImage;
    public System.DateTime creationTime;

    public void InitializeSkill(BattleSystem battleSystem)
    {
        if (CharacterSkillData == null) return;

        bool conditionMet = false;

        switch (CharacterSkillData.ConditionType)
        {
            case 0:
                conditionMet = true;
                break;
            // �ش���尡 ConditionValue ���� �϶� 
            case 6:
                conditionMet = battleSystem.CurrentRound == CharacterSkillData.ConditionValue;
                break;

            // �ش� ���忡�� ĳ���Ͱ� ���ݿ� ���� �� ��
            case 9:
                var ci = GetComponent<CharacterControll>();
                ci.RunMode(true);
                conditionMet = ci.isRun == false;
                Debug.Log(conditionMet);
                break;
        }

        if (conditionMet)
        {
            SkillType(battleSystem);
        }
    }

    public void ApplySkill(BattleSystem battleSystem)
    {
        if (CharacterSkillData == null) return;

        bool conditionMet = false;

        // ��ų ���� üũ
        switch (CharacterSkillData.ConditionType)
        {
            case 1:
                conditionMet = battleSystem.MonsterInfo.Hp <= battleSystem.MonsterInfo.MaxHp * CharacterSkillData.ConditionValue * 0.01f;
                break;
            case 2:
                conditionMet = battleSystem.remainingCharacters.Count >= CharacterSkillData.ConditionValue;
                break;
            case 3:
                conditionMet = battleSystem.StandRemainingCharacters.Contains(gameObject);
                break;
            case 4:
                // ĳ���Ͱ� �ܷ� ���� Ȯ�� �ϰ� && ���ÿ� �ٸ� �ο��� ������ �������� ���
                conditionMet = battleSystem.StandRemainingCharacters.Contains(gameObject) && battleSystem.StandRemainingCharacters.Count >= 2;
                break;
            case 5:
                conditionMet = battleSystem.playingCharacters.Count == 1 && battleSystem.playingCharacters[0] == gameObject;
                break;
            case 8:
                conditionMet = battleSystem.CurrentRound == CharacterSkillData.ConditionValue && GetComponent<CharacterControll>().isRun;
                break;
        }
        Debug.Log(conditionMet);
        if (conditionMet)
        {
            SkillType(battleSystem);
        }
    }

    public void SkillType(BattleSystem battleSystem)
    {
        switch (CharacterSkillData.EffectType)
        {
            case 1:
                IncreasedDamage(battleSystem);
                break;
            case 2:
                chargeForward(battleSystem);
                break;
            case 3:
                RemainingRoundsPass(battleSystem);
                break;
            case 4:
                TryCountPass();
                break;
            case 5:
                // �ݵ�� ���� �ϸ� �ݵ�� �ܷ� ���� ����
                ChargeAndRun(battleSystem);
                break;
            case 6:
                // �� ������ ���ݷ���, ���� �ο� �� ���� ���� ��ġ�� �����ȴ�.
                HighDamage(battleSystem);
                break;
        }
    }

    public void IncreasedDamage(BattleSystem battleSystem)
    {
        // ��ų Ÿ�� ���� �� ȿ�� ����
        switch (CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                Debug.Log("�ڽ��� ĳ���Ϳ��� ȿ�� ����");
                BattleAttack = DamageCheck(this);
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                Debug.Log("�ܷ� ���翡�� ȿ�� ����");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = DamageCheck(cc);
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                Debug.Log("��� �ο����� ȿ�� ����");
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterInfo>();
                        cc.BattleAttack = DamageCheck(cc);
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = DamageCheck(cc);
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = DamageCheck(cc);
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public int DamageCheck(CharacterInfo characterInfo)
    {
        float damage = 0;

        switch (CharacterSkillData.EffectType)
        {
            case 1:
                if (CharacterSkillData.EffectValue.EndsWith("x"))
                {
                    float multiplier = float.Parse(CharacterSkillData.EffectValue.TrimEnd('x'));
                    damage = (int)multiplier * characterInfo.BattleAttack;
                }
                else if (CharacterSkillData.EffectValue.EndsWith("%"))
                {
                    float multiplier = float.Parse(CharacterSkillData.EffectValue.TrimEnd('%'));
                    damage = (int)multiplier * characterInfo.BattleAttack * 0.01f;

                }
                else
                {
                    damage = int.Parse(CharacterSkillData.EffectValue) + characterInfo.BattleAttack;
                }
                break;
        }

        return (int)damage;
    }

    // ������ ���� �޼���
    public void chargeForward(BattleSystem battleSystem)
    {
        switch (CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                Debug.Log("�ڽ��� ĳ���Ϳ��� ȿ�� ����");
                GetComponent<CharacterControll>().isPass = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                Debug.Log("�ܷ� ���翡�� ȿ�� ����");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                Debug.Log("��� �ο����� ȿ�� ����");
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isPass = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void RemainingRoundsPass(BattleSystem battleSystem)
    {
        switch (CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                Debug.Log("�ڽ��� ĳ���Ϳ��� ȿ�� ����");
                GetComponent<CharacterControll>().isAttackEndPass = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                Debug.Log("�ܷ� ���翡�� ȿ�� ����");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                Debug.Log("��� �ο����� ȿ�� ����");
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isAttackEndPass = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isAttackEndPass = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void ChargeAndRun(BattleSystem battleSystem)
    {
        switch (CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                Debug.Log("�ڽ��� ĳ���Ϳ��� ȿ�� ����");
                GetComponent<CharacterControll>().isPass = true;
                GetComponent<CharacterControll>().confirmAttackEndRun = true;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                Debug.Log("�ܷ� ���翡�� ȿ�� ����");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                Debug.Log("��� �ο����� ȿ�� ����");
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterControll>();
                        cc.isPass = true;
                        cc.confirmAttackEndRun = true;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterControll>();
                    cc.isPass = true;
                    cc.confirmAttackEndRun = true;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void HighDamage(BattleSystem battleSystem)
    {
        int maxAttack = 0;

        foreach(var characterList in battleSystem.battleCharacter)
        {
            foreach(var character in characterList)
            {
                var ci = character.GetComponent<CharacterInfo>();
                if(maxAttack < ci.BattleAttack) maxAttack = ci.BattleAttack;
            }
        }

        switch (CharacterSkillData.Target)
        {
            case 1:
                // �ڽ��� ĳ���Ϳ��� ȿ�� ����
                Debug.Log("�ڽ��� ĳ���Ϳ��� ȿ�� ����");
                BattleAttack = maxAttack;
                GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                break;
            case 2:
                // �ܷ� ���翡�� ȿ�� ����
                Debug.Log("�ܷ� ���翡�� ȿ�� ����");
                foreach (var character in battleSystem.remainingCharacters)
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 3:
                // ��� �ο����� ȿ�� ����
                Debug.Log("��� �ο����� ȿ�� ����");
                foreach (var characterList in battleSystem.battleCharacter)
                {
                    foreach (var character in characterList)
                    {
                        var cc = character.GetComponent<CharacterInfo>();
                        cc.BattleAttack = maxAttack;
                        cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                    }
                }
                break;
            case 4:
                //���� ������ ���� �ο����� ȿ�� ����
                int nextRound = battleSystem.CurrentRound + 1;
                if (nextRound > battleSystem.Round) break;
                foreach (var character in battleSystem.roundsCharacters[nextRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
            case 5:
                //�ش� ������ ���� �ο����� ȿ�� ����
                foreach (var character in battleSystem.roundsCharacters[battleSystem.CurrentRound - 1])
                {
                    var cc = character.GetComponent<CharacterInfo>();
                    cc.BattleAttack = maxAttack;
                    cc.GetComponent<CharacterSkillIcon>().AddSkillIcon(CharacterSkillData.Skill_Icon.ToString());
                }
                break;
        }
    }

    public void TryCountPass()
    {
        GameManager.Instance.isPassTryCount = true;
    }

    public void SetCharacterData(CharacterData data)
    {
        Id = data.Id;
        Name = data.Name;
        Tier = data.Tier;
        Atk = data.Atk;
        Atk_Up = data.Atk_Up;
        Run = data.Run;
        Run_Up = data.Run_Up;
        Skill_Id = data.Skill_Id;
        Level = data.Level;
        Texture = data.Id;
        BattleAttack = data.Atk;
        Atk_Effect_Id = data.Atk_Effect_Id;
        Cry_Effect_Id = data.Cry_Effect_Id;
        Run_Effect_Id = data.Run_Effect_Id;

        CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
        //characterImage = Resources.Load<Sprite>("ChatacterImage/" + Id.ToString() + "Img");
    }

    public void SetCharacterInfo(CharacterInfo characterInfo)
    {
        Id = characterInfo.Id;
        Name = characterInfo.Name;
        Tier = characterInfo.Tier;
        Atk = characterInfo.Atk;
        Atk_Up = characterInfo.Atk_Up;
        Run = characterInfo.Run;
        Run_Up = characterInfo.Run_Up;
        Skill_Id = characterInfo.Skill_Id;
        Level = characterInfo.Level;
        Texture = characterInfo.Id;
        InstanceId = characterInfo.InstanceId;
        BattleAttack = characterInfo.Atk;
        Atk_Effect_Id = characterInfo.Atk_Effect_Id;
        Cry_Effect_Id = characterInfo.Cry_Effect_Id;
        Run_Effect_Id = characterInfo.Run_Effect_Id;


        CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
        //CharacterSkillData = DataTableMgr.Instance.Get<CharacterSkillTable>("CharacterSkill").Get(Skill_Id.ToString());
    }

    public CharacterData ConvertCharacterData()
    {
        CharacterData characterData = new CharacterData();

        characterData.Id = Id;
        characterData.Name = Name;
        characterData.Tier = Tier;
        characterData.Atk = Atk;
        characterData.Atk_Up = Atk_Up;
        characterData.Run = Run;
        characterData.Run_Up = Run_Up;
        characterData.Skill_Id = Skill_Id;
        characterData.Level = Level;
        characterData.Instance_Id = InstanceId;
        characterData.Atk_Effect_Id = Atk_Effect_Id;
        characterData.Cry_Effect_Id = Atk_Effect_Id;
        characterData.Run_Effect_Id = Atk_Effect_Id;

        return characterData;
    }

    public void LevelUp()
    {
        Level++;
        Atk += Atk_Up;
        Run -= Run_Up;

        if (Run < 0) Run = 0;
    }

}
