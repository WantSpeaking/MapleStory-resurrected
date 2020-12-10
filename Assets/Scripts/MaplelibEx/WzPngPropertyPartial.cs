using System;
using System.Drawing;
//using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using MapleLib.Helpers;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that contains the information for a bitmap
	/// </summary>
	public partial class WzPngProperty : WzImageProperty
	{
		private byte[] _data;

		// true: argb32 false:rgb565
		private bool _formatType = true;

		public override object WzValue { get { return GetPngData(out var pngInfo); } }
		public override byte[] GetPngData (out PngInfo pngInfo)
		{
			if (_data == null)
			{
				compressedImageBytes = GetCompressedBytes(false);
				ParsePngNew ();
			}
			pngInfo = new PngInfo ();
			pngInfo.width = Width;
			pngInfo.height = Height;
			pngInfo.format = Format;
			var copyData = new byte[_data.Length];
			Array.Copy (_data,copyData,_data.Length);
			return copyData;
		}

		private void ParsePngNew ()
		{
			DeflateStream zlib;
			int uncompressedSize;
			int x = 0, y = 0;
			byte[] data = null;
			var imgParent = ParentImage;
			byte[] decBuf;

			var reader = new BinaryReader (new MemoryStream (compressedImageBytes));
			var header = reader.ReadUInt16 ();
			listWzUsed = header != 0x9C78 && header != 0xDA78 && header != 0x0178 && header != 0x5E78;
			if (!listWzUsed)
			{
				zlib = new DeflateStream (reader.BaseStream, CompressionMode.Decompress);
			}
			else
			{
				reader.BaseStream.Position -= 2;
				var dataStream = new MemoryStream ();
				var endOfPng = compressedImageBytes.Length;

				while (reader.BaseStream.Position < endOfPng)
				{
					var blockSize = reader.ReadInt32 ();
					for (var i = 0; i < blockSize; i++)
					{
						dataStream.WriteByte ((byte)(reader.ReadByte () ^ ParentImage.reader.WzKey[i]));
					}
				}

				dataStream.Position = 2;
				zlib = new DeflateStream (dataStream, CompressionMode.Decompress);
			}

			switch (format + format2)
			{
				case 1:
					uncompressedSize = Width * Height * 2;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					var argb = new byte[uncompressedSize * 2];
					for (var i = 0; i < uncompressedSize; i++)
					{
						var b = decBuf[i] & 0x0F;
						b |= (b << 4);
						argb[i * 2] = (byte)b;
						var g = decBuf[i] & 0xF0;
						g |= (g >> 4);
						argb[i * 2 + 1] = (byte)g;
					}

					data = argb;
					break;
				case 2:
					uncompressedSize = Width * Height * 4;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					data = decBuf;
					break;
				case 3: // thanks to Elem8100 
					uncompressedSize = ((int)Math.Ceiling (Width / 4.0)) * 4 * ((int)Math.Ceiling (Height / 4.0)) * 4 /
					                   8;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					var argb2 = new byte[Width * Height];
				{
					var w = ((int)Math.Ceiling (Width / 4.0));
					var h = ((int)Math.Ceiling (Height / 4.0));
					for (var i = 0; i < h; i++)
					{
						int index2;
						for (var j = 0; j < w; j++)
						{
							var index = (j + i * w) * 2;
							index2 = j * 4 + i * Width * 4;
							var p = (decBuf[index] & 0x0F) | ((decBuf[index] & 0x0F) << 4);
							p |= ((decBuf[index] & 0xF0) | ((decBuf[index] & 0xF0) >> 4)) << 8;
							p |= ((decBuf[index + 1] & 0x0F) | ((decBuf[index + 1] & 0x0F) << 4)) << 16;
							p |= ((decBuf[index + 1] & 0xF0) | ((decBuf[index] & 0xF0) >> 4)) << 24;

							for (var k = 0; k < 4; k++)
							{
								if (x * 4 + k < Width)
								{
									argb2[index2 + k] = (byte)p;
								}
								else
								{
									break;
								}
							}
						}

						index2 = y * Width * 4;
						for (var m = 1; m < 4; m++)
						{
							if (y * 4 + m < Height)
							{
								Array.Copy (argb2, index2, argb2, index2 + m * Width, Width);
							}
							else
							{
								break;
							}
						}
					}
				}
					data = argb2;
					break;

				case 513:
					uncompressedSize = Width * Height * 2;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					_formatType = true;
					data = decBuf;
					break;

				case 517:
					uncompressedSize = Width * Height / 128;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					for (var i = 0; i < uncompressedSize; i++)
					{
						for (byte j = 0; j < 8; j++)
						{
							var iB = Convert.ToByte (((decBuf[i] & (0x01 << (7 - j))) >> (7 - j)) * 0xFF);
							for (var k = 0; k < 16; k++)
							{
								if (x == Width)
								{
									x = 0;
									y++;
								}

								// TODO
								// bmp.SetPixel(x, y, Color.FromArgb(0xFF, iB, iB, iB));
								x++;
							}
						}
					}

					Console.WriteLine ("转换DEBUG !!!");
					break;

				case 1026:
					uncompressedSize = Width * Height;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					decBuf = GetPixelDataDXT3 (decBuf, Width, Height);
					data = decBuf;
					break;

				case 2050: // thanks to Elem8100
					uncompressedSize = Width * Height;
					decBuf = new byte[uncompressedSize];
					zlib.Read (decBuf, 0, uncompressedSize);
					decBuf = GetPixelDataDXT5 (decBuf, Width, Height);
					data = decBuf;
					break;

				default:
					ErrorLogger.Log (ErrorLevel.MissingFeature,
						$"Unknown PNG format {format} {format2}");
					break;
			}

			_data = data;
		}

		public void SetImage(byte[] byteData)
		{
			_data = byteData;
		}

		public byte[] GetImage(bool saveInMemory)
		{
			return GetPngData(out var pngInfo);
		}
		/*internal void ParsePngNew2()
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(new MemoryStream(compressedImageBytes)))
                {
                    DeflateStream zlib;
                    Bitmap bmp = null;

                    ushort header = reader.ReadUInt16();
                    listWzUsed = header != 0x9C78 && header != 0xDA78 && header != 0x0178 && header != 0x5E78;
                    if (!listWzUsed)
                    {
                        zlib = new DeflateStream(reader.BaseStream, CompressionMode.Decompress);
                    }
                    else
                    {
                        reader.BaseStream.Position -= 2;
                        MemoryStream dataStream = new MemoryStream();
                        int blocksize = 0;
                        int endOfPng = compressedImageBytes.Length;

                        while (reader.BaseStream.Position < endOfPng)
                        {
                            blocksize = reader.ReadInt32();
                            for (int i = 0; i < blocksize; i++)
                            {
                                dataStream.WriteByte((byte)(reader.ReadByte() ^ ParentImage.reader.WzKey[i]));
                            }
                        }
                        dataStream.Position = 2;
                        zlib = new DeflateStream(dataStream, CompressionMode.Decompress);
                    }

                    using (zlib)
                    {
                        switch (format + format2)
                        {
                            case 1:
                                {
                                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height * 2;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataBgra4444(decBuf, width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, decoded.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 2:
                                {
                                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height * 4;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();
                                    _data = decBuf;
                                    Marshal.Copy(decBuf, 0, bmpData.Scan0, decBuf.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 3:
                                {
                                    // New format 黑白缩略图
                                    // thank you Elem8100, http://forum.ragezone.com/f702/wz-png-format-decode-code-1114978/ 
                                    // you'll be remembered forever <3 
                                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height * 4;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataDXT3(decBuf, width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, width * height);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 513:
                                {
                                    bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb565);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height * 2;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();
                                    _data = decBuf;
                                    Marshal.Copy(decBuf, 0, bmpData.Scan0, decBuf.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 517:
                                {
                                    bmp = new Bitmap(width, height, PixelFormat.Format16bppRgb565);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);

                                    int uncompressedSize = width * height / 128;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataForm517(decBuf, width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, decoded.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 1026:
                                {
                                    bmp = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height * 4;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    decBuf = GetPixelDataDXT3(decBuf, this.width, this.height);
                                    _data = decBuf;
                                    Marshal.Copy(decBuf, 0, bmpData.Scan0, decBuf.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 2050: // new
                                {
                                    bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = width * height;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    decBuf = GetPixelDataDXT5(decBuf, Width, Height);
                                    _data = decBuf;
                                    Marshal.Copy(decBuf, 0, bmpData.Scan0, decBuf.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            default:
                                Helpers.ErrorLogger.Log(Helpers.ErrorLevel.MissingFeature, string.Format("Unknown PNG format {0} {1}", format, format2));
                                break;
                        }
                        png = bmp;
                    }
                }
            }
            catch (InvalidDataException) 
            { 
                png = null;
            }
        }*/
	}
}