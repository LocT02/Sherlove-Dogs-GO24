using System;
using System.Collections.Generic;
using Godot;
using InventoryManager;
using MainWordGameWIPNAME;
using Result;

namespace IGameManager {
    public interface IGameInstance {

        public Result<Error> SceneChanger(string scenePath);
        public Result<int> EndGame();


        
        
    }
}