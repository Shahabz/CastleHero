public class ResourceDataPacket : IPacket<ResourceData>
{
    public class ResourceDataSerializer : Serializer
    {
        public bool Serialize(ResourceData data)
        {
            bool ret = true;
            ret &= Serialize(data.gold);
            return ret;
        }

        public bool Deserialize(ref ResourceData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte gold = 0;

            ret &= Deserialize(ref gold);
            element.gold = gold;

            return ret;
        }
    }

    ResourceData m_data;

    public ResourceDataPacket(ResourceData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public ResourceDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        ResourceDataSerializer serializer = new ResourceDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        ResourceDataSerializer serializer = new ResourceDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public ResourceData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.ResourceData;
    }
}

public class ResourceData
{
    public int gold;

    public ResourceData(int newGold)
    {
        gold = newGold;
    }
}
