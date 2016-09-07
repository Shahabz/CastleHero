public class StateDataPacket : IPacket<StateData>
{
    public class StateDataSerializer : Serializer
    {
        public bool Serialize(StateData data)
        {
            bool ret = true;
            ret &= Serialize(data.heroState);
            ret &= Serialize(data.castleState);
            return ret;
        }

        public bool Deserialize(ref StateData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte heroData = 0;
            byte castleData = 0;

            ret &= Deserialize(ref heroData);
            ret &= Deserialize(ref castleData);
            element.heroState = heroData;
            element.castleState = castleData;

            return ret;
        }
    }

    StateData m_data;

    public StateDataPacket(StateData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public StateDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new StateData();
        StateDataSerializer serializer = new StateDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        StateDataSerializer serializer = new StateDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public StateData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.StateData;
    }
}

public class StateData
{
    public byte heroState;
    public byte castleState;

    public StateData()
    {
        heroState = 0;
        castleState = 0;
    }

    public StateData(byte hState, byte cState)
    {
        heroState = hState;
        castleState = cState;
    }
}
