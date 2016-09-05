public class HeroDataPacket : IPacket<HeroData>
{
    public class HeroDataSerializer : Serializer
    {
        public bool Serialize(HeroData data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(data.level);
            return ret;
        }

        public bool Deserialize(ref HeroData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte Id = 0;
            byte level = 0;

            ret &= Deserialize(ref Id);
            ret &= Deserialize(ref level);
            element.Id = Id;
            element.level = level;

            return ret;
        }
    }

    HeroData m_data;

    public HeroDataPacket(HeroData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public HeroDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new HeroData();
        HeroDataSerializer serializer = new HeroDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        HeroDataSerializer serializer = new HeroDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public HeroData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.HeroData;
    }
}

public class HeroData
{
    public byte Id;
    public byte level;

    public HeroData()
    {
        Id = 1;
        level = 1;
    }

    public HeroData(int newId, int newLevel)
    {
        Id = (byte)newId;
        level = (byte)newLevel;
    }
}
