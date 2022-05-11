using LadaEngine;

namespace Platformer.Common
{
	public class Camera
	{
		public Pos Position = new Pos(0, 0);
		public float Zoom = 1f;

		public Camera(Pos position, float zoom)
		{
			Position = position;
			Zoom = zoom;
		}

		public Camera()
		{
			Position = new Pos(0, 0);
			Zoom = 20f;
		}
	}
}