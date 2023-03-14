namespace App_ChatRoom.Services
{
    public interface ILocalStorage
    {
        Task Clear();
        Task<T> GetItem<T>(string key);
        Task RemoveItem(string key);
        Task SetItem<T>(string key, T value);
    }
}