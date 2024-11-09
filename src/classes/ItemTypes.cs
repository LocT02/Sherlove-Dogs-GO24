using IItems;
using Result;

namespace ItemTypes {

    public class ItemA : IItemTypes {

        public string Name { get; }= "FreeAttempt"; // Sample Name

        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }
    }

    public class ItemB : IItemTypes {

        public string Name { get; } = "RandomLetters"; // Sample Name
        
        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }
    }

    public class ItemC : IItemTypes {

        public string Name { get; } = "RevealLetter"; // Sample Name

        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }
    }
}