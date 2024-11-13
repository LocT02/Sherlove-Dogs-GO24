using Godot;
using System.Collections.Generic;

public partial class Bone : Area2D
{
    public int Points { get; private set; }
    private Texture2D texture;
    private CollisionShape2D coll;
    private readonly static Dictionary<int, (Texture2D texture, int points)> boneData = new Dictionary<int, (Texture2D, int)>
    {
        { 0, ((Texture2D)GD.Load("res://Assets/Icons/Bone0.svg"), 50) },
        { 1, ((Texture2D)GD.Load("res://Assets/Icons/Bone1.svg"), -50) },
        { 2, ((Texture2D)GD.Load("res://Assets/Icons/Bone2.svg"), 25) }
    };

    public void Initialize(int boneType)
    {
        if (boneData.ContainsKey(boneType))
        {
            (texture, Points) = boneData[boneType];
            var sprite = new TextureRect { Texture = texture };
            sprite.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            AddChild(sprite);
        }
        coll = new CollisionShape2D();
        var shape = new RectangleShape2D();
        shape.Size = new Vector2(64,64);
        coll.Shape = shape;
        AddChild(coll);
        this.BodyEntered += OnBodyEntered;
        
    }

    public override void _Process(double delta)
    {
        Position += Vector2.Down * 150 * (float)delta; // Adjust speed as needed

        if (Position.Y > GetViewportRect().Size.Y)
        {
            QueueFree();  // Remove bone if it falls out of view
        }
    }

    public static void OnBodyEntered(Node2D body){
        GD.Print(body.GetClass());
        if(!body.HasMethod("OnBoneCollected")) return;

        body.Call("OnBoneCollected");
    }
}
