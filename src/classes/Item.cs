using Godot;
using System;
using System.Collections.Generic;
using Result;

namespace ItemDataWIP
{
    public class Item
    {
        // Attributes
        public string Name { get; set; }
        public int Quantity { get; set; }

        // Constructor
        public Item(string name, int Quantity)
        {
            Name = name;
            Quantity = Quantity;
        }

        // method to apply item? probably in the individual item subclasses
    }
}