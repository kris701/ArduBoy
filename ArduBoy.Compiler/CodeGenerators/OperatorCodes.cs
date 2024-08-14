namespace ArduBoy.Compiler.CodeGenerators
{
	public static class OperatorCodes
	{
		private static readonly Dictionary<string, int> _opCodes = new Dictionary<string, int>()
		{
			{ ":", -1 },
			{ "-", 0 },
			{ ":call", 1 },
			{ ":if", 2 },
			{ ":wait", 3 },
			{ ":set", 4 },
			{ ":audio", 5 },
			{ ":draw-line", 6 },
			{ ":goto", 7 },
			{ ":add", 8 },
			{ ":sub", 9 },
			{ ":mult", 10 },
			{ ":div", 11 },

			{ ":draw-circle", 12 },
			{ ":draw-fill-circle", 13 },
			{ ":draw-triangle", 14 },
			{ ":draw-fill-triangle", 15 },
			{ ":draw-rect", 16 },
			{ ":draw-fill-rect", 17 },
			{ ":draw-text", 18 },
			{ ":draw-fill", 19 },

			{ "==", 20 },
			{ "<", 21 },
			{ ">", 22 },
			{ "!=", 23 },
		};

		public static string GetByteCode(string id)
		{
#if DEBUG
			return id;
#endif
			return $"{Convert.ToChar(_opCodes[id.ToLower()] + 33)}";
		}
	}
}
