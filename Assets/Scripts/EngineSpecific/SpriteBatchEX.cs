using Microsoft.Xna.Framework.Graphics;

namespace ms.Util
{
	public static class SpriteBatchEX
	{
		private static Texture2D _blankTexture;
		public static Texture2D BlankTexture (this SpriteBatch s)
		{
			if (_blankTexture == null)
			{
				_blankTexture = new Texture2D (s.GraphicsDevice, 1, 1);
				_blankTexture.SetData (new[] { Microsoft.Xna.Framework.Color.White });
			}

			return _blankTexture;
		}
	}
}