using Godot;

namespace dungeonblaster.Player
{
    public class Player : KinematicBody
    {
        [Export]
        private float maxSpeed = 20f;

        [Export]
        private float acceleration = 4.5f;

        [Export]
        private float deceleration = 16f;

        [Export]
        private float mouseSensitivity = 0.05f;
        
        private Vector3 velocity = Vector3.Zero;
        private Spatial cameraRotationHoriz;
        private Camera camera;
        private SpotLight flashlight;
        
        
        public override void _Ready()
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            cameraRotationHoriz = GetNode<Spatial>("CameraRotationHoriz");
            camera = GetNode<Camera>("CameraRotationHoriz/Camera");
            flashlight = GetNode<SpotLight>("CameraRotationHoriz/Flashlight");
        }

        public override void _PhysicsProcess(float delta)
        {
            var cameraXform = camera.GlobalTransform;
            // Get our input
            var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            var movementDir = new Vector3();
            movementDir += cameraXform.basis.z * direction.y;
            movementDir += cameraXform.basis.x * direction.x;


            var horizontalVelocity = velocity;
            horizontalVelocity.y = 0;
            
            // How far could we move?
            var target = movementDir * maxSpeed;

            var accel = movementDir.Dot(horizontalVelocity) > 0 ? acceleration : deceleration;

            horizontalVelocity = horizontalVelocity.LinearInterpolate(target, accel * delta);
            velocity.x = horizontalVelocity.x;
            velocity.z = horizontalVelocity.z;
            velocity = MoveAndSlide(velocity, Vector3.Up);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event is InputEventMouseMotion mouseEvent)
            {
                cameraRotationHoriz.RotateX(Mathf.Deg2Rad(mouseEvent.Relative.y * mouseSensitivity * -1));
                RotateY(Mathf.Deg2Rad(mouseEvent.Relative.x * mouseSensitivity * -1));
            }
            else if (@event is InputEventMouseButton mouseButton)
            {
                if (mouseButton.Pressed)
                {
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                }
            }
            else if (@event.IsActionPressed("detach_mouse"))
            {
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else if (@event.IsActionPressed("toggle_flashlight"))
            {
                if (flashlight.IsVisibleInTree())
                {
                    flashlight.Hide();
                }
                else
                {
                    flashlight.Show();
                }
            }
        }
    }
}
