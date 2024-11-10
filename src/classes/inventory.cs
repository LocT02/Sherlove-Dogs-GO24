using ResultManager;
using IInventory;
using System.Collections.Generic;
using IItemsTypes;

namespace InventoryManager {
    public class Inventory : _IInventory {
        public List<IItem> Items { get; private set; } = new();
        public int ItemCount = 0;

        public Result<IItem> AddItem(IItem item) {
            if (item != null) {
                Items.Add(item);
                return Result.Success(item);
            }
            return Result.Failure<IItem>("Failed to add Item");
        }

        public Result<IItem> RemoveItem(IItem item) {
            // shrug
            return Result.Failure<IItem>($"Failed to Remove Item: {item}");
        }
    }
}