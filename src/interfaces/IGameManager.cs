using System;
using System.Collections.Generic;
using Godot;
using InventoryManager;
using MainWordGameWIPNAME;
using ResultManager;

namespace IGameManager {
    public interface IGameInstance {
        
        string GameState { get; set; }
        Result SceneChanger(string scenePath);
        Result EndGame();
        
    }
}