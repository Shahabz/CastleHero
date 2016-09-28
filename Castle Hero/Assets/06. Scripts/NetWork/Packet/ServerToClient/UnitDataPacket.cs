using System;

public class UnitDataPacket : IPacket<UnitData>
{
    public class UnitDataSerializer : Serializer
    {
        public bool Serialize(UnitData data)
        {
            bool ret = true;

            ret &= Serialize(data.unitKind);

            for (int i = 0; i < data.unitKind; i++)
            {
                ret &= Serialize(data.unit[i].Id);
                ret &= Serialize(data.unit[i].num);
            }

            return ret;
        }

        public bool Deserialize(ref UnitData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte unitKind = 0;
            byte Id = 0;
            byte num = 0;

            ret &= Deserialize(ref unitKind);
            element.unitKind = unitKind;

            element.unit = new Unit[unitKind];

            for (int i = 0; i < element.unitKind; i++)
            {
                ret &= Deserialize(ref Id);
                ret &= Deserialize(ref num);
                element.unit[i] = new Unit(Id, num);
            }

            return ret;
        }
    }

    UnitData m_data;

    public UnitDataPacket(UnitData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitData();
        UnitDataSerializer serializer = new UnitDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UnitDataSerializer serializer = new UnitDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public UnitData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.UnitData;
    }
}

public class UnitData
{
    public byte unitKind;
    public Unit[] unit;

    public UnitData()
    {
        unitKind = 0;
        unit = new Unit[unitKind];

        for (int i = 0; i < unitKind; i++)
        {
            unit[i] = new Unit((int)UnitId.None, 0);
        }
    }

    public UnitData(int newUnitKind, Unit[] newUnit)
    {
        unitKind = (byte)newUnitKind;
        unit = new Unit[unitKind];

        for (int i = 0; i < unitKind; i++)
        {
            unit[i] = new Unit(newUnit[i]);
        }
    }
}

public class Unit
{
    public byte Id;
    public byte num;

    public Unit()
    {
        Id = (int)UnitId.None;
        num = 0;
    }

    public Unit(int newId, int newNum)
    {
        Id = (byte)newId;
        num = (byte)newNum;
    }

    public Unit(Unit unit)
    {
        Id = unit.Id;
        num = unit.num;
    }
}