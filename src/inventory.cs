using Godot;
using System;
using System.Collections.Generic;
using Result;

namespace inventory 
{
    public class inventory
    {
        public List<Item> items;

        public Result<Item> AddItem(Item item)
        {
            if (item = null)
            {
                return Result<Item>.Failure('Item is Null.');
            }


        }

        public Result<Item> RemoveItem(Item item)
        {
            
        }
    }
}