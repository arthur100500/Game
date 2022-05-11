using LadaEngine;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Light.Shadow
{
	internal class ShadowTile : Tile
	{
		private float[] p = new float[4];
		// . * .
		// *   *
		// . * .
		const float shadowSize = 2f;

		public ShadowTile(Pos position, TextureAtlas textureAtlas, bool[] shadowdata) : base(position, textureAtlas, new iPos(0, 2))
		{
			if (shadowdata[0] || shadowdata[1] || shadowdata[3] || shadowdata[4])
				p[0] = shadowSize;
			if (shadowdata[1] || shadowdata[2] || shadowdata[4] || shadowdata[5])
				p[1] = shadowSize;
			if (shadowdata[3] || shadowdata[4] || shadowdata[6] || shadowdata[7])
				p[2] = shadowSize;
			if (shadowdata[4] || shadowdata[5] || shadowdata[7] || shadowdata[8])
				p[3] = shadowSize;

			for (int i = 0; i < p.Length; i++)
				p[i] = shadowSize - p[i];

			FlipP(new float[] { shadowSize, 0, 0, 0 });
			FlipP(new float[] { 0, 0, 0, shadowSize });
			FlipP(new float[] { shadowSize, shadowSize, shadowSize, 0 });
			FlipP(new float[] { 0, shadowSize, shadowSize, shadowSize });
		}

		private void FlipP(float[] array)
		{
			if (p.SequenceEqual(array))
			{
				p = new float[] { array[1], array[0], array[3], array[2] };

				Width = -Width;
			}
		}

		public override void AddToVerts(List<float> verticies, Camera camera)
		{
			this.InitializeVerts();
			base.RotateVerts(Rotation, camera);


			if (true)
				for (var i = 0; i < 20; i++)
					verticies.Add(_verts[i]);
		}

		private new void InitializeVerts()
		{
			float baseX = _atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 0];

			_verts = new float[]
			{
				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 0],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 3] + (float)p[3] / 100, // top right
				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 2],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 3] + (float)p[1] / 100, // bottom right
				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 4],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 3] + (float)p[0] / 100, // bottom left
				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 6],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 3] + (float) p[2] / 100 // top left
			};
		}
	}
}
