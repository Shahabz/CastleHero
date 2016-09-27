using System.Text;

public class PlaceDataPacket : IPacket<Place[]>
{
    public class PlaceDataSerializer : Serializer
    {
        public bool Serialize(Place[] data)
        {
            bool ret = true;
            
            ret &= Serialize((short)data.Length);
            
            for (int i = 0; i < data.Length; i++)
            {
                ret &= Serialize(data[i].Type);
                ret &= Serialize(data[i].Position.X);
                ret &= Serialize(data[i].Position.Y);
                ret &= Serialize(data[i].Level);
                ret &= Serialize((byte) Encoding.Unicode.GetBytes(data[i].ID).Length);
                ret &= Serialize(data[i].ID);
            }

            return ret;
        }

        public bool Deserialize(ref Place[] element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            short posLength = 0;

            byte placeType = 0;
            short x = 0;
            short y = 0;
            byte level = 0;
            byte IdLength = 0;
            string Id;

            ret &= Deserialize(ref posLength);
            element = new Place[posLength];

            for (int i = 0; i < posLength; i++)
            {
                element[i] = new Place();
                ret &= Deserialize(ref placeType);
                ret &= Deserialize(ref x);
                ret &= Deserialize(ref y);
                ret &= Deserialize(ref level);
                ret &= Deserialize(ref IdLength);
                ret &= Deserialize(out Id, IdLength);

                element[i] = new Place(placeType, new Position(x, y), level, Id);
            }

            return ret;
        }
    }

    Place[] m_data;

    public PlaceDataPacket(Place[] data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public PlaceDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new Place[1000];
        PlaceDataSerializer serializer = new PlaceDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        PlaceDataSerializer serializer = new PlaceDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public Place[] GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.PlaceDataRequest;
    }
}