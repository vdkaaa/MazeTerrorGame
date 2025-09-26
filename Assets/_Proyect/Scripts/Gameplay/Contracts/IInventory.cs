namespace Project.Gameplay.Player
{
    public interface IInventory
    {
        bool AddItem(string id, int amount = 1);
        bool UseItem(string id);
        int Count(string id);
    }
}
