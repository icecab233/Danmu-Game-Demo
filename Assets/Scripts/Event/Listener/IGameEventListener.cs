namespace DanmuGame.events
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}