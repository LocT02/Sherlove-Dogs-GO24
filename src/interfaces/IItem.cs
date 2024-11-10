using Result;

namespace IItems
{
    // Need better names for namespace, class, and interface :P
    public interface IItemTypes {

        string Name { get; }
        Result<string> ApplyItem();
    }
}