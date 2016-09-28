public class PositionDataPacket : IPacket<Position>
{
    public class PositionDataSerializer : Serializer
    {
        public bool Serialize(Position data)
        {
            bool ret = true;

            ret &= Serialize((data.X * 1000) + data.Y);
            return ret;
        }

        public bool Deserialize(ref Position element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            int total = 0;

            ret &= Deserialize(ref total);
            element = new Position((short)(total / 1000), (short)(total % 1000));

            return ret;
        }
    }

    Position m_data;

    public PositionDataPacket(Position data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public PositionDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new Position(0, 0);
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

    public Position GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.EnemyUnitDataRequest;
    }
}