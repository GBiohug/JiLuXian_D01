using System.Collections.Generic;

//���ϵͳ�ű�
public class Inventory : SingletonMono<Inventory>
{
    public List<Item> inventoryItems = new List<Item>();//��������Ʒ�б�
}
