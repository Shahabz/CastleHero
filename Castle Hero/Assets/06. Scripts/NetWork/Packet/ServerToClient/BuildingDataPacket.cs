public class BuildingDataPacket : IPacket<BuildingData>
{
    public class BuildingDataSerializer : Serializer
    {
        public bool Serialize(BuildingData data)
        {
            bool ret = true;

            for(int i =0; i< BuildingData.buildingMaxNum; i++)
            {
                ret &= Serialize(data.buildingLevel[i]);
            }

            return ret;
        }

        public bool Deserialize(ref BuildingData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            short level = 0;

            for(int i=0; i< BuildingData.buildingMaxNum; i++)
            {
                ret &= Deserialize(ref level);
                element.buildingLevel[i] = level;
            }

            return ret;
        }
    }

    BuildingData m_data;

    public BuildingDataPacket(BuildingData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public BuildingDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        BuildingDataSerializer serializer = new BuildingDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        BuildingDataSerializer serializer = new BuildingDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public BuildingData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.BuildingData;
    }
}
