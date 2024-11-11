using ResultManager;

namespace IItemsTypes
{
    // Need better names for namespace, class, and interface :P
    public interface IItem {

        string Name { get; }
        Result ApplyItem();
    }
}