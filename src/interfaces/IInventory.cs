using ResultManager;
using System.Collections.Generic;
using IItemsTypes;

namespace IInventory
{
    public interface _IInventory {

        List<IItem> Items { get; }
        Result<IItem> AddItem(IItem item);
        Result<IItem> UseItem(IItem item);
        Result<IItem> SelectRandomItem(bool upgraded);
    }
}