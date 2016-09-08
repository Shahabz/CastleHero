using System.Collections.Generic;

class ItemDatabase
{
    public ItemDatabase instance;

    private ItemDatabase Instance()
    {
        if(instance == null)
        {
            instance = new ItemDatabase();
        }

        return instance;
    }
}
