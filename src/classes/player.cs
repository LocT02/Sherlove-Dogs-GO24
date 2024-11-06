using Godot;
using System;
using System.Collections.Generic;
using Result;
using InventoryManager;

namespace PlayerUser
{
    public class Player
    {
        // Attributes
        public int HP { get; set; }
        public int Score { get; set; }
        public Inventory _Inventory { get; set; }

        public Result<int> TakeDamage(int damage)
        {
            // fix logic // sample logic
            if (HP > damage) 
            {
                HP -= damage;
                return Result.Success(HP);
            }

            return Result.Failure("Not implemented");
        }


        // change score
        public Result<int> ChangeScore(int increment)
        {
            // sample logic fix later
            if (Score > 0)
            {
                Score += increment;
                return Result.Success(Score);
            }

            return Result.Failure("Not implemented");

        }





    }
}