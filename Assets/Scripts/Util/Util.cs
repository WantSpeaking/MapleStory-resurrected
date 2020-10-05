namespace ms
{
	public static class Util<T>
	{
		public static T lerp(T first, T second, float alpha)
		{
			return alpha <= 0.0f ? first
				: alpha >= 1.0f ? second
				: (dynamic)first == second ? first
				: (dynamic)(1.0f - alpha) * first + (dynamic)alpha * second;
		}
	}
}