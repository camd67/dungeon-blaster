using Godot;
using Godot.Collections;

namespace dungeonblaster.Debug
{
    public class DebugDrawer : Spatial
    {
        private static DebugDrawer instance; 
            
        /// <summary>
        /// Draws a debug sphere at the given location with a given color.
        /// </summary>
        /// <param name="position">Center of the sphere's location</param>
        /// <param name="color">Color of the sphere to draw</param>
        /// <param name="size">How large (radius) we want the sphere to be</param>
        /// <param name="decaySeconds">How long until this sphere should decay away. -1 will never decay</param>
        public static void DrawDebugSphere(Vector3 position, Color color, float size = 0.2f, float decaySeconds = -1f)
        {
            instance.DrawDebugSphereInternal(position, color, size, decaySeconds);
        }

        public override void _Ready()
        {
            instance = this;
        }

        private void DrawDebugSphereInternal(Vector3 position, Color color, float size, float decaySeconds)
        {
            var sphere = new SphereMesh();
            // Arbitrary level of detail here
            sphere.RadialSegments = 4;
            sphere.Rings = 4;
            sphere.Radius = size;
            sphere.Height = size * 2;
            
            var material = new SpatialMaterial();
            material.AlbedoColor = color;
            material.FlagsUnshaded = true;
            
            sphere.SurfaceSetMaterial(0, material);

            var node = new MeshInstance();
            node.Mesh = sphere;
            node.Translation = position;

            
            AddChild(node);
            
            // Decay MUST come after adding child to the node so that the name is created
            if (decaySeconds > 0)
            {
                GetTree().CreateTimer(decaySeconds).Connect("timeout", this, nameof(DecayNode), new Array {node.Name});
            }
        }

        private void DecayNode(string nodeName)
        {
            GetNode(nodeName).QueueFree();
        }
    }
}
