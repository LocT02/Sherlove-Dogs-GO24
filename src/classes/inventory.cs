using Result;
using IInventory;
using System.Collections.Generic;
using IItems;
using System;
using Godot.Collections;

namespace InventoryManager {
    public class Inventory : _IInventory {
        public List<IItemTypes> Items = new List<IItemTypes>();
        public int ItemCount = 0;
        public Result<IItemTypes> AddItem(IItemTypes item) {

            // shrug
            return Result<IItemTypes>.Failure("Item Not implemented", item);
        }

        public Result<IItemTypes> RemoveItem(IItemTypes item) {
            // shrug
            return Result<IItemTypes>.Failure("Item Not implemented", item);
        }
    }
}