public class ItemDataPacket : IPacket<ItemData>
{
    public class ItemDataSerializer : Serializer
    {
        public bool Serialize(ItemData data)
        {
            bool ret = true;
            for (int i = 0; i < DataManager.equipNum; i++)
            {
                ret &= Serialize(data.equipment[i].Id);
                ret &= Serialize(data.equipment[i].num);
            }

            for (int i = 0; i < DataManager.invenNum; i++)
            {
                ret &= Serialize(data.inventory[i].Id);
                ret &= Serialize(data.inventory[i].num);
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
            Item item = new Item();

            for (int i = 0; i < DataManager.equipNum; i++)
            {
                ret &= Deserialize(ref item.Id);
                ret &= Deserialize(ref item.num);
                element.equipment[i] = item;
            }

            for (int i = 0; i < DataManager.invenNum; i++)
            {
                ret &= Deserialize(ref item.Id);
                ret &= Deserialize(ref item.num);
                element.inventory[i] = item;
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
    public Item[] equipment;
    public Item[] inventory;

    public ItemData()
    {
        equipment = new Item[DataManager.equipNum];
        inventory = new Item[DataManager.invenNum];

        for (int i = 0; i < DataManager.equipNum; i++) { equipment[i] = new Item(0, 0); }
        for (int i = 0; i < DataManager.invenNum; i++) { inventory[i] = new Item(0, 0); }
    }

    public ItemData(Item[] newEquipment, Item[] newInventory)
    {
        equipment = newEquipment;
        inventory = newInventory;
    }
}

public class Item
{
    public byte Id;
    public byte num;

    public Item()
    {
        Id = 0;
        num = 0;
    }

    public Item(int newId, int newNum)
    {
        Id = (byte)newId;
        num = (byte)newNum;
    }
}