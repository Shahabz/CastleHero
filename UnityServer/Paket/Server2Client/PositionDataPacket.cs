using System;

public class PositionDataPacket : IPacket<PositionData>
{
    public class PositionDataSerializer : Serializer
    {

        public bool Serialize(PositionData data)
        {
            bool ret = true;
            ret &= Serialize(data.xPos);
            ret &= Serialize(data.yPos);

            return ret;
        }

        public bool Deserialize(ref PositionData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            int xPos = 0;
            int yPos = 0;

            ret &= Deserialize(ref xPos);
            ret &= Deserialize(ref yPos);
            element.xPos = xPos;
            element.yPos = yPos;

            return ret;
        }
    }

    PositionData m_data;

    public PositionDataPacket(PositionData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public PositionDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new PositionData();
        PositionDataSerializer serializer = new PositionDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        PositionDataSerializer serializer = new PositionDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public PositionData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.Create;
    }
}

public class PositionData
{
    public int xPos;
    public int yPos;

    public PositionData()
    {
        xPos = 0;
        yPos = 0;
    }

    public PositionData(int x, int y)
    {
        xPos = x;
        yPos = y;
    }
}