using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace config
{
	public class YamlConfig
	{
		public static YamlConfig config = fromFile ("config.yaml");

		public List<WorldConfig> worlds = new List<WorldConfig>();
		public ServerConfig server = new ServerConfig();

		public static YamlConfig fromFile (string filename)
		{
			return new YamlConfig (); 
        } 
    }
}
