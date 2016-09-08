public class ItemDataPacket : IPacket<ItemData>
{
    public class ItemDataSerializer : Serializer
    {
        public bool Serialize(ItemData data)
        {
            bool ret = true;
            for (int i = 0; i < DataManager.equipNum; i++)
            {
                ret &= Serialize((byte)data.equipment[i]);
            }

            for (int i = 0; i < DataManager.invenNum; i++)
            {
                ret &= Serialize((byte)data.inventoryId[i]);
                ret &= Serialize((byte)data.inventoryNum[i]);
            }

            return ret;
        }

        public bool Deserialize(ref ItemData element)
        {
            if (GetDataSize() == 0)
            {
                // 데이터가 설정되지 않았다.
                return false;
            }

            bool ret = true;
            byte item = 0;

            for (int i = 0; i < DataManager.equipNum; i++)
            {
                ret &= Deserialize(ref item);
                element.equipment[i] = item;
            }

            for (int i = 0; i < DataManager.invenNum; i++)
            {
                ret &= Deserialize(ref item);
                element.inventoryId[i] = item;
                ret &= Deserialize(ref item);
                element.inventoryNum[i] = item;
            }

            return ret;
        }
    }

    ItemData m_data;

    public ItemDataPacket(ItemData data) // 데이터로 초기화(송신용)
    {
        m_data = data;
    }

    public ItemDataPacket(byte[] data) // 패킷을 데이터로 변환(수신용)
    {
        m_data = new ItemData();
        ItemDataSerializer serializer = new ItemDataSerializer();
        serializer.SetDeserializedData(data);
        serializer.Deserialize(ref m_data);
    }

    public byte[] GetPacketData() // 바이트형 패킷(송신용)
    {
        ItemDataSerializer serializer = new ItemDataSerializer();
        serializer.Serialize(m_data);
        return serializer.GetSerializedData();
    }

    public ItemData GetData() // 데이터 얻기(수신용)
    {
        return m_data;
    }

    public int GetPacketId()
    {
        return (int)ServerPacketId.ItemData;
    }
}

public class ItemData
{
    public int[] equipment;
    public int[] inventoryId;
    public int[] inventoryNum;

    public ItemData()
    {
        equipment = new int[DataManager.equipNum];
        inventoryId = new int[DataManager.invenNum];
        inventoryNum = new int[DataManager.invenNum];

        for (int i = 0; i < DataManager.equipNum; i++) { equipment[i] = 0; }
        for (int i = 0; i < DataManager.invenNum; i++) { inventoryId[i] = 0; inventoryNum[i] = 0; }
    }

    public ItemData(int[] newEquipment, int[] newInventory, int[] newInventoryNum)
    {
        equipment = newEquipment;
        inventoryId = newInventory;
        inventoryNum = newInventoryNum;
    }
}
