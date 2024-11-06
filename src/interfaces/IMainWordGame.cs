using System;
using System.Collections.Generic;
using Result;

namespace IMainWordGame {

    public interface _IMainWordGame {

        List<char> GuessedLetters { get; set; }

        Result<Tuple<string,string>> GetWord();

        Result<string> CheckGuess(string guess);

        Result<char> RevealLetter();
    }
}