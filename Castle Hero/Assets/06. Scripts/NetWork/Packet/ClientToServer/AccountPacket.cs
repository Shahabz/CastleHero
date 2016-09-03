public class AccountPacket : IPacket<AccountData>
{
    public class AccountSerializer : Serializer
    {
        public bool Serialize(AccountData data)
        {
            bool ret = true;
            ret &= Serialize(data.Id);
            ret &= Serialize(".");
            ret &= Serialize(data.Pw);
            return ret;
        }

        public bool Deserialize(ref AccountData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            string total;
            ret &= Deserialize(out total, (int)GetDataSize());

            string[] str = total.Split('.');
            if (str.Length < 2)
            {
                return false;
            }

            element.Id = str[0];
            element.Pw = str[1];

            return ret;
        }
    }
    AccountData m_data;

    public AccountPacket(AccountData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public AccountPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        AccountSerializer serializer = new AccountSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        AccountSerializer serializer = new AccountSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public AccountData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ClientPacketId.Create;
    }
}
