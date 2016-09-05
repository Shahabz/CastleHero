using System;

public enum CastleState
{
    Peace = 1,
    Famine,
    Attacked,
    BeingAttacked
}

[Serializable]
public class UserData
{
    string Id;
    string Pw;
    int heroId;
    int heroLevel;
    Item[] equipment;
    Item[] inventory;
    int[] skill;
    int unitKind;
    Unit[] unit;
    int[] createUnit;
    int[] attackUnit;
    int[] building;
    int buildBuilding;
    int[] upgrade;
    int resource;
    int castleState;

    public string ID { get { return Id; } }
    public string PW { get { return Pw; } }
    public int HeroId { get { return heroId; } }
    public int HeroLevel { get { return heroLevel; } }
    public Item[] Equipment { get { return equipment; } }
    public Item[] Inventory { get { return inventory; } }
    public Unit[] Unit { get { return unit; } }
    public int UnitKind
    {
        get
        {
            unitKind = 0;

            for (int i = 0; i < unitNum; i++)
            {
                if (unit[i].num != 0)
                    unitKind++;
            }
            return unitKind;
        }
    }
    public int[] Skill { get { return skill; } }
    public int[] Building { get { return building; } }
    public int[] Upgrade { get { return upgrade; } }
    public int Resource { get { return resource; } }

    public const int equipNum = 7;
    public const int invenNum = 16;
    public const int skillNum = 15;
    public const int unitNum = 5;
    public const int buildingNum = 7;

    public UserData(string newId, string newPw)
    {
        Id = newId;
        Pw = newPw;
        heroId = 1;
        heroLevel = 1;
        equipment = new Item[equipNum];
        inventory = new Item[invenNum];
        skill = new int[skillNum];
        unit = new Unit[unitNum];
        createUnit = new int[unitNum];
        attackUnit = new int[unitNum];
        building = new int[buildingNum];
        buildBuilding = 0;
        upgrade = new int[unitNum];
        resource = 0;
        castleState = (int)CastleState.Peace;

        for (int i = 0; i < equipNum; i++) { equipment[i] = new Item(0, 0); }
        for (int i = 0; i < invenNum; i++) { inventory[i] = new Item(0, 0); }
        for (int i = 0; i < skillNum; i++) { skill[i] = 0; }
        for (int i = 0; i < unitNum; i++) { unit[i] = new Unit(0, 0); }
        for (int i = 0; i < buildingNum; i++) { building[i] = 0; }
        for (int i = 0; i < unitNum; i++) { upgrade[i] = 0; }
}

    //레벨 변경
    public void SetLevel(int newLevel)
    {
        heroLevel = newLevel;
    }

    //클라이언트에서 미리 그곳에 아이템이 있는지 확인해서 데이터를 전송
    //장비 장착
    public void Equip(int itemId, int type)
    {
        equipment[type].Id = (byte) itemId;
    }

    //장비 해제
    public void UnEquip(int index)
    {
        equipment[index].Id = 0;
    }

    //아이템 획득
    public void AddItem(int Id)
    {
        int index = FindSlotWithId(Id);

        if(index != -1)
        {
            inventory[index].num++;
        }
        else
        {
            index = FindEmptySlot();

            if (index != -1)
            {
                inventory[index].Id = (byte) Id;
                inventory[index].num ++;
            }
        }
    }

    //아이템 빼기
    public void AbstractItem(int index)
    {
        if(index != 0)
            inventory[index].Id --;
    }

    public int FindEmptySlot()
    {
        for (int i = 0; i < invenNum; i++)
        {
            if(inventory[i].Id == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public int FindSlotWithId(int Id)
    {
        for(int i =0; i< invenNum; i++)
        {
            if(inventory[i].Id == Id)
            {
                return i;
            }
        }

        return -1;
    }

    //자원 사용
    public void UseResources (int amount)
    {
        resource -= amount;
    }

    //성 상태 변경
    public void ChangeCastleState(int index)
    {
        castleState = index;
    }

    //유닛숫자변경
    public void AddUnit(int unitId, int unitNum)
    {
        unit[unitId - 1].num += (byte) unitNum;
    }

    //유닛생산, 취소
    //유닛공격, 복귀
    //건물생산, 취소
}

[Serializable]
public class Item
{
    public byte Id;
    public byte num;

    public Item()
    {
        Id = 0;
        num = 0;
    }

    public Item(int newId, int newNum)
    {
        Id = (byte) newId;
        num = (byte) newNum;
    }
}