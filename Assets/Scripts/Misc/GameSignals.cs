using Zenject;

namespace TicTacToe3D
{
    public class BadgeSpawned : Signal<BadgeSpawned, BadgeModel>
    { }

    public class ActivePlayerChanged : Signal<ActivePlayerChanged, Player>
    { }
    
    public class PlayerWon : Signal<PlayerWon, Player>
    { }
}