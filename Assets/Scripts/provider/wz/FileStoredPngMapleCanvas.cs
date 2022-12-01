using System;
using System.IO;

namespace provider.wz
{

	public class FileStoredPngMapleCanvas : MapleCanvas
	{
		private DirectoryInfo file;
		private int width;
		private int height;
		//private BufferedImage image;

		public FileStoredPngMapleCanvas(int width, int height, DirectoryInfo fileIn)
		{
			this.width = width;
			this.height = height;
			this.file = fileIn;
		}

		public virtual int Height
		{
			get
			{
				return height;
			}
		}

		public virtual int Width
		{
			get
			{
				return width;
			}
		}

		public DirectoryInfo File => file;
	/*	public virtual BufferedImage Image
		{
			get
			{
				loadImageIfNecessary();
				return image;
			}
		}*/

		/*		private void loadImageIfNecessary()
				{
					if (image == null)
					{
						try
						{
							image = ImageIO.read(file);
							// replace the dimensions loaded from the wz by the REAL dimensions from the image - should be equal tho
							width = image.Width;
							height = image.Height;
						}
						catch (IOException e)
						{
							throw new Exception(e);
						}
					}
				}*/
	}

}