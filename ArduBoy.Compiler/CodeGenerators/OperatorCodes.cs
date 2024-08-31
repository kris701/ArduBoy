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
			{ ":mod", 12 },

			{ ":draw-circle", 20 },
			{ ":draw-fill-circle", 21 },
			{ ":draw-triangle", 22 },
			{ ":draw-fill-triangle", 23 },
			{ ":draw-rect", 24 },
			{ ":draw-fill-rect", 25 },
			{ ":draw-text", 26 },
			{ ":draw-fill", 27 },

			{ "==", 30 },
			{ "<", 31 },
			{ ">", 32 },
			{ "!=", 33 },
		};

		public static string GetByteCode(string id)
		{
#if DEBUG
			return id;
#else
			return $"{Convert.ToChar(_opCodes[id.ToLower()] + 33)}";
#endif
		}
	}
}
