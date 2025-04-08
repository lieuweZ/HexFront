using System.Collections.Generic;

namespace Blok3Game.Engine.Helpers
{
	public class GameSettingsManager
	{
		protected Dictionary<string, string> stringSettings;

		public GameSettingsManager()
		{
			stringSettings = new Dictionary<string, string>();
		}

		public void SetValue(string key, string value)
		{
			stringSettings[key] = value;
		}

		public string GetValue(string key)
		{
			if (stringSettings.ContainsKey(key))
			{
				return stringSettings[key];
			}
			else
			{
				return "";
			}
		}
	}
}