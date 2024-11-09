using IItems;
using Result;

namespace ItemTypes {
    public class ItemA : IItemTypes {
        // Attributes
        public string Name { get; } = "ItemA"; //Freebie or something

        public ItemA(string name){
            this.Name=name;
        }
        public ItemA(){}

        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }
    }

    public class ItemB : IItemTypes {
        public string Name { get; } = "ItemB"; //Double points on the next attempt or something
        
        public ItemB(string name){
            this.Name=name;
        }
        public ItemB(){}
        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }

    }
    public class ItemC : IItemTypes {
        public string Name { get; } = "ItemC"; //Double points on the next attempt or something
        
        public ItemC(string name){
            this.Name=name;
        }
        public ItemC(){}
        public Result<string> ApplyItem() {
            return Result<string>.Failure("Apply Not Implemented", Name);
        }

    }
}