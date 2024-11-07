using Result;
using InventoryManager;
using MainWordGameWIPNAME;
using IGameManager;

namespace GameManager {
    public class GameInstance : IGameInstance {
        // Attributes
        public int HP { get; set; }
        public int Score { get; set; }
        public Inventory Inventory { get; set; }

        public Result<string> StartGame() {
            //Instantiate inventory, mainwordgame (possibly move this into game manager?),
            //whatever else we need for assets or variables
            //call GetWord()
            //switch scenes??
            return Result<string>.Success("Successfully started game with word: currentWord");
            
        }
        
        //function to change scenes

        //Hp check function

        //end game function

        public Result<int> TakeDamage(int damage) {
            // fix logic // sample logic
            if (HP > damage) {
                HP -= damage;
                return Result<int>.Success(HP);
            }

            return Result<int>.Failure("Take Damage Not implemented");
        }


        // change score
        public Result<int> ChangeScore(int increment) {
            // sample logic fix later
            if (Score > 0) {
                Score += increment;
                return Result<int>.Success(Score);
            }

            return Result<int>.Failure("Change Score Not implemented");

        }

    }
}