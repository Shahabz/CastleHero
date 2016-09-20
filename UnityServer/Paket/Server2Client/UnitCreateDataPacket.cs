using System;

public class UnitCreateDataPacket : IPacket<UnitCreateData>
{
    public class UnitCreateDataSerializer : Serializer
    {

        public bool Serialize(UnitCreateData data)
        {
            bool ret = true;
            ret &= Serialize(data.year);
            ret &= Serialize(data.month);
            ret &= Serialize(data.day);
            ret &= Serialize(data.hour);
            ret &= Serialize(data.minute);
            ret &= Serialize(data.second);
            ret &= Serialize(data.kind);

            for(int i =0; i< data.kind; i++)
            {
                ret &= Serialize(data.unit[i].Id);
                ret &= Serialize(data.unit[i].num);
            }

            return ret;
        }

        public bool Deserialize(ref UnitCreateData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            short year = 0;
            byte month = 0;
            byte day = 0;
            byte hour = 0;
            byte minute = 0;
            byte second = 0;
            byte kind = 0;
            byte unitId = 0;
            byte unitNum = 0;
            
            ret &= Deserialize(ref year);
            ret &= Deserialize(ref month);
            ret &= Deserialize(ref day);
            ret &= Deserialize(ref hour);
            ret &= Deserialize(ref minute);
            ret &= Deserialize(ref second);
            ret &= Deserialize(ref kind);
            element.year = year;
            element.month = month;
            element.day = day;
            element.hour = hour;
            element.minute = minute;
            element.second = second;
            element.kind = kind;

            for (int i = 0; i < kind; i++)
            {
                ret &= Deserialize(ref unitId);
                ret &= Deserialize(ref unitNum);
                element.unit[i].Id = unitId;
                element.unit[i].num = unitNum;
            }

            return ret;
        }
    }

    UnitCreateData m_data;

    public UnitCreateDataPacket(UnitCreateData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitCreateDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitCreateData();
        UnitCreateDataSerializer serializer = new UnitCreateDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UnitCreateDataSerializer serializer = new UnitCreateDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public UnitCreateData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.Create;
    }
}

public class UnitCreateData
{
    public short year;
    public byte month;
    public byte day;
    public byte hour;
    public byte minute;
    public byte second;
    public byte kind;
    public Unit[] unit;

    public UnitCreateData()
    {
        year = 0;
        month = 0;
        day = 0;
        hour = 0;
        minute = 0;
        second = 0;
        kind = 0;
        unit = new Unit[kind];
    }

    public UnitCreateData(DateTime time, int newKind, Unit[] newUnit)
    {
        year = (short)time.Year;
        month = (byte)time.Month;
        day = (byte)time.Day;
        hour = (byte)time.Hour;
        minute = (byte)time.Minute;
        second = (byte)time.Second;
        kind = (byte)newKind;
        unit = newUnit;
    }
}