public class UpgradeDataPacket : IPacket<UpgradeData>
{
    public class UpgradeDataSerializer : Serializer
    {
        public bool Serialize(UpgradeData data)
        {
            bool ret = true;
            for (int i = 0; i < UserData.unitNum; i++)
            {
                ret &= Serialize(data.upgrade[i]);
            }

            return ret;
        }

        public bool Deserialize(ref UpgradeData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte level = 0;

            for (int i = 0; i < UserData.unitNum; i++)
            {
                ret &= Deserialize(ref level);
                element.upgrade[i] = level;
            }

            return ret;
        }
    }

    UpgradeData m_data;

    public UpgradeDataPacket(UpgradeData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UpgradeDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        UpgradeDataSerializer serializer = new UpgradeDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UpgradeDataSerializer serializer = new UpgradeDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public UpgradeData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.UpgradeData;
    }
}

public class UpgradeData
{
    public byte[] upgrade;

    public UpgradeData()
    {
        upgrade = new byte[UserData.unitNum];

        for (int i = 0; i < UserData.unitNum; i++) { upgrade[i] = 0; }
    }

    public UpgradeData(int[] newUpgrade)
    {
        upgrade = new byte[UserData.unitNum];

        for (int i = 0; i < UserData.unitNum; i++)
        {
            upgrade[i] = (byte) newUpgrade[i];
        }
    }
}