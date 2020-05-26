namespace BouncingBall
{
	/// <summary>
	/// Sreen format available in <see cref="Format"/>
	/// </summary>
	public enum EnumFormat : int
	{
		// 16/9
		_5PC,
		_6PC,
		_7PC,
		_8PC,
		_10PC,
		_12PC,
		_14PC,
		_15PC,
		_17PC,
		_19PC,
		_22PC,
		_24PC,

		// 4/3
		_22PC43
	}

	/// <summary>
	/// A classe use to get the real size of a screen
	/// </summary>
	public class Format
	{
		private static int[][] dimFormat =
		{
			// 16/9
			new int[] {111, 62 },	// _5PC
			new int[] {133, 75 },	// _6PC
			new int[] {155, 87 },	// _7PC
			new int[] {177, 100 },	// _8PC
			new int[] {221, 125 },	// _10PC
			new int[] {266, 149 },	// _12PC
			new int[] {310, 174 },	// _14PC
			new int[] {332, 187 },	// _15PC
			new int[] {376, 212 },	// _17PC
			new int[] {421, 237 },	// _19PC
			new int[] {487, 274 },	// _22PC
			new int[] {531, 299 },	// _24PC

			// 4/3
			new int[] {447, 335 },	// _22PC43
		};

		/// <summary> 
		/// Return the dimension of a screen in a 2 members Array
		/// </summary>
		/// <param name="scf">A value available in <see cref="EnumFormat"/></param>
		/// <returns>IntArray containing {width, hight} of a screen</returns>
		public static int[] GetFormat(EnumFormat scf)
		{
			return dimFormat[(int)scf];
		}
	}
}