using LadaEngine;
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
		public Light(Pos position, TextureAtlas textureAtlas, iPos lightTexture) : base(position, textureAtlas, lightTexture)
		{
			Width = Height = 8;
			
		}
	}
}
