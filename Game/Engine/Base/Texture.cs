using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace LadaEngine
{
	public class Texture : IDisposable
	{
		/// <summary>
		///  Create texture from GL Handle
		/// </summary>
		/// <param name="glHandle"></param>
		public Texture(int glHandle)
		{
			if (GlobalOptions.full_debug)
				Misc.Log("Texture " + Convert.ToString(Handle) + " created");
			Handle = glHandle;
		}

		public int Handle { get; set; }

		public void Dispose()
		{
			// To be implemented
		}

		/// <summary>
		///  Create new Texture from bitmap class
		/// </summary>
		/// <param name="bmp"></param>
		/// <returns></returns>
		public static Texture LoadFromBitmap(Bitmap bmp)
		{
			var handle = GL.GenTexture();
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, handle);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			var data = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.ReadOnly,
				PixelFormat.Format32bppArgb);
			GL.TexImage2D(TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				bmp.Width,
				bmp.Height,
				0,
				OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
				PixelType.UnsignedByte,
				data.Scan0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) TextureMagFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			bmp.UnlockBits(data);
			return new Texture(handle);
		}

		/// <summary>
		///  Create texture with an image from file
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static Texture LoadFromFile(string path)
		{
			// Generate handle
			var handle = GL.GenTexture();

			// Bind the handle
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, handle);

			// For this example, we're going to use .NET's built-in System.Drawing library to load textures.

			// Load the image
			using (var image = new Bitmap(path))
			{
				image.RotateFlip(RotateFlipType.RotateNoneFlipY);
				var data = image.LockBits(
					new Rectangle(0, 0, image.Width, image.Height),
					ImageLockMode.ReadOnly,
					PixelFormat.Format32bppArgb);
				GL.TexImage2D(TextureTarget.Texture2D,
					0,
					PixelInternalFormat.Rgba,
					image.Width,
					image.Height,
					0,
					OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
					PixelType.UnsignedByte,
					data.Scan0);
			}

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
				(int) TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
				(int) TextureMagFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) TextureWrapMode.Repeat);
			//GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

			return new Texture(handle);
		}


		public void UpdateData(Bitmap bmp)
		{
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Handle);
			bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
			var data = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,
				bmp.Width,
				bmp.Height,
				0,
				OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
				PixelType.UnsignedByte,
				data.Scan0);

			bmp.UnlockBits(data);
		}

		/// <summary>
		///  Place texture in a slot
		/// </summary>
		/// <param name="unit"></param>
		public void Use(TextureUnit unit)
		{
			if (Handle != GlobalOptions.lastTextureUsed[unit - TextureUnit.Texture0])
			{
				GL.ActiveTexture(unit);
				GL.BindTexture(TextureTarget.Texture2D, Handle);
				if (GlobalOptions.full_debug)
					Misc.Log("Texture " + Convert.ToString(Handle) + " loaded to slot " +
					         Convert.ToString((int) unit - 33984));

				GlobalOptions.lastTextureUsed[unit - TextureUnit.Texture0] = Handle;
			}
		}
	}
}