using System.IO;
using System.Reflection;
using System.Text.Json;
using System;

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
		// Allow runtime override so Itch builds can point to a non-HvA backend
		// without modifying game files each release.
		string envLocation = Environment.GetEnvironmentVariable("HEXFRONT_SERVER_URL");
		string envPath = Environment.GetEnvironmentVariable("HEXFRONT_SERVER_PATH");
		if (!string.IsNullOrWhiteSpace(envLocation))
		{
			return Normalize(new ServerAddress
			{
				Location = envLocation,
				Path = envPath ?? string.Empty
			});
		}

		string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Content", "ServerLocation.txt");
		using StreamReader reader = new StreamReader(path);
		var json = reader.ReadToEnd();
		JsonSerializerOptions options = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true
		};
		ServerLocations serverLocations = JsonSerializer.Deserialize<ServerLocations>(json, options);
		
		#if DEBUG
		return Normalize(serverLocations.Debug);
		#else
		return Normalize(serverLocations.Release);
		#endif
	}

	private static ServerAddress Normalize(ServerAddress address)
	{
		address.Location = (address.Location ?? string.Empty).TrimEnd('/');
		address.Path = address.Path ?? string.Empty;

		if (address.Path.Length > 0 && !address.Path.StartsWith("/"))
		{
			address.Path = "/" + address.Path;
		}

		return address;
	}
}
