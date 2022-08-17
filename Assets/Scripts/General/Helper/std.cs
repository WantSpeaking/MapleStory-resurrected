namespace Helper
{
	public class std
	{
		public struct div_t
		{
			public int rem;
			public int quot;
		}
		public static div_t div (int a, int b)
		{
			var result = new div_t ();
			result.quot = a / b;
			result.rem = a % b;
			return result;
		}
		
	}
}