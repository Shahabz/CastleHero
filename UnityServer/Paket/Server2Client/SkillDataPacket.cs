public class SkillDataPacket : IPacket<SkillData>
{
    public class SkillDataSerializer : Serializer
    {
        public bool Serialize(SkillData data)
        {
            bool ret = true;
            for (int i = 0; i < UserData.skillNum; i++)
            {
                ret &= Serialize(data.skillLevel[i]);
            }

            return ret;
        }

        public bool Deserialize(ref SkillData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte level = 0;

            for (int i = 0; i < UserData.skillNum; i++)
            {
                ret &= Deserialize(ref level);
                element.skillLevel[i] = level;
            }

            return ret;
        }
    }

    SkillData m_data;

    public SkillDataPacket(SkillData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public SkillDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        SkillDataSerializer serializer = new SkillDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        SkillDataSerializer serializer = new SkillDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public SkillData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.SkillData;
    }
}

public class SkillData
{
    public byte[] skillLevel;

    public SkillData()
    {
        skillLevel = new byte[UserData.skillNum];

        for (int i = 0; i < UserData.skillNum; i++) { skillLevel[i] = 0; }
    }

    public SkillData(int[] newSkillLevel)
    {
        skillLevel = new byte[UserData.skillNum];

        for (int i = 0; i < UserData.skillNum; i++)
        {
            skillLevel[i] = (byte)newSkillLevel[i];
        }
    }
}