using System;
using OpenTK.Graphics.OpenGL4;

namespace LadaEngine
{
	public class FrameBuffer
	{
		private int FBO;
		public Sprite sprite;

		public Texture texture;

		public void Load(iPos screen_resolution)
		{
			FBO = GL.GenFramebuffer();

			GL.BindFramebuffer(FramebufferTarget.Framebuffer, FBO);

			texture = CreateTexture(screen_resolution);

			GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
				TextureTarget.Texture2D, texture.Handle, 0);

			sprite = new Sprite(texture);

		}

		public void Start()
		{
			if (GlobalOptions.bfbo != FBO)
			{
				GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, FBO);
				GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
			}

			GlobalOptions.bfbo = FBO;
		}

		public void Stop()
		{
			if (GlobalOptions.bfbo != 0)
			{
				GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
				GL.Clear(ClearBufferMask.ColorBufferBit);
			}

			GlobalOptions.bfbo = 0;
		}

		private Texture CreateTexture(iPos screen_resolution)
		{
			// Generate handle
			var handle = GL.GenTexture();

			// Bind the handle
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, handle);


			GL.TexImage2D(TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				screen_resolution.X,
				screen_resolution.Y,
				0,
				PixelFormat.Bgra,
				PixelType.UnsignedByte,
				IntPtr.Zero);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) TextureMagFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);

			return new Texture(handle);
		}

		public void ResizeToFullscreen()
		{
			sprite.quad.ReshapeWithCoords(-Misc.fbo_sprite_coords.X, Misc.fbo_sprite_coords.Y, 1, -1);
		}

		public void Resize(iPos screen_resolution)
		{

			// Bind the handle
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

			GL.TexImage2D(TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				screen_resolution.X,
				screen_resolution.Y,
				0,
				PixelFormat.Bgra,
				PixelType.UnsignedByte,
				IntPtr.Zero);
		}
	}
}