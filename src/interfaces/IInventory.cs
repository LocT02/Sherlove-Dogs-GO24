using Result;
using ItemTypes;
using System.Collections.Generic;
using IItems;
using Godot.Collections;

namespace IInventory
{
    public interface _IInventory {
        Result<IItemTypes> AddItem(IItemTypes item);
        Result<IItemTypes> RemoveItem(IItemTypes item);
    }
}