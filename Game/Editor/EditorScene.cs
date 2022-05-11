using LadaEngine;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Platformer.Common
{
	public class EditorScene : IScene
	{
		private Camera _camera;
		private Editor _editor;

		public string GetName()
		{
			return "Editor";
		}

		public void Load()
		{
			_camera = new Camera();
			_camera.Zoom = 5;
			_editor = new Editor(_camera);
		}

		public void Render()
		{
			_editor.Render(_camera);
		}

		public void Update()
		{
			_editor.Update();
		}

		public void FixedUpdate()
		{

		}

		public void Resize()
		{


		}
	}
}