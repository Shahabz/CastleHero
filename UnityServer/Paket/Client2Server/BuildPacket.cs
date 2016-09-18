using System;

public class BuildPacket : IPacket<Build>
{
    public class BuildDataSerializer : Serializer
    {

        public bool Serialize(Build data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            return ret;
        }

        public bool Deserialize(ref Build element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte Id = 0;

            ret &= Deserialize(ref Id);
            element.Id = Id;

            return ret;
        }
    }

    Build m_data;

	public BuildPacket(Build data) // 데이터로 초기화(송신용)
	{
		m_data = data;
	}

	public BuildPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
	{
        m_data = new Build();
		BuildDataSerializer serializer = new BuildDataSerializer();
		serializer.SetDeserializedData(data);
		serializer.Deserialize(ref m_data);
	}

	public byte[] GetPacketData() // 바이트형 패킷(송신용)
	{
		BuildDataSerializer serializer = new BuildDataSerializer();
		serializer.Serialize(m_data);
		return serializer.GetSerializedData();
	}

	public Build GetData() // 데이터 얻기(수신용)
	{
		return m_data;
	}

	public int GetPacketId()
	{
		return (int) ClientPacketId.Create;
	}
}

public class Build
{
    public byte Id;
    
    public Build()
    {
        Id = 0;
    }

    public Build(int newId)
    {
        Id = (byte)newId;
    }
}