using dungeonblaster.Debug;
using Godot;

namespace dungeonblaster.Player.Weapon
{
    /// <summary>
    /// Manages the firing of all weapons
    /// </summary>
    public class WeaponHandler : Spatial
    {
        private RayCast weaponRaycast;
        
        public override void _Ready()
        {
            weaponRaycast = GetNode<RayCast>("pistol/PistolRaycast");
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed("use_weapon"))
            {
                FireWeapon();
            }
        }

        private void FireWeapon()
        {
            weaponRaycast.ForceRaycastUpdate();

            if (!weaponRaycast.IsColliding())
            {
                return;
            }

            var collider = weaponRaycast.GetCollider();
            // GD.Print(collider.Get("name"));
            DebugDrawer.DrawDebugSphere(weaponRaycast.GetCollisionPoint(), Colors.Magenta, decaySeconds: 2f);
        }
    }
}
