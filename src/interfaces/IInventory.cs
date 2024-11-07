using Result;
using ItemTypes;
using System.Collections.Generic;
using IItems;

namespace IInventory
{
    public interface _IInventory {
        Dictionary<IItemTypes, int> Items { get; }
        Result<IItemTypes> AddItem(IItemTypes item);
        Result<IItemTypes> RemoveItem(IItemTypes item);
    }
}