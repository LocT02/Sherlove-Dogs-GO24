using Result;
using ItemTypes;
using System.Collections.Generic;
using IItems;
using Godot.Collections;

namespace IInventory
{
    public interface _IInventory {

        List<IItemTypes> Items { get; }
        Result<IItemTypes> AddItem(IItemTypes item);
        Result<IItemTypes> RemoveItem(IItemTypes item);
    }
}