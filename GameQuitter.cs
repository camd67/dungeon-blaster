using Godot;

namespace dungeonblaster
{
    /// <summary>
    /// Quits the game no matter what, after holding the button for a bit
    /// </summary>
    public class GameQuitter : Node
    {
        [Export]
        private float quitHoldTime = 3f;

        private float currentHoldTime;
        private Label quitTimer;

        public override void _Ready()
        {
            quitTimer = GetNode<Label>("QuitTimer");
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("quit"))
            {
                currentHoldTime += delta;
                
                quitTimer.Visible = true;
                quitTimer.Text = $"Quit - {quitHoldTime - currentHoldTime}";

                if (currentHoldTime >= quitHoldTime)
                {
                    GetTree().Quit();
                }
            }
            else
            {
                currentHoldTime = 0;
                quitTimer.Visible = false;
            }
        }
    }
}
