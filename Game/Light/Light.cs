using LadaEngine;
using OpenTK.Mathematics;
using Platformer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Light
{
	public class Light : Tile
	{
		Vector4 color;
		public Light(Pos position, TextureAtlas textureAtlas, iPos lightTexture, Vector4 color) : base(position, textureAtlas, lightTexture)
		{
			Width = Height = 8;
			this.color = color;
		}

		public override void AddToVerts(List<float> verticies, Camera camera)
		{
			InitializeVerts();
			RotateVerts(Rotation, camera);

			if (true)
				for (var i = 0; i < 36; i++)
					verticies.Add(_verts[i]);
		}

		protected new void InitializeVerts()
		{
			_verts = new[]
			{
				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 0],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 1], // top right
				color.X, color.Y, color.Z, color.W,

				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 2],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 3], // bottom right
				color.X, color.Y, color.Z, color.W,

				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 4],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 5], // bottom left
				color.X, color.Y, color.Z, color.W,

				-0, 0, 1 / (1.1f + Level),
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 6],
				_atlas.Coordinates[(TextureInAtlas.X + TextureInAtlas.Y * _atlas.Width) * 8 + 7], // top left
				color.X, color.Y, color.Z, color.W
			};
		}

		protected new bool CheckBounds()
		{
			if (_verts[0] < -1 && _verts[10] < -1 && _verts[5] < -1 && _verts[15] < -1)
				return false;
			if (_verts[0] > 1 && _verts[10] > 1 && _verts[5] > 1 && _verts[15] > 1)
				return false;
			if (_verts[1] < -1 && _verts[11] < -1 && _verts[6] < -1 && _verts[16] < -1)
				return false;
			if (_verts[1] > 1 && _verts[11] > 1 && _verts[6] > 1 && _verts[16] > 1)
				return false;
			return true;
		}

		protected new void RotateVerts(float angle, Camera camera)
		{
			var rad = MathF.Sqrt(Width * Width + Height * Height) / camera.Zoom / 2;
			var AB = new Pos(Height, Width);
			var AC = new Pos(Height, -Width);

			rel_angles[0] = (float)Math.Atan2(AB.X, AB.Y);
			rel_angles[1] = (float)Math.Atan2(AC.X, AC.Y);
			rel_angles[2] = (float)Math.Atan2(AB.X, AB.Y) + (float)Math.PI;
			rel_angles[3] = (float)Math.Atan2(AC.X, AC.Y) + (float)Math.PI;

			var transposed = (Position - camera.Position) * (1 / camera.Zoom);
			angle = MathF.PI - angle;
			_verts[0] = transposed.X + rad * (float)Math.Cos(angle + rel_angles[2]);
			_verts[1] = transposed.Y + rad * (float)Math.Sin(angle + rel_angles[2]);
			_verts[9] = transposed.X + rad * (float)Math.Cos(angle + rel_angles[1]);
			_verts[10] = transposed.Y + rad * (float)Math.Sin(angle + rel_angles[1]);
			_verts[18] = transposed.X + rad * (float)Math.Cos(angle + rel_angles[0]);
			_verts[19] = transposed.Y + rad * (float)Math.Sin(angle + rel_angles[0]);
			_verts[27] = transposed.X + rad * (float)Math.Cos(angle + rel_angles[3]);
			_verts[28] = transposed.Y + rad * (float)Math.Sin(angle + rel_angles[3]);
		}
	}
}
