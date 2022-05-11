using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using Game.Enemy;
using Game.Light;
using Game.World.Tile;
using Game.WorldGen;
using Game.WorldGen.RGLevelgen;
using LadaEngine;
using Platformer.Common;

namespace Game.World
{
	public class World
	{
		public LevelRenderer Renderer;
		public LevelRenderer EntityRenderer;

		public List<IChunk> LoadedChunks;
		public List<IChunk> _renderLoadedChunks;
		public List<Entity> Entities;
		public List<Entity> NewEntities;
		public List<Slime> Enemies;

		private int _chunkLoadedArea = 3;

		public readonly int ChunkSize = 16;

		public Player player;
		private iPos _playerPrevPos;
		public TextureAtlas Atlas;
		private Level loadedChunksLevel;
		private Level entitiesLevel;
		public LightMap LightMap;

		public LevelGen generator;
		public World(Window window)
		{
			player = new Player(this, window);
			player.Camera.Zoom = 5f;
			generator = new LevelGen();

			loadedChunksLevel = new Level("Files/Textures/sprites2.png", 100, 100);
			entitiesLevel = new Level("Files/Textures/sprites2.png", 100, 100);
			_renderLoadedChunks = new List<IChunk>();

			LoadedChunks = new List<IChunk>();
			Entities = new List<Entity>();
			NewEntities = new List<Entity>();
			Enemies = new List<Slime>();

			Atlas = loadedChunksLevel.textureAtlas;

			Renderer = new LevelRenderer(loadedChunksLevel.textureAtlas, loadedChunksLevel);
			EntityRenderer = new LevelRenderer(entitiesLevel.textureAtlas, entitiesLevel);

			LightMap = new LightMap(window, this);
			var playerLight = new Light.Light(new Pos(0, 0), LightMap.LightLevel.textureAtlas, new iPos(0, 1));
			playerLight.Width = playerLight.Height = 30;
			LightMap.LightLevel.tiles.Add(playerLight);
		}

		public void SetTile(iPos position, WorldTile tile)
		{
			iPos worldPos = new iPos((int)MathF.Floor(position.X / (float)ChunkSize), (int)MathF.Floor(position.Y / (float)ChunkSize));

			GetChunk(worldPos).SetTile(position.X - worldPos.X * ChunkSize, position.Y - worldPos.Y * ChunkSize, tile);

			Console.WriteLine(position);

			UpdateRenderData();
		}

		public IChunk GetChunk(iPos position)
		{

			foreach (var chunk in LoadedChunks)
			{
				if (chunk.Position == position)
					return chunk;
			}
			LoadedChunks.Add(generator.GenerateChunk(position, this));
			return LoadedChunks.Last();
		}

		public void Render()
		{
			LightMap.RenderWorldStart();

			if (_playerPrevPos != new iPos((int)(player.Position.X / ChunkSize), (int)(player.Position.Y / ChunkSize)))
			{
				OnPLayerEnteringChunk();
				_playerPrevPos = new iPos((int)(player.Position.X / ChunkSize), (int)(player.Position.Y / ChunkSize));
				Renderer.UpdateBuffers();
			}

			Renderer.Shader.SetVector2("position", new OpenTK.Mathematics.Vector2(CutPos(player.Position.X / player.Camera.Zoom), CutPos(player.Position.Y / player.Camera.Zoom)));

			Renderer.Render(new Camera(new Pos(0, 0), player.Camera.Zoom));

			entitiesLevel.tiles.Clear();
			foreach (var e in Entities)
			{
				if (Misc.Len(e.Position, player.Position) >= 3 * player.Camera.Zoom)
					continue;
				if (e is MultitileStructure)
					entitiesLevel.tiles.AddRange(((MultitileStructure)e).InnerTiles);
				if (e is Drop)
					entitiesLevel.tiles.Add(((Drop)e).Tile);
			}
			foreach (var enemy in Enemies)
				entitiesLevel.tiles.Add(enemy.Tile);


			EntityRenderer.UpdateVerts(new Camera(new Pos(0, 0), player.Camera.Zoom));
			EntityRenderer.Shader.SetVector2("position", new OpenTK.Mathematics.Vector2(CutPos(player.Position.X / player.Camera.Zoom), CutPos(player.Position.Y / player.Camera.Zoom)));

			EntityRenderer.UpdateBuffers();
			EntityRenderer.Render(new Camera(new Pos(0, 0), player.Camera.Zoom));

			
			player.Render();
			LightMap.LightLevel.tiles[0].Position = player.Position;

			LightMap.RenderWorldStop();
			LightMap.Renderer.Shader.SetVector2("position", new OpenTK.Mathematics.Vector2(CutPos(player.Position.X / player.Camera.Zoom), CutPos(player.Position.Y / player.Camera.Zoom)));

			LightMap.RenderLights();
		}

		private float CutPos(float f)
		{
			return f;
		}

		public void Update()
		{
			foreach (var e in Entities)
			{
				if (Misc.Len(e.Position, player.Position) >= 3 * player.Camera.Zoom)
					continue;
				if (e is Drop)
					((Drop)e).Update();
			}
			foreach (var s in Enemies)
				if (Misc.Len(s.Position, player.Position) >= 3 * player.Camera.Zoom)
					continue;
				else
					s.Update();

			DeleteDeadEntities();
		}

		private void DeleteDeadEntities()
		{
			var deletedEntities = 0;
			for (int i = 0; i < Enemies.Count - deletedEntities; i++)
			{
				if (Enemies[i].MarkedToDelete)
				{
					deletedEntities++;
					Enemies.Remove(Enemies[i]);
				}
			}

			deletedEntities = 0;
			for (int i = 0; i < Entities.Count - deletedEntities; i++)
			{
				if (Entities[i].MarkedToDelete)
				{
					deletedEntities++;
					Entities.Remove(Entities[i]);
				}
			}

			Entities.AddRange(NewEntities);
			NewEntities.Clear();
		}

		public void UpdateRenderData()
		{
			loadedChunksLevel.tiles.Clear();
			foreach (var chunk in _renderLoadedChunks)
			{
				loadedChunksLevel.tiles.AddRange(chunk.RenderableTiles);
			}
			Renderer.UpdateVerts(new Camera(new Pos(0, 0), player.Camera.Zoom));
		}

		public void OnPLayerEnteringChunk()
		{
			_renderLoadedChunks.Clear();

			for (int i = 0; i < _chunkLoadedArea * _chunkLoadedArea; i++)
			{
				_renderLoadedChunks.Add(GetChunk(new iPos(
					(int)MathF.Floor(player.Position.X / ChunkSize) + i % _chunkLoadedArea - _chunkLoadedArea / 2,
					(int)MathF.Floor(player.Position.Y / ChunkSize) + i / _chunkLoadedArea - _chunkLoadedArea / 2)));
				_renderLoadedChunks.Last().AssambleRenderableTiles();
			}

			UpdateRenderData();
		}

		public WorldTile GetTile(iPos position)
		{
			iPos worldPos = new iPos((int)MathF.Floor(position.X / (float)ChunkSize), (int)MathF.Floor(position.Y / (float)ChunkSize));

			return GetChunk(worldPos).GetTile(position.X - worldPos.X * ChunkSize, position.Y - worldPos.Y * ChunkSize);
		}
	}

}