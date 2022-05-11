namespace Game.World.Tile
{
	public class WorldTile
	{
		public Platformer.Common.Tile TextureTile;
		public Platformer.Common.Tile TextureWallTile;
		public bool IsFloor;
		public float BreakingPower;

		public Drop BreakingDrop;

		public WorldTile(Platformer.Common.Tile tile, Platformer.Common.Tile wallTile, bool isFloor)
		{
			TextureTile = tile;
			TextureWallTile = wallTile;
			IsFloor = isFloor;
			BreakingPower = 1;
			BreakingDrop = null;
		}
	}
}