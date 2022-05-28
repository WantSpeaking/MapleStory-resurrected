using System;
using MapleLib.WzLib;
//using MapleLib.WzLib.Imaging;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MapleLib.Helpers;
using Microsoft.Xna.Framework.Graphics;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that contains the information for a bitmap
	/// </summary>
	public partial class WzPngProperty : WzImageProperty
	{
		private byte[] _data;
		internal Bitmap png;
		// true: argb32 false:rgb565
		private bool _formatType = true;

		public override object WzValue { get { return GetImage (false); } }
		/*public override byte[] GetPngData (out PngInfo pngInfo)
		{
			//if (_data == null)
			{
				compressedImageBytes = GetCompressedBytes (false);
				//ParsePngNew ();
				ParsePng(false,null);
			}
			if (_data == null)
			{
				AppDebug.Log ($"{FullPath} pngdata is null ,may be wrong format");
				pngInfo = default;
				return null;
			}
			pngInfo = new PngInfo ();
			pngInfo.Width = Width;
			pngInfo.height = Height;
			pngInfo.format = Format;
			var copyData = new byte[_data.Length];
			Array.Copy (_data, copyData, _data.Length);
			return copyData;
		}*/

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
					data = decBuf;
					AppDebug.Log ("转换DEBUG !!!");
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

		public void SetImage (Bitmap png)
		{
			this.png = png;
			//CompressPng (png);
			//_data = byteData;
		}

		public Bitmap GetImage (bool saveInMemory)
		{
			if (png == null)
			{
				ParsePng (saveInMemory);
			}
			return png;
			//return GetPngData (out var pngInfo);
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
                                    bmp = new Bitmap(Width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height * 2;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataBgra4444(decBuf, Width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, decoded.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 2:
                                {
                                    bmp = new Bitmap(Width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height * 4;
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
                                    bmp = new Bitmap(Width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height * 4;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataDXT3(decBuf, Width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, Width * height);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 513:
                                {
                                    bmp = new Bitmap(Width, height, PixelFormat.Format16bppRgb565);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height * 2;
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
                                    bmp = new Bitmap(Width, height, PixelFormat.Format16bppRgb565);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);

                                    int uncompressedSize = Width * height / 128;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    byte[] decoded = GetPixelDataForm517(decBuf, Width, height);
                                    _data = decoded;
                                    Marshal.Copy(decoded, 0, bmpData.Scan0, decoded.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 1026:
                                {
                                    bmp = new Bitmap(this.Width, this.height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(new Point(), bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height * 4;
                                    byte[] decBuf = new byte[uncompressedSize];
                                    zlib.Read(decBuf, 0, uncompressedSize);
                                    zlib.Close();

                                    decBuf = GetPixelDataDXT3(decBuf, this.Width, this.height);
                                    _data = decBuf;
                                    Marshal.Copy(decBuf, 0, bmpData.Scan0, decBuf.Length);
                                    bmp.UnlockBits(bmpData);
                                    break;
                                }
                            case 2050: // new
                                {
                                    bmp = new Bitmap(Width, height, PixelFormat.Format32bppArgb);
                                    BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, Width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                                    int uncompressedSize = Width * height;
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

		/// <summary>
		/// Wz PNG format to Microsoft.Xna.Framework.Graphics.SurfaceFormat
		/// https://github.com/Kagamia/WzComparerR2/search?q=wzlibextension
		/// </summary>
		/// <param name="pngform"></param>
		/// <returns></returns>
		public SurfaceFormat GetXNASurfaceFormat ()
		{
			return XnaFormatHelper.RawPng_To_XNASurfaceFormat (Format);
		}

		public void ParsePng (bool saveInMemory, Texture2D texture2d = null)
		{
			byte[] rawBytes = GetRawImage (saveInMemory);
			byte[] decodedBytes = null;

			if (rawBytes == null)
			{
				png = null;
				return;
			}
			try
			{
				Bitmap bmp = null;
				Rectangle rect_ = new Rectangle (0, 0, width, height);

				switch (Format)
				{
					case 1:
						{
							//bmp = new Bitmap (Width, height, PixelFormat.Format32bppArgb);
							//BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

							DecompressImage_PixelDataBgra4444 (rawBytes, width, height, out decodedBytes);
							DecompressImage_PixelDataBgra_To_RGBA (decodedBytes, width, height, out decodedBytes);
							//decodedBytes = rawBytes;
							
							break;
						}
					case 2:
						{
							/*bmp = new Bitmap (Width, height, PixelFormat.Format32bppArgb);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

							Marshal.Copy (rawBytes, 0, bmpData.Scan0, rawBytes.Length);
							bmp.UnlockBits (bmpData);*/
							//DecompressImage_PixelDataBgra32_To_ARGB (rawBytes, Width, height, out decodedBytes);
							//DecompressImage_PixelDataBgra32_To_ARGB (rawBytes, Width, height, out decodedBytes);
							DecompressImage_PixelDataBgra_To_RGBA(rawBytes, width, height, out decodedBytes);
							//decodedBytes = rawBytes;
							break;
						}
					case 3:
						{
							// New format 黑白缩略图
							// thank you Elem8100, http://forum.ragezone.com/f702/wz-png-format-decode-code-1114978/ 
							// you'll be remembered forever <3 
							/*bmp = new Bitmap (Width, height, PixelFormat.Format32bppArgb);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);*/

							DecompressImageDXT3 (rawBytes, width, height, out decodedBytes);
							DecompressImage_PixelDataBgra_To_RGBA (decodedBytes, width, height, out decodedBytes);
							break;
						}
					case 257: // http://forum.ragezone.com/f702/wz-png-format-decode-code-1114978/index2.html#post9053713
						{
							//bmp = new Bitmap (Width, height, PixelFormat.Format16bppArgb1555);
							//BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
							// "Npc.wz\\2570101.img\\info\\illustration2\\face\\0"

							//CopyBmpDataWithStride (rawBytes, bmp.Width * 2, bmpData);
							//todo CopyBmpDataWithStride
							decodedBytes = rawBytes;

							//bmp.UnlockBits (bmpData);
							break;
						}
					case 513: // nexon wizet logo
						{
							/*bmp = new Bitmap (Width, height, PixelFormat.Format16bppRgb565);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);

							Marshal.Copy (rawBytes, 0, bmpData.Scan0, rawBytes.Length);
							bmp.UnlockBits (bmpData);*/

							decodedBytes = rawBytes;
							break;
						}
					case 517:
						{
							/*bmp = new Bitmap (Width, height, PixelFormat.Format16bppRgb565);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);*/

							DecompressImage_PixelDataForm517 (rawBytes, width, height, out decodedBytes);
							break;
						}
					case 1026:
						{
							/*bmp = new Bitmap (this.Width, this.height, PixelFormat.Format32bppArgb);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);*/

							DecompressImageDXT3 (rawBytes, this.width, this.height, out decodedBytes);
							DecompressImage_PixelDataBgra_To_RGBA (decodedBytes, width, height, out decodedBytes);

							break;
						}
					case 2050: // new
						{
							/*bmp = new Bitmap (Width, height, PixelFormat.Format32bppArgb);
							BitmapData bmpData = bmp.LockBits (rect_, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);*/
							DecompressImage_PixelDataBgra32_To_ARGB (rawBytes, width, height, out decodedBytes);
							DecompressImage_PixelDataBgra_To_RGBA (decodedBytes, width, height, out decodedBytes);

							//DecompressImageDXT5 (rawBytes, Width, Height, out decodedBytes);
							break;
						}
					default:
						Helpers.ErrorLogger.Log (Helpers.ErrorLevel.MissingFeature, string.Format ("Unknown PNG format {0} {1}", format, format2));
						break;
				}
				bmp = new Bitmap (decodedBytes, width, height, Format);
				if (bmp != null)
				{
					if (texture2d != null)
					{
						Microsoft.Xna.Framework.Rectangle rect = new Microsoft.Xna.Framework.Rectangle (Microsoft.Xna.Framework.Point.Zero,
							new Microsoft.Xna.Framework.Point (width, height));
						texture2d.SetData (0, 0, rect, rawBytes, 0, rawBytes.Length);
					}
				}

				_data = decodedBytes;
				png = bmp;
			}
			catch (InvalidDataException)
			{
				_data = null;
				png = null;
			}
		}

		public int pngWidth => width;
		public int pngHeight => height;
		/// <summary>
		/// Parses the raw image bytes from WZ
		/// </summary>
		/// <returns></returns>
		internal byte[] GetRawImage (bool saveInMemory)
		{
			byte[] rawImageBytes = GetCompressedBytes (saveInMemory);

			try
			{
				using (BinaryReader reader = new BinaryReader (new MemoryStream (rawImageBytes)))
				{
					DeflateStream zlib;

					ushort header = reader.ReadUInt16 ();
					listWzUsed = header != 0x9C78 && header != 0xDA78 && header != 0x0178 && header != 0x5E78;
					if (!listWzUsed)
					{
						zlib = new DeflateStream (reader.BaseStream, CompressionMode.Decompress);
					}
					else
					{
						reader.BaseStream.Position -= 2;
						MemoryStream dataStream = new MemoryStream ();
						int blocksize = 0;
						int endOfPng = rawImageBytes.Length;

						// Read image into zlib
						while (reader.BaseStream.Position < endOfPng)
						{
							blocksize = reader.ReadInt32 ();
							for (int i = 0; i < blocksize; i++)
							{
								dataStream.WriteByte ((byte)(reader.ReadByte () ^ ParentImage.reader.WzKey[i]));
							}
						}
						dataStream.Position = 2;
						zlib = new DeflateStream (dataStream, CompressionMode.Decompress);
					}

					int uncompressedSize = 0;
					byte[] decBuf = null;

					switch (format + format2)
					{
						case 1: // 0x1
							{
								uncompressedSize = width * height * 2;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 2: // 0x2
							{
								uncompressedSize = width * height * 4;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 3: // 0x2 + 1?
							{
								// New format 黑白缩略图
								// thank you Elem8100, http://forum.ragezone.com/f702/wz-png-format-decode-code-1114978/ 
								// you'll be remembered forever <3 

								uncompressedSize = width * height * 4;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 257: // 0x100 + 1?
							{
								// http://forum.ragezone.com/f702/wz-png-format-decode-code-1114978/index2.html#post9053713
								// "Npc.wz\\2570101.img\\info\\illustration2\\face\\0"

								uncompressedSize = width * height * 2;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 513: // 0x200 nexon wizet logo
							{
								uncompressedSize = width * height * 2;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 517: // 0x200 + 5
							{
								uncompressedSize = width * height / 128;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 1026: // 0x400 + 2?
							{
								uncompressedSize = width * height * 4;
								decBuf = new byte[uncompressedSize];
								break;
							}
						case 2050: // 0x800 + 2? new
							{
								uncompressedSize = width * height;
								decBuf = new byte[uncompressedSize];
								break;
							}
						default:
							Helpers.ErrorLogger.Log (Helpers.ErrorLevel.MissingFeature, string.Format ("Unknown PNG format {0} {1}", format, format2));
							break;
					}

					if (decBuf != null)
					{
						using (zlib)
						{
							zlib.Read (decBuf, 0, uncompressedSize);
							return decBuf;
						}
					}
				}
			}
			catch (InvalidDataException)
			{
			}
			return null;
		}

		public byte[] GetDecodedData()
		{
			//if (_data ==null)
			{
				ParsePng(false, null);
			}
			return _data;	
		}

		#region Decoders
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void DecompressImage_PixelDataBgra4444 (byte[] rawData, int width, int height, out byte[] decoded)
		{
			int uncompressedSize = width * height * 2;
			decoded = new byte[uncompressedSize * 2];

			for (int i = 0; i < uncompressedSize; i++)
			{
				byte byteAtPosition = rawData[i];

				int lo = byteAtPosition & 0x0F;
				byte b = (byte)(lo | (lo << 4));
				decoded[i * 2] = b;

				int hi = byteAtPosition & 0xF0;
				byte g = (byte)(hi | (hi >> 4));
				decoded[i * 2 + 1] = g;
			}
			//AppDebug.Log ($"rawData.Length:{rawData.Length} uncompressedSize:{uncompressedSize}");

			/*Marshal.Copy (decoded, 0, bmpData.Scan0, decoded.Length);
            bmp.UnlockBits (bmpData);*/
		}

		/// <summary>
		/// DXT3
		/// </summary>
		/// <param name="rawData"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bmp"></param>
		/// <param name="bmpData"></param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void DecompressImageDXT3 (byte[] rawData, int width, int height, out byte[] decoded)
		{
			decoded = new byte[width * height * 4];

			if (SquishPNGWrapper.CheckAndLoadLibrary ())
			{
				SquishPNGWrapper.DecompressImage (decoded, width, height, rawData, (int)SquishPNGWrapper.FlagsEnum.kDxt3);
			}
			else  // otherwise decode here directly, fallback.
			{
				Color[] colorTable = new Color[4];
				int[] colorIdxTable = new int[16];
				byte[] alphaTable = new byte[16];
				for (int y = 0; y < height; y += 4)
				{
					for (int x = 0; x < width; x += 4)
					{
						int off = x * 4 + y * width;
						ExpandAlphaTableDXT3 (alphaTable, rawData, off);
						ushort u0 = BitConverter.ToUInt16 (rawData, off + 8);
						ushort u1 = BitConverter.ToUInt16 (rawData, off + 10);
						ExpandColorTable (colorTable, u0, u1);
						ExpandColorIndexTable (colorIdxTable, rawData, off + 12);

						for (int j = 0; j < 4; j++)
						{
							for (int i = 0; i < 4; i++)
							{
								SetPixel (decoded,
									x + i,
									y + j,
									width,
									colorTable[colorIdxTable[j * 4 + i]],
									alphaTable[j * 4 + i]);
							}
						}
					}
				}
			}
			/*Marshal.Copy (decoded, 0, bmpData.Scan0, decoded.Length);
			bmp.UnlockBits (bmpData);*/
		}


		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void DecompressImage_PixelDataForm517 (byte[] rawData, int width, int height, out byte[] decoded)
		{
			decoded = new byte[width * height * 2];

			int lineIndex = 0;
			for (int j0 = 0, j1 = height / 16; j0 < j1; j0++)
			{
				var dstIndex = lineIndex;
				for (int i0 = 0, i1 = width / 16; i0 < i1; i0++)
				{
					int idx = (i0 + j0 * i1) * 2;
					byte b0 = rawData[idx];
					byte b1 = rawData[idx + 1];
					for (int k = 0; k < 16; k++)
					{
						decoded[dstIndex++] = b0;
						decoded[dstIndex++] = b1;
					}
				}
				for (int k = 1; k < 16; k++)
				{
					Array.Copy (decoded, lineIndex, decoded, dstIndex, width * 2);
					dstIndex += width * 2;
				}

				lineIndex += width * 32;
			}
			/*Marshal.Copy (decoded, 0, bmpData.Scan0, decoded.Length);
			bmp.UnlockBits (bmpData);*/
		}

		/// <summary>
		/// DXT5
		/// </summary>
		/// <param name="rawData"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bmp"></param>
		/// <param name="bmpData"></param>
		[MethodImpl (MethodImplOptions.AggressiveInlining)]
		public static void DecompressImageDXT5 (byte[] rawData, int width, int height, out byte[] decoded)
		{
			decoded = new byte[width * height * 4];

			if (SquishPNGWrapper.CheckAndLoadLibrary ())
			{
				SquishPNGWrapper.DecompressImage (decoded, width, height, rawData, (int)SquishPNGWrapper.FlagsEnum.kDxt5);
			}
			else  // otherwise decode here directly, fallback
			{
				Color[] colorTable = new Color[4];
				int[] colorIdxTable = new int[16];
				byte[] alphaTable = new byte[8];
				int[] alphaIdxTable = new int[16];
				for (int y = 0; y < height; y += 4)
				{
					for (int x = 0; x < width; x += 4)
					{
						int off = x * 4 + y * width;
						ExpandAlphaTableDXT5 (alphaTable, rawData[off + 0], rawData[off + 1]);
						ExpandAlphaIndexTableDXT5 (alphaIdxTable, rawData, off + 2);
						ushort u0 = BitConverter.ToUInt16 (rawData, off + 8);
						ushort u1 = BitConverter.ToUInt16 (rawData, off + 10);
						ExpandColorTable (colorTable, u0, u1);
						ExpandColorIndexTable (colorIdxTable, rawData, off + 12);

						for (int j = 0; j < 4; j++)
						{
							for (int i = 0; i < 4; i++)
							{
								SetPixel (decoded,
									x + i,
									y + j,
									width,
									colorTable[colorIdxTable[j * 4 + i]],
									alphaTable[alphaIdxTable[j * 4 + i]]);
							}
						}
					}
				}
			}
			/*Marshal.Copy (decoded, 0, bmpData.Scan0, decoded.Length);
			bmp.UnlockBits (bmpData);*/
		}


		/*[MethodImpl (MethodImplOptions.AggressiveInlining)]
		private static void CopyBmpDataWithStride (byte[] source, int stride, BitmapData bmpData)
		{
			if (bmpData.Stride == stride)
			{
				Marshal.Copy (source, 0, bmpData.Scan0, source.Length);
			}
			else
			{
				for (int y = 0; y < bmpData.Height; y++)
				{
					Marshal.Copy (source, stride * y, bmpData.Scan0 + bmpData.Stride * y, stride);
				}
			}

		}*/

		public static void DecompressImage_PixelDataBgra32_To_ARGB (byte[] rawData, int width, int height, out byte[] decoded)
		{
			var pixels = rawData;
			/*decoded = new byte[rawData.Length];
			for (var i = 0; i < pixels.Length; i += 4)
			{
				// bgra => rgba
				var ob = pixels[i];
				var og = pixels[i + 1];
				var or = pixels[i + 2];
				var oa = pixels[i + 3];

				decoded[i] = oa;
				decoded[i + 1] = or;
				decoded[i + 2] = og;
				decoded[i + 3] = ob;

				/*if (oa == 255)
					continue;
				var alpha = oa / 255f;
				decoded[i+1] = (byte)(decoded[i+1] * alpha);
				decoded[i + 2] = (byte)(decoded[i + 2] * alpha);
				decoded[i + 3] = (byte)(decoded[i + 3] * alpha);#1#
			}*/

			for (var i = 0; i < pixels.Length; i += 4)
			{
				// bgra => rgba
				var ob = pixels[i];
				var og = pixels[i + 1];
				var or = pixels[i + 2];
				var oa = pixels[i + 3];
				pixels[i] = or;
				pixels[i + 1] = og;
				pixels[i + 2] = ob;
				pixels[i + 3] = oa;
				if (pixels[i + 3] == 255)
					continue;
				var alpha = pixels[i + 3] / 255f;
				pixels[i] = (byte)(pixels[i] * alpha);
				pixels[i + 1] = (byte)(pixels[i + 1] * alpha);
				pixels[i + 2] = (byte)(pixels[i + 2] * alpha);
			}

			decoded = pixels;
		}

		public static void DecompressImage_PixelDataBgra_To_RGBA (byte[] rawData, int width, int height, out byte[] decoded)
		{
			//var pixels = rawData;
			decoded = new byte[rawData.Length];
			for (var i = 0; i < rawData.Length; i += 4)
			{
				// bgra => rgba
				var ob = rawData[i];
				var og = rawData[i + 1];
				var or = rawData[i + 2];
				var oa = rawData[i + 3];

				decoded[i] = or;
				decoded[i + 1] = og;
				decoded[i + 2] = ob;
				decoded[i + 3] = oa;

				if (oa == 255)
					continue;
				var alpha = oa / 255f;
				decoded[i+0] = (byte)(decoded[i+0] * alpha);
				decoded[i + 1] = (byte)(decoded[i + 1] * alpha);
				decoded[i + 2] = (byte)(decoded[i + 2] * alpha);
			}

			

		}

		public static void DecompressImage_PixelDataRGB565_To_RGB565 (byte[] rawData, int width, int height, out byte[] decoded)
		{
			//var pixels = rawData;
			decoded = new byte[rawData.Length];
			for (var i = 0; i < rawData.Length; i += 4)
			{
				// bgra => rgba
				var ob = rawData[i];
				var og = rawData[i + 1];
				var or = rawData[i + 2];
				var oa = rawData[i + 3];

				decoded[i] = or;
				decoded[i + 1] = og;
				decoded[i + 2] = ob;
				decoded[i + 3] = oa;

				if (oa == 255)
					continue;
				var alpha = oa / 255f;
				decoded[i + 0] = (byte)(decoded[i + 0] * alpha);
				decoded[i + 1] = (byte)(decoded[i + 1] * alpha);
				decoded[i + 2] = (byte)(decoded[i + 2] * alpha);
			}



		}
		#endregion




	}
}