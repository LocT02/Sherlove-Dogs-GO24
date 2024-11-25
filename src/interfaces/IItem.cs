using ResultManager;

namespace IItemsTypes
{
    // Need better names for namespace, class, and interface :P
    public interface IItem {

        string Name { get; }
        bool Upgraded { get; set; }
        string[] ImgFilePath { get; set; }
        Result<char[]> ApplyItem();

    }
}