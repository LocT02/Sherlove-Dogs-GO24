using Godot;
using System;
using System.Collections.Generic;
using Result;
using inventory;

namespace player
{
    public class player
    {
        // Attributes
        prublic int hp { get; set; }
        public int score { get; set; }
        public inventory inventoryData { get; set; }

        public Result takeDamage(int damage)
        {
            if (hp > damage) 
            {
                hp -= damage;
                return Result.Success;
            }

            return Result.Failure("idk some issue?");
        }





    }
}