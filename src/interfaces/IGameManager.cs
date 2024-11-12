using GameData;
using MainWordGameWIPNAME;
using ResultManager;

namespace IGameManager {
    public interface IGameInstance {
        
        string GameState { get; set; }
        GameDataManager gameData { get; }
        MainWordGame mainWordGame { get; }
        Result SceneChanger(string scenePath);
        Result EndGame();
        Result StartGame();
        Result<char[]> GuessAttempt(string guess);
        Result MidGameSave();
    }
}