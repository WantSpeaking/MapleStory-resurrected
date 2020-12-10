using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Drawing;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Utility.PoolSystem;
using vadersb.utils.unity;
using Graphics = UnityEngine.Graphics;

namespace ms
{
	public struct BatchItem
	{
		public Vector2 position => new Vector2 (realposX, -realposY);
		public int posX;
		public int posY;
		public int realposX => posX - pivotX;
		public int realposY => posY - pivotY;
		public int pivotX;
		public int pivotY;
		public int width;
		public int height;
		public byte[] data;
		//public Bitmap bmp;
		public UnityEngine.Color color;
		public UnityEngine.Texture2D texture;
		public UnityEngine.Sprite sprite;
	}

	public class SpriteBatch : SingletonMono<SpriteBatch>
	{
		private static int screenWidth = 800;
		private static int screenHeight = 600;
		private static ConcurrentQueue<BatchItem> batchQueue = new ConcurrentQueue<BatchItem> ();
		private static ConcurrentQueue<UnityPoolItem> poolItemQueue = new ConcurrentQueue<UnityPoolItem> ();
		public static ConcurrentQueue<SpriteRenderer> spriteRenderQueue = new ConcurrentQueue<SpriteRenderer> ();
		//private ConcurrentDictionary<int, SpriteRenderer> spriteRenderDict = new ConcurrentDictionary<int, SpriteRenderer> ();
		private ConcurrentDictionary<Texture, SpriteRenderer> spriteRenderDict = new ConcurrentDictionary<Texture, SpriteRenderer> ();

		public int DrawOrder;
		private Texture2D background;

		public RenderTexture target => MapleStory.Instance.target;
		public UnityEngine.Color clearColor = UnityEngine.Color.magenta;

		private CommandBuffer clearBuffer;
		private Rect textureRelativeToCamera => new Rect (textureRect.x - cameraRect.x, textureRect.y - cameraRect.y, textureRect.width, textureRect.height);
		private const short camera_left = 0;
		private const short camera_top = 0;
		private const short camera_right = 800;
		private const short camera_bottom = 600;
		Rectangle<short> cameraRange = new Rectangle<short> (camera_left, camera_right, camera_top, camera_bottom);
		Rect cameraRect = new Rect (0, 0, camera_right, camera_bottom);
		private Rectangle<short> textureRange;
		private Rect textureRect;

		private SpriteRenderer spriteRenderer;

		protected override void OnAwake ()
		{
			base.OnAwake ();
			background = new Texture2D (screenWidth, screenHeight, TextureFormat.BGRA32, false);
			clearBuffer = new CommandBuffer () {name = "Clear Buffer"};
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}

		private bool overlaps ()
		{
			return cameraRange.overlaps (textureRange);
		}

		private bool contains ()
		{
			return cameraRange.contains (textureRange);
		}

		//private byte[][,] texturePixels = new byte[4][,];
		private byte[] texturePixels = new byte[screenWidth * screenHeight * 4];
		private int[,] array2d_Screen = new int[screenWidth, screenHeight];


		public void DrawTextureToTarget ()
		{
			GL.PushMatrix ();
			//GL.LoadPixelMatrix();
			GL.LoadPixelMatrix (0, screenWidth, screenHeight, 0);
			Graphics.DrawTexture (new Rect (0, 0, screenWidth, screenHeight), background);
			GL.PopMatrix ();


			/*Graphics.SetRenderTarget (target);
			//clearBuffer.Clear ();
			//clearBuffer.ClearRenderTarget (true, true, clearColor);
			//Graphics.ExecuteCommandBuffer (clearBuffer);
			GL.PushMatrix ();
			GL.LoadPixelMatrix (0, target.width, target.height, 0);
			//GL.LoadPixelMatrix(0, target.width, 0, target.height);
			Graphics.DrawTexture (textureRelativeToCamera, mainTexture);
			GL.PopMatrix ();*/
			Debug.Log ($"{texturePixels.ToDebugLog ()}");
			spriteRenderer.sprite = UnityEngine.Sprite.Create (background, cameraRect, Vector2.zero, 1);
		}

		public void DrawTextureToTarget (UnityEngine.Texture2D mainTexture)
		{
			Graphics.SetRenderTarget (target);
			//target.graphicsFormat = GraphicsFormat.B8G8R8A8_UInt;
			//target.format = RenderTextureFormat.BGRA32;
			//clearBuffer.Clear ();
			//clearBuffer.ClearRenderTarget (true, true, clearColor);
			//Graphics.ExecuteCommandBuffer (clearBuffer);
			GL.PushMatrix ();
			GL.LoadPixelMatrix (0, target.width, target.height, 0);
			//GL.LoadPixelMatrix(0, target.width, 0, target.height);
			Graphics.DrawTexture (new Rect (0, 0, target.width, target.height), mainTexture);
			GL.PopMatrix ();
		}

		public void Add (BatchItem batchItem)
		{
			batchQueue.Enqueue (batchItem);
		}

		public void Add (UnityPoolItem poolItem)
		{
			poolItemQueue.Enqueue (poolItem);
		}

		public SpriteRenderer TryGetSpriteRenderer (Texture hashCode, Func<SpriteRenderer> create = null)
		{
			if (!spriteRenderDict.TryGetValue (hashCode, out var result))
			{
				result = create?.Invoke ();
				if (result != null)
				{
					spriteRenderDict.TryAdd (hashCode, result);
				}
			}

			else
			{
				if (result == null)
				{
					spriteRenderDict.TryRemove (hashCode, out var removed);
				}
			}

			return result;
		}

		public void ClearSpriteRendererDict ()
		{
			spriteRenderDict.Clear ();
		}
		private void OnPostRender ()
		{
		}

		protected override void OnUpdate ()
		{
			base.OnUpdate ();
			//Draw ();
			//StartCoroutine (DespawnSpriteDrawer ());
		}

		protected override void OnLateUpdate ()
		{
			//Draw ();
			StartCoroutine (DespawnSpriteDrawer ());
		}

		WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame ();

		IEnumerator DespawnSpriteDrawer ()
		{
			yield return _waitForEndOfFrame;
			DrawOrder = 0;
			/*for (; poolItemQueue.Count > 0; poolItemQueue.TryDequeue (out var de))
			{
				if (poolItemQueue.TryPeek (out var poolItem))
				{
					//PoolManager.Despawn (poolItem);
					PoolManager.Despawn (poolItem, FinalDeSpawn);
				}
			}*/

			for (; spriteRenderQueue.Count > 0; spriteRenderQueue.TryDequeue (out var de))
			{
				if (spriteRenderQueue.TryPeek (out var spriteRenderer))
				{
					//spriteRenderer.sprite = null;
					spriteRenderer.enabled = false;
					if (spriteRenderer.gameObject.name.Contains (@"character.wz\00002003.img\walk2\2\body"))
					{
						Debug.Log ($"Despawn spriteRenderer.sprite:{spriteRenderer.sprite}");
					}
				}
			}
		}

		private void FinalDeSpawn (UnityPoolItem unityPoolItem)
		{
			var renderer = unityPoolItem.PooledRef.GetComponent<SpriteRenderer> ().Component;
			/*	var transform = unityPoolItem.PooledRef.GetComponent<Transform> ().Component;
				renderer.gameObject.name = Constants.get ().defaultSpriteDrawerName;*/
			renderer.sprite = null;
			/*renderer.flipY = false;
			renderer.sortingLayerName = Constants.get ().sortingLayer_Default.ToString ();
			renderer.sortingOrder = 0;
			transform.position = Vector3.zero;
			transform.localScale = Vector3.zero;*/
		}

		private void Draw ()
		{
			/*clearBuffer.Clear ();
			clearBuffer.ClearRenderTarget (true, true, clearColor);
			Graphics.ExecuteCommandBuffer (clearBuffer);*/

			if (batchQueue.Count == 0) return;

			if (batchQueue.Count > 0)
			{
				if (batchQueue.TryPeek (out var batchItem))
				{
					SpriteBatcher.Instance.RefreshMaterialPropertyBlock (batchItem.sprite?.texture);
					SpriteBatcher.Instance.ApplyMaterialPropertyBlock ();
				}
			}

			for (; batchQueue.Count > 0; batchQueue.TryDequeue (out var de))
			{
				if (batchQueue.TryPeek (out var batchItem))
				{
					textureRange = new Rectangle<short> ((short)(batchItem.realposX), (short)(batchItem.realposY), (short)batchItem.width, (short)batchItem.height);
					var realposX = batchItem.realposX;
					var realposY = batchItem.realposY;
					var height = batchItem.height;
					var width = batchItem.width;
					var pixelData = batchItem.data;
					var pixelTexture = batchItem.texture;
					var sprite = batchItem.sprite;
					var position = batchItem.position;
					var color = batchItem.color;
					//Debug.Log ($"texturePixels:{texturePixels.ToDebugLog ()}");
					//Debug.Log ($"pixelData:{pixelData.ToDebugLog ()}");

					var startPositionX = realposX;
					var startPositionY = realposY;


					//if (cameraRange.overlaps (textureRange))
					if (realposX >= camera_left && realposX <= camera_right && realposY >= camera_top && realposY <= camera_bottom)
					{
						{
							/*SpriteQuad spriteQuad = new SpriteQuad ();
							spriteQuad.SetupFromSprite (sprite);
							spriteQuad.Vertices_Move (position);
							SpriteBatcher.Instance.DrawSpriteQuad (spriteQuad);*/
							SpriteBatcher.Instance.DrawSprite (sprite, position, color);
						}

						{
							//DrawTextureToTarget (pixelTexture);
						}

						{
							/*for (int x = startPositionX; x < background.width; x++)
							{
								for (int y = startPositionY; y < background.height; y++)
								{
									if (x - startPositionX < width && y - startPositionY < height)
									{
										var wmColor = GetPixelByteArray (pixelData, x - startPositionX, y - startPositionY, width, height);
										SetPixelByteArray (wmColor, texturePixels, x, y, screenWidth, screenHeight);
									}
								}
							}*/
						}

						{
							//TextureAndSpriteUtil.AddWatermark (mainTexture, pixelTexture, realposX, realposY);
						}

						/*{
							Array.Copy (pixelData,texturePixels,pixelData.Length);
							
						}*/

						{
							/*var array2d_Pixel = ByteArray_1To2 (pixelData, width, height);
							for (int j = 0; j < height; j++)
							{
								for (int i = 0; i < width; i++)
								{
									//var indexInPixelArray = (realposY * screenWidth + realposX) * 4;
									if ((realposX + width) < screenWidth && (realposY + width) < screenHeight)
									{
										array2d_Screen[realposX + i, realposY + j] = array2d_Pixel[i, j];
									}
								}
							}*/
						}

						{
							/*var indexInPixelArray = (realposY * screenWidth + realposX) * 4;

							for (int j = 0; j < height; j++)
							{
								for (int i = 0; i < width; i++)
								{
									if ((realposX + width) < screenWidth && (realposY + width) < screenHeight)
									{
										var indexInPixelData = (j * width + i) * 4;
										texturePixels[indexInPixelArray] = pixelData[indexInPixelData];
										texturePixels[indexInPixelArray + 1] = pixelData[indexInPixelData + 1];
										texturePixels[indexInPixelArray + 2] = pixelData[indexInPixelData + 2];
										texturePixels[indexInPixelArray + 3] = pixelData[indexInPixelData + 3];
									}
								}
							}*/
						}

						{
							//DrawTextureToTarget (batchItem.texture);
						}


						{
							/*//var scale = batchItem.data.Length / (batchItem.height * batchItem.width);
													var height = batchItem.height;
													var width = batchItem.width;
													var pixelData = batchItem.data;
													var bmp = batchItem.bmp;
													int curPos = 0;
							
													for (int i = 0; i < height; i++)
													for (int j = 0; j < width; j++)
													{
														var drawingColor = bmp.GetPixel (j, i);
							
														UnityEngine.Color curPixel = new UnityEngine.Color (drawingColor.R, drawingColor.G, drawingColor.B, drawingColor.A);
														mainTexture.SetPixel (j, i, curPixel);
							
														curPos += 4;
													}*/
						}

						{
							/*var scale = batchItem.data.Length / (batchItem.height * batchItem.width);
													var height = batchItem.height;
													var width = batchItem.width;
													var pixelData = batchItem.data;
													for (int y = 0; y < height; y += scale)
													{
														for (int x = 0; x < width; x += scale)
														{
															for (int j = 0; j < scale; j++)
															{
																for (int i = 0; i < scale; i++)
																{
																	int offset = (y * width + x) * scale;
																	/*pixelData[offset + 0] = color.B;
																	pixelData[offset + 1] = color.G;
																	pixelData[offset + 2] = color.R;
																	pixelData[offset + 3] = alpha;#1#
																	var color = new UnityEngine.Color (pixelData[offset + 2], pixelData[offset + 1], pixelData[offset + 0], pixelData[offset + 3]);
																	mainTexture.SetPixel (x + i, y + j, color);
																}
															}
														}
													}*/
						}
					}
				}
			}

			/*background.LoadRawTextureData (texturePixels);
			//mainTexture.SetPixelData (ByteArray_2To1 (array2d_Screen, screenWidth, screenHeight), 0, 0);
			background.Apply ();
			DrawTextureToTarget (background);*/
			SpriteBatcher.Instance.CompleteMesh ();
		}

		private int[,] ByteArray_1To2 (byte[] src, int width, int height)
		{
			var array2d = new int[width, height];
			int count = src.Length / 4;
			var array1d = new int[count];

			for (int i = 0; i < count; i++)
			{
				var value = src[i * 4] + src[i * 4 + 1] << 8 + src[i * 4 + 2] << 16 + src[i * 4 + 3] << 24;
				//array1d[i] = value;

				array2d[i / height, i % height] = value;
			}

			return array2d;


			/*byte[] buf = new byte[width * height * 4];

			int curPos = 0;
			for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				temp[i, j] = new byte[,];
				Color curPixel = bmp.GetPixel (j, i);
				buf[curPos] = curPixel.B;
				buf[curPos + 1] = curPixel.G;
				buf[curPos + 2] = curPixel.R;
				buf[curPos + 3] = curPixel.A;
				curPos += 4;
			}*/
		}

		private byte[] ByteArray_2To1 (int[,] src, int width, int height)
		{
			/*for (int i = 0; i < src.Length; i++)
			{
				var value = src[i * 4] + src[i * 4 + 1] << 8 + src[i * 4 + 2] << 16 + src[i * 4 + 3] << 24;
				//array1d[i] = value;
				
				array2d[i / height, i % height] = value;
			}

			return array2d;*/

			byte[] buf = new byte[width * height * 4];

			int curPos = 0;
			for (int i = 0; i < height; i++)
			for (int j = 0; j < width; j++)
			{
				var value = src[j, i];
				buf[curPos] = (byte)value;
				value >>= 8;
				buf[curPos + 1] = (byte)value;
				value >>= 8;
				buf[curPos + 2] = (byte)value;
				value >>= 8;
				buf[curPos + 3] = (byte)value;
				curPos += 4;
			}

			return buf;
		}

		private byte[] GetPixelByteArray (byte[] src, int startposX, int startposY, int width, int height)
		{
			var result = new byte[4];
			var startIndex = (width * startposY + startposX) * 4;
			result[0] = src[startIndex];
			result[1] = src[startIndex + 1];
			result[2] = src[startIndex + 2];
			result[3] = src[startIndex + 3];
			return result;
		}

		private void SetPixelByteArray (byte[] source, byte[] destination, int startposX, int startposY, int width, int height)
		{
			var startIndex = (width * startposY + startposX) * 4;
			Array.Copy (source, 0, destination, startIndex, source.Length);
		}
	}
}