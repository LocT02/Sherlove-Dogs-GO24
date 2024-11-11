using ResultManager;
using System.Collections.Generic;
using IItemsTypes;
using Godot.Collections;

namespace IInventory
{
    public interface _IInventory {

        List<IItem> Items { get; }
        Result<IItem> AddItem(IItem item);
        Result<IItem> RemoveItem(IItem item);
    }
}