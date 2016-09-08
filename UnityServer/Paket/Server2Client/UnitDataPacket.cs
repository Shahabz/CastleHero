using System;

public class UnitDataPacket : IPacket<UnitData[]>
{
    public class UnitDataSerializer : Serializer
    {
        public bool Serialize(UnitData[] data)
        {
            bool ret = true;

            for (int i = 0; i < data.Length; i++)
            {
                ret &= Serialize(data[i].unitKind);

                for (int j = 0; j < data[i].unitKind; j++)
                {
                    ret &= Serialize(data[i].unit[j].Id);
                    ret &= Serialize(data[i].unit[j].num);
                }
            }

            return ret;
        }

        public bool Deserialize(ref UnitData[] element)
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

            for (int i = 0; i < element.Length; i++)
            {
                ret &= Deserialize(ref unitKind);
                element[i].unitKind = unitKind;

                element[i].unit = new Unit[element[i].unitKind];

                for (int j = 0; j < element[i].unitKind; j++)
                {
                    ret &= Deserialize(ref Id);
                    ret &= Deserialize(ref num);
                    element[i].unit[j].Id = Id;
                    element[i].unit[j].num = num;
                }
            }

            return ret;
        }
    }

    UnitData[] m_data;

    public UnitDataPacket(UnitData[] data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitData[3];
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

    public UnitData[] GetData() // 데이터 얻기(수신용)
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
            unit[i].Id = 0; unit[i].num = 0;
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

[Serializable]
public class Unit
{
    public byte Id;
    public byte num;

    public Unit()
    {
        Id = 0;
        num = 0;
    }

    public Unit(Unit unit)
    {
        Id = unit.Id;
        num = unit.num;
    }
}