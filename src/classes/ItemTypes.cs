using IItemsTypes;
using ResultManager;

namespace ItemTypes {

    public class ItemA : IItem {

        public string Name { get; }= "FreeAttempt"; // Sample Name

        public Result ApplyItem() {
            return Result.Failure($"Item: {Name} Application Not Implemented");
        }
    }

    public class ItemB : IItem {

        public string Name { get; } = "RandomLetters"; // Sample Name
        
        public Result ApplyItem() {
            return Result.Failure($"Item: {Name} Application Not Implemented");
        }
    }

    public class ItemC : IItem {

        public string Name { get; } = "RevealLetter"; // Sample Name

        public Result ApplyItem() {
            return Result.Failure($"Item: {Name} Application Not Implemented");
        }
    }
}