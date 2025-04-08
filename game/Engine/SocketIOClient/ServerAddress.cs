using System.IO;
using System.Reflection;
using System.Text.Json;

public class ServerAddressReader
{
	public class ServerAddress
	{
		public string Location { get; set; }
		public string Path { get; set; }
	}

	public class ServerLocations
	{
		public ServerAddress Debug { get; set; }
		public ServerAddress Release { get; set; }
	}

	public static ServerAddress Read()
	{
		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Content", "ServerLocation.txt");
		using StreamReader reader = new StreamReader(path);
		var json = reader.ReadToEnd();
		JsonSerializerOptions options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		ServerLocations serverLocations = JsonSerializer.Deserialize<ServerLocations>(json, options);
		
		#if DEBUG
		return serverLocations.Debug;
		#else
		return serverLocations.Release;
		#endif
	}
}