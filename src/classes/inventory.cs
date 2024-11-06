using Result;
using ItemTypes;
using IInventory;
using System.Collections.Generic;
using IItems;

namespace InventoryManager {
    public class Inventory : _IInventory {
        public Dictionary<IItemTypes, int> Items { get; } = new Dictionary<IItemTypes, int>();

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