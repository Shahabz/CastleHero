using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    Item[] equipments;
    Item[] items;
    int resource;
    int castleState;
    int[] buildings;
    int buildBuilding;
    Unit[] units;
    Unit[] createUnits;
    Unit[] attackUnits;

    public string ID { get { return Id; } }
    public string PW { get { return Pw; } }
    public int HeroId { get { return heroId; } }
    public int HeroLevel { get { return heroLevel; } }

    public const int equipNum = 7;
    public const int buildingNum = 6;
    public const int unitNum = 4;

    public UserData(string newId, string newPw)
    {
        Id = newId;
        Pw = newPw;
        heroId = 1;
        heroLevel = 1;
        equipments = new Item[equipNum];
        items = new Item[16];
        resource = 0;
        castleState = (int) CastleState.Peace;
        buildings = new int[buildingNum];
        buildBuilding = 0;
        units = new Unit[unitNum];
        createUnits = new Unit[unitNum];
        attackUnits = new Unit[unitNum];
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
        equipments[type].Id = itemId;
    }

    //장비 해제
    public void UnEquip(int index)
    {
        equipments[index].Id = 0;
    }

    //아이템 획득, 버리기
    public void AddItem(int itemId, int index)
    {
        items[index].Id = itemId;
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
    public void AddUnit(Unit unit)
    {
        int index = FindUnit(unit);

        if (index != -1)
        {
            units[index].num += unit.num;
        }
        else
        {
            index = FindEmptyUnitSlot();

            if (index != -1)
            {
                units[index].num = unit.num;
            }
        }
    }

    public int FindUnit(Unit unit)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].Id == unit.Id)
            {
                return i;
            }
        }

        return -1;
    }

    public int FindEmptyUnitSlot()
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].Id == 0)
            {
                return i;
            }
        }

        return -1;
    }

    //유닛생산, 취소
    //유닛공격, 복귀
    //건물생산, 취소
}

[Serializable]
public struct Unit
{
    public int Id;
    public int num;
}

[Serializable]
public struct Item
{
    public int Id;
    public int type;
}