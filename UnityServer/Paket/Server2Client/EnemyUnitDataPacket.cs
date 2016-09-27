public class EnemyUnitDataPacket : IPacket<EnemyUnitData>
{
    public class EnemyUnitDataSerializer : Serializer
    {
        public bool Serialize(EnemyUnitData data)
        {
            bool ret = true;

            for (int i = 0; i < UserData.buildingNum; i++)
            {
                ret &= Serialize(data.building[i]);
            }

            return ret;
        }

        public bool Deserialize(ref EnemyUnitData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte level = 0;

            for (int i = 0; i < UserData.buildingNum; i++)
            {
                ret &= Deserialize(ref level);
                element.building[i] = level;
            }

            return ret;
        }
    }

    EnemyUnitData m_data;

    public EnemyUnitDataPacket(EnemyUnitData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public EnemyUnitDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new EnemyUnitData();
        EnemyUnitDataSerializer serializer = new EnemyUnitDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        EnemyUnitDataSerializer serializer = new EnemyUnitDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public EnemyUnitData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.EnemyUnitData;
    }
}

public class EnemyUnitData
{
    public byte[] building;

    public EnemyUnitData()
    {
        building = new byte[UserData.buildingNum];

        for (int i = 0; i < UserData.buildingNum; i++) { building[i] = 0; }
    }

    public EnemyUnitData(int[] newBuilding)
    {
        building = new byte[UserData.buildingNum];

        for (int i = 0; i < UserData.buildingNum; i++)
        {
            building[i] = (byte) newBuilding[i];
        }
    }
}