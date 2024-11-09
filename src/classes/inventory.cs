using Result;
using IInventory;
using System.Collections.Generic;
using IItems;
using System;
using Godot.Collections;

namespace InventoryManager {
    public class Inventory : _IInventory {
        public List<IItemTypes> Items { get; private set; } = new();
        public int ItemCount = 0;
        public Result<IItemTypes> AddItem(IItemTypes item) {
            if (item != null) {
                Items.Add(item);
                return Result<IItemTypes>.Success(item, $"Successfully Added Item: {item}");
            }
            return Result<IItemTypes>.Failure("Failed To Add Item", item);
        }

        public Result<IItemTypes> RemoveItem(IItemTypes item) {
            // shrug
            return Result<IItemTypes>.Failure("Item Not implemented", item);
        }
    }
}