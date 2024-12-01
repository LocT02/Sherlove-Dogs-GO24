using ResultManager;
using IInventory;
using System.Collections.Generic;
using IItemsTypes;
using System;
using ItemTypes;
using Godot;

namespace InventoryManager {
    public class Inventory : _IInventory {
        public List<IItem> Items { get; private set; } = new();

        public Result<IItem> AddItem(IItem item) {
            if (item == null) {
                return Result.Failure<IItem>("Failed to add Item");
            }
            if (Items.Count < 3){    
                Items.Add(item);
            }
            
            return Result.Success(item);
        }

        private Result<IItem> RemoveItem(IItem item) {
            // shrug
            if (item != null && Items.Contains(item)) {
                Items.Remove(item);
                return Result.Success(item);
            }
            return Result.Failure<IItem>($"Failed to Remove Item: {item}");
        }

        public Result<IItem> UseItem(IItem item) {
            // Checks if item is in inventory
            // Applys Item
            // Removes Item
            if (item == null) {
                return Result.Failure<IItem>("Item is null, cannot use");
            }

            if (!Items.Contains(item)) {
                return Result.Failure<IItem>("Item not found in Inventory");
            }

            var applyResult = item.ApplyItem();

            if (applyResult.IsSuccess) {
                var removeResult = RemoveItem(item);
                return removeResult;
            }

            return Result.Failure<IItem>("Failed to Use Item");
        }

        public Result<IItem> SelectRandomItem(bool upgraded) {
            // Randomly selects and returns an Instance of that random Item

            // List of Items
            List<Type> itemTypes = new List<Type> {
                typeof(ItemA),
                typeof(ItemB),
                typeof(ItemC)
            };

            if (itemTypes.Count == 0) {
                return Result.Failure<IItem>("Somehow Failed to Read Item Classes");
            }

            Random rand = new();
            int randomIndex = rand.Next(itemTypes.Count);

            Type selectedItemType = itemTypes[randomIndex];

            IItem newItem = (IItem)Activator.CreateInstance(selectedItemType);

            if (upgraded) {
                newItem.Upgraded = true;
            }

            return Result.Success(newItem);

        }
    }
}