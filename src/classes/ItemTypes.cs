using IItems;
using Result;

namespace ItemTypes {
    public class ItemA : IItemTypes {
        // Attributes
        public string Name { get; } = "ItemA"; //Freebie or something

        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }
    }

    public class ItemB : IItemTypes {
        public string Name { get; } = "ItemB"; //Double points on the next attempt or something

        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }

    }
}