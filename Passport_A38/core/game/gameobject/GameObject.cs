using System.Numerics;

namespace Passport_A38.core.game.gameobject;

public abstract class GameObject
{
    public Vector2 Pos { get; set; }

    /*
     * The character of the object.
     * An underscore is used, if the object should not be rendered in
     */
    public abstract char Character { get; }

    protected GameObject(Vector2 pos)
    {
        Pos = pos;
    }
}