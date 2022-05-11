namespace LadaEngine
{
	public class AnimatedSprite : BaseObject
	{
		public int grid_length = 10;
		public int grid_width = 10;
		public Texture normal_map;
		private Quad quad;
		public Shader shader;

		public int state;
		public Texture textures;

		public AnimatedSprite(Texture textureLocation, int gridLength, int gridRowLength)
		{
			// shader is a default tilemap shader
			shader = StandartShaders.GenAnimatedShader();
			grid_length = gridLength;
			grid_width = gridRowLength;
			textures = textureLocation;

			Load();
		}

		public void Load()
		{
			quad = new Quad(Misc.fullscreenverticies, shader, textures);
			quad.Load();

			if (GlobalOptions.full_debug)
				Misc.Log("Tilemap loaded");
		}

		public override void Render(Pos cam)
		{
			if (GlobalOptions.full_debug)
				Misc.Log("--- Anim render begin ---");

			quad.texture = textures;
			shader.SetInt("texture_length", grid_length);
			shader.SetInt("texture_width", grid_width);
			shader.SetInt("state", state);
			quad.Render(cam);
			if (GlobalOptions.full_debug)
				Misc.Log(" --- Anim render end ---");
		}

		public override void ReshapeVertexArray()
		{
			quad.ReshapeVertexArray(this);
		}

		public void Rotate(float angle)
		{
			rotation = angle;
			quad.rotate(angle);
		}

		public void FlipX()
		{
			quad.FlipX();
		}

		public void FlipY()
		{
			quad.FlipY();
		}
	}
}