using System.Reflection;

namespace ArduBoy.Compiler.Helpers
{
	public static class ResourceHelpers
	{
		public static string ReadEmbeddedFile(string target)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var fileStream = assembly.GetManifestResourceStream(target);
			if (fileStream == null)
				throw new ArgumentNullException($"Cannot read resource: {target}");
			using (Stream stream = fileStream)
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}

		public static bool EmbeddedFileExists(string target)
		{
			var assembly = Assembly.GetExecutingAssembly();
			var fileStream = assembly.GetManifestResourceStream(target);
			if (fileStream == null)
				return false;
			return true;
		}
	}
}
