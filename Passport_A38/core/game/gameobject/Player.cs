using System.Numerics;

namespace Passport_A38.core.game.gameobject;

public class Player : GameObject
{
    private const char character= '§';
    public override char Character => character;

    public int Score { get; set; }

    public Player(Vector2 pos) : base(pos)
    {
        
    }
}