using System.Collections.Generic;
using ResultManager;

namespace IMainWordGame {

    public interface _IMainWordGame {

        string CurrentWord { get; set; }
        string Category { get; set; }
        List<char> GuessedLetters { get; set; }
        List<char> CorrectLetters { get; set; }
        Result<Dictionary<string, string>> GetNewWord();
        Result<char[]> CheckGuess(string guess);
        Result<char[]> RevealLetters(List<int> indicesToReveal);
    }
}