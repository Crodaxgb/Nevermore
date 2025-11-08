namespace NevermoreStudios.GameState
{
    //If a script wants to be a state manager, it needs to implement this interface
    public interface IGameStateManager
    {
        object State { get; }
    }

    public interface IGameStateManager<TState> : IGameStateManager
    {
        TState State { get; }
    }
}
