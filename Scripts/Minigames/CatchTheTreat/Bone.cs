using Godot;
using System.Collections.Generic;

public partial class Bone : Area2D
{
    public int Points { get; private set; }
    private int speed = 150;

    // References to the child nodes
    private TextureRect sprite;
    private CollisionShape2D collisionShape;
    public int boneTypeForSFX;

    // Dictionary to hold different textures and point values
    private static Dictionary<int, (Texture2D texture, int points)> BoneTypes;

    public override void _Ready()
    {
    // Defer the setup to ensure the node is fully initialized

        BoneTypes = new Dictionary<int, (Texture2D, int)>
        {
            { 0, ((Texture2D)GD.Load("res://Assets/Icons/Biscuit_Gold.svg"), 50) },
            { 1, ((Texture2D)GD.Load("res://Assets/Icons/Tomato.svg"), -50) },
            { 2, ((Texture2D)GD.Load("res://Assets/Icons/Biscuit_White.svg"), 25) }
        };
        sprite = GetNode<TextureRect>("TextureRect");
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        // Setup signal for when the player collides with this bone
        this.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += Vector2.Down * speed * (float)delta;

        if (Position.Y > GetViewportRect().Size.Y)
        {
            QueueFree();  // Remove bone if it falls out of view
        }
    }
    public void Initialize(int boneType, Vector2 boneSize)
    {
        
        if (BoneTypes.ContainsKey(boneType))
        {
            var boneData = BoneTypes[boneType];
            boneTypeForSFX = boneType; // Grab bone type data to play SFX based on bone type
            
            if (boneData.texture == null)
            {
                GD.PrintErr("Texture not loaded correctly for bone type: " + boneType);
                return;
            }

            sprite.Texture = boneData.texture;
            Points = boneData.points;

            sprite.StretchMode = TextureRect.StretchModeEnum.Scale;
            sprite.Size = boneSize;
            if (collisionShape.Shape is RectangleShape2D rectShape)
            {
                rectShape.Size = new Vector2(boneSize.X / 2, boneSize.Y);
                collisionShape.Position = new Vector2(boneSize.X / 2, boneSize.Y / 2);// Update collision shape dimensions
            }
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is CTBPlayer)  // Replace 'Player' with the actual class name for the player
        {
            // Signal or directly modify score
            var player = body as CTBPlayer;
            player.Call("OnBoneCollected", this);
        }
    }

    public void SetSpeed(int speed){
        this.speed = speed;
    }
}
