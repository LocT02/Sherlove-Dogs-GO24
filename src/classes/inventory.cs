using Godot;
using System;
using System.Collections.Generic;
using Result;
using ItemData;

namespace InventoryManager
{
    public class Inventory
    {
        public List<Item> Items;

        public Result<Item> AddItem(Item item)
        {
            if (item == null)
            {
                return Result<Item>.Failure("Item is Null.");
            }

            // shrug
            return Result<Item>.Failure("Not implemented");
        }

        public Result<Item> RemoveItem(Item item)
        {
            // shrug
            return Result<Item>.Failure("Not implemented");
            
        }
    }
}