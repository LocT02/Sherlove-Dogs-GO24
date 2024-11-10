using System.Collections.Generic;
using ResultManager;

namespace IMainWordGame {

    public interface _IMainWordGame {

        string CurrentWord { get; set; }
        string Category { get; set; }
        List<char> GuessedLetters { get; set; }
        Result GetNewWord();
        Result CheckGuess(string guess);
        Result<int> CalculatePoints();
        Result<int> CalculateDamage();
        Result<char> RevealLetter();
    }
}