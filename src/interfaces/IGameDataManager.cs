using Godot;
using Godot.Collections;
using InventoryManager;
using ResultManager;

namespace IGameDataManager {

    public interface _IGameDataManager {

        int Hp { get; set; }
        int Score { get; set; }
        Inventory Inventory { get; }

        Result CreateNewSave();
        Result LoadSaveData();
        Result SaveGame();
        Result<int> ChangeHp(int increment);
        Result ChangeScore(int increment);
    }
}