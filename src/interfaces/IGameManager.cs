using ResultManager;

namespace IGameManager {
    public interface IGameInstance {
        
        string GameState { get; set; }
        Result SceneChanger(string scenePath);
        Result EndGame();
        Result StartGame();
        Result<char[]> GuessAttempt(string guess);
    }
}