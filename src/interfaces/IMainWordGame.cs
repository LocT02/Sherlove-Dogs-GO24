using System;
using System.Collections.Generic;
using ResultManager;

namespace IMainWordGame {

    public interface _IMainWordGame {

        List<char> GuessedLetters { get; set; }

        Result GetWord();

        Result CheckGuess(string guess);

        Result<char> RevealLetter();
    }
}