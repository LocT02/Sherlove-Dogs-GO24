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
        public int hp { get; set; }
        public int score { get; set; }
        public inventory inventoryData { get; set; }

        public Result<int> takeDamage(int damage)
        {
            // fix logic // sample logic
            if (hp > damage) 
            {
                hp -= damage;
                return Result.Success(hp);
            }

            return Result.Failure("Not implemented");
        }


        // change score
        public Result<int> changeScore(int increment)
        {
            // sample logic fix later
            if (score > 0)
            {
                score += increment;
                return Result.Success(score);
            }

            return Result.Failure("Not implemented");

        }





    }
}