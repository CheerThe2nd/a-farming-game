using Godot;

public partial class World : Node3D
{
    Node3D dirtPatch;

    public override void _Ready()
    {
        dirtPatch = GetNode<Node3D>("/root/World/DirtPatch");
    }

    public override void _Process(double delta)
    {
        // TODO: move this somewhere else
        if (Input.IsActionJustPressed("escape"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else
            {
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
    }
}
