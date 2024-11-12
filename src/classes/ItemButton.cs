using Godot;
using ICustomButton;
using IItemsTypes;

namespace CustomButton {
    public partial class ItemButton : Button, IItemButton {

        public IItem AttachedItem { get; set;}

    }
}