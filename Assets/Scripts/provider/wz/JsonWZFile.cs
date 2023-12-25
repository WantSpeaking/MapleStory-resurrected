using constants.skills;
using FairyGUI;
using MapleLib.WzLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace provider.wz
{
    public class JsonWZFile : MapleDataProvider
    {
		//public MapleDataDirectoryEntry Root => throw new NotImplementedException();

		public MapleData this[string imgPath]
		{
			get
			{
                if (!datas.TryGetValue(imgPath,out var mapleData))
                {
					mapleData = getData(imgPath);
					datas.Add(imgPath, mapleData);
				}
				return mapleData;
			}
		}

		public MapleData getData(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath)) return null;
            if (!imgPath.Contains(".img")) return null;

            ParseImgJson(imgPath);
            var jaonMapleData = new JsonMapleData(imgJOb, imgPath);
            return jaonMapleData;
        }

        public string wzPath;
        private bool parsed;
        private JObject imgJOb;
        public JsonWZFile (string wzFileName)
        {
            wzFileName = wzFileName.Replace("/", "");
            wzPath = System.IO.Path.Combine(ms.Constants.Instance.path_MapleStoryFolder, "Json", wzFileName);
        }

        private void ParseImgJson(string imgPath = "")
        {
            var fullpath = System.IO.Path.Combine(wzPath, $"{imgPath}.json");
            using (StreamReader r = new StreamReader(fullpath))
            {
                var jasonText = r.ReadToEnd();
                imgJOb = JObject.Parse(jasonText);
            }
        }

        private Dictionary<string,MapleData> datas = new Dictionary<string, MapleData>();
    }
}
