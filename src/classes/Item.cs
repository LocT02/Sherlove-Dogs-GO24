using Godot;
using System;
using System.Collections.Generic;
using Result;

namespace ItemData
{
    public class Item
    {
        // Attributes
        public string Name { get; set; }
        public int Quantity { get; set; }

        // Constructor
        public Item(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        // method to apply item? probably in the individual item subclasses
    }
}