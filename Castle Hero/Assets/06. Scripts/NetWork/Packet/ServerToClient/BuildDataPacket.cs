using System;

public class BuildDataPacket : IPacket<BuildData>
{
    public class BuildDataSerializer : Serializer
    {

        public bool Serialize(BuildData data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(data.year);
            ret &= Serialize(data.month);
            ret &= Serialize(data.day);
            ret &= Serialize(data.hour);
            ret &= Serialize(data.minute);
            ret &= Serialize(data.second);
            return ret;
        }

        public bool Deserialize(ref BuildData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte Id = 0;
            short year = 0;
            byte month = 0;
            byte day = 0;
            byte hour = 0;
            byte minute = 0;
            byte second = 0;

            ret &= Deserialize(ref Id);
            ret &= Deserialize(ref year);
            ret &= Deserialize(ref month);
            ret &= Deserialize(ref day);
            ret &= Deserialize(ref hour);
            ret &= Deserialize(ref minute);
            ret &= Deserialize(ref second);
            element.Id = Id;
            element.year = year;
            element.month = month;
            element.day = day;
            element.hour = hour;
            element.minute = minute;
            element.second = second;

            return ret;
        }
    }

    BuildData m_data;

    public BuildDataPacket(BuildData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public BuildDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new BuildData();
        BuildDataSerializer serializer = new BuildDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        BuildDataSerializer serializer = new BuildDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public BuildData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.Create;
    }
}

public class BuildData
{
    public byte Id;
    public short year;
    public byte month;
    public byte day;
    public byte hour;
    public byte minute;
    public byte second;

    public BuildData()
    {
        Id = 0;
        year = 0;
        month = 0;
        day = 0;
        hour = 0;
        minute = 0;
        second = 0;
    }

    public BuildData(int newId, DateTime time)
    {
        Id = (byte)newId;
        year = (short)time.Year;
        month = (byte)time.Month;
        day = (byte)time.Day;
        hour = (byte)time.Hour;
        minute = (byte)time.Minute;
        second = (byte)time.Second;
    }
}