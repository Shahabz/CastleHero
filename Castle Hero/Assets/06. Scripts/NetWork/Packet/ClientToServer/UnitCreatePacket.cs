public class UnitCreatePacket : IPacket<UnitCreate>
{
    public class UnitCreateDataSerializer : Serializer
    {

        public bool Serialize(UnitCreate data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(data.num);
            return ret;
        }

        public bool Deserialize(ref UnitCreate element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte Id = 0;
            byte num = 0;

            ret &= Deserialize(ref Id);
            ret &= Deserialize(ref num);
            element.Id = Id;
            element.num = num;

            return ret;
        }
    }

    UnitCreate m_data;

    public UnitCreatePacket(UnitCreate data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public UnitCreatePacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new UnitCreate();
        UnitCreateDataSerializer serializer = new UnitCreateDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        UnitCreateDataSerializer serializer = new UnitCreateDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public UnitCreate GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.UnitCreate;
    }
}

public class UnitCreate
{
    public byte Id;
    public byte num;

    public UnitCreate()
    {
        Id = 0;
        num = 0;
    }

    public UnitCreate(int newId, int newNum)
    {
        Id = (byte)newId;
        num = (byte)newNum;
    }
}