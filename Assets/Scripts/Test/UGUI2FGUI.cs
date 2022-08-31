using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UGUI2FGUI : MonoBehaviour
{
	class PackageItem
	{
		public string type;

		public string name { get; set; }
		public string data {get; set; }

		public string id { get;  set; }
		public string scale { get;  set; }
		public bool scale9Grid { get;  set; }

	}
	class UIPackage
	{
		public string id { get; set; }
		public string itemIdBase { get; set; }
		public int nextItemIndex { get; set; }
		public string basePath { get; set; }
		public string getNextItemId => this.itemIdBase + System.Convert.ToString ((this.nextItemIndex++), 36);
		public List<PackageItem> resources = new List<PackageItem> ();
		public Dictionary<string,PackageItem> sameDataTestHelper = new Dictionary<string, PackageItem> ();
		public Dictionary<string, int> sameNameTestHelper = new Dictionary<string, int> ();
		
		public UIPackage (string basePath, string buildId)
		{
			this.id = buildId.Substring (0, 8);
			this.itemIdBase = buildId.Substring (8);
			this.nextItemIndex = 0;


			this.basePath = basePath;
			//fs.ensureDirSync (basePath);

			/*this.resources = [];
			this.sameDataTestHelper = { };
			this.sameNameTestHelper = { };*/
		}
	}

	XmlDocument xml_Package;
	XmlDocument xml_Component;
	// Start is called before the first frame update
	void Start ()
	{
		XmlDocument xml_Package = new XmlDocument ();
		XmlDocument xml_Component = new XmlDocument ();
	}

	// Update is called once per frame
	void Update ()
	{

	}

	/**
 * Convert a PSD file to a fairygui package.
 * @param {string} psdFile path of the psd file.
 * @param {string} outputFile optional. output file path.
 * @param {integer} option psd2fgui.constants.
 * @param {string} buildId optinal. You can use same build id to keep resource ids unchanged during multiple converting for a psd file.
 * @return {string} output file path.
 */
	void convert (/*psdFile, outputFile, option,*/string buildId)
	{
		RectTransform root = null;
		/*if (!option)
			option = 0;*/
		//if (!buildId)
		buildId = genBuildId ();

		//var pathInfo = path.parse (psdFile);
		string outputDirectory = string.Empty;

		/*		if (option & exports.constants.NO_PACK)
				{
					outputDirectory = outputFile;
					if (!outputDirectory)
						outputDirectory = path.join (pathInfo.dir, pathInfo.name + "-fairypackage");
				}
				else
				{
					outputDirectory = path.join (pathInfo.dir, pathInfo.name + "~temp");
					fs.emptyDirSync (outputDirectory);

					if (!outputFile)
						outputFile = path.join (pathInfo.dir, pathInfo.name + ".fairypackage");
				}*/

		/*		var psd = PSD.fromFile (psdFile);
				psd.parse ();*/

		var targetPackage = new UIPackage (outputDirectory, buildId);
		//targetPackage.exportOption = option;

		createComponent (root, root.name);

		var pkgDesc = xml_Package.CreateElement ("packageDescription");
		xml_Package.AppendChild (pkgDesc);
		pkgDesc.SetAttribute ("id", targetPackage.id);

		var resourcesNode = xml_Package.CreateElement ("resources");
		pkgDesc.AppendChild (resourcesNode);

		//var savePromises = [];

		targetPackage.resources.ForEach ((item) =>
		{
			var resNode = xml_Package.CreateElement (item.type);
			resourcesNode.AppendChild (resNode);

			resNode.SetAttribute ("id", item.id);
			resNode.SetAttribute ("name", item.name);
			resNode.SetAttribute ("path", "/");
			if (item.type == "image")
			{
				if (item.scale9Grid)
				{
					resNode.SetAttribute ("scale", item.scale);
					resNode.SetAttribute ("scale9Grid", item.scale9Grid.ToString ());
				}
			}

			/*if (item.type == "image")
				savePromises.push (item.data.saveAsPng (path.join (targetPackage.basePath, item.name)));
			else
				savePromises.push (fs.writeFile (path.join (targetPackage.basePath, item.name), item.data));*/
		});

		/*		savePromises.push (fs.writeFile (path.join (targetPackage.basePath, "package.xml"),
				pkgDesc.end ({ pretty: true })));*/

		/*			var pa = Promise.all (savePromises);
					if (option & exports.constants.NO_PACK)
					{
						pa.then (function () {
							console.log (psdFile + "->" + outputDirectory);
							resolve (buildId);
						}).catch (function (reason) {
							reject (reason);
						});
						}

				else
						{
							pa.then (function () {
								return fs.readdir (outputDirectory);
							}).then (function (files) {
								var output = fs.createWriteStream (outputFile);
								output.on ("close", function () {
									fs.emptyDirSync (outputDirectory);
									fs.rmdirSync (outputDirectory);

									console.log (psdFile + "->" + outputFile);
									resolve (buildId);
								});

								var zipArchiver = archiver ("zip");
								zipArchiver.pipe (output);
								files.forEach (function (ff) {
									zipArchiver.file (path.join (outputDirectory, ff), { "name": ff });
								});
								zipArchiver.finalize ();
							}).catch (function (reason) {
								reject (reason);
							});
							}
						});*/
	}

	/*	UIPackage createUIPackage (string basePath, string buildId)
		{
			var targetPackage = new UIPackage (basePath, buildId);

			targetPackage.id = buildId.substr (0, 8);
			targetPackage.itemIdBase = buildId.Substring (8);
			targetPackage.nextItemIndex = 0;
			targetPackage.getNextItemId = function () {
				return targetPackage.itemIdBase + (targetPackage.nextItemIndex++).toString (36);
			};

			targetPackage.basePath = basePath;
			fs.ensureDirSync (basePath);

			targetPackage.resources = [];
			targetPackage.sameDataTestHelper = { };
			targetPackage.sameNameTestHelper = { };

			return targetPackage;
		}*/

	PackageItem createImage (RectTransform aNode, float scale9Grid)
	{
		var packageItem = createPackageItem ("image", aNode.name + ".png", aNode.name);
		if (scale9Grid != 0)
		{
			packageItem.scale = "9grid";
			packageItem.scale9Grid = scale9Grid!=0;
		}

		return packageItem;
	}


	PackageItem createComponent (RectTransform aNode, string name)
	{
		var component = xml_Component.CreateElement ("component");
		xml_Component.AppendChild (component);
		component.SetAttribute ("size", $"{aNode.rect.width},{aNode.rect.height}");

		var displayList = xml_Component.CreateElement ("displayList");
		component.AppendChild (displayList);

		var cnt = aNode.childCount;
		for (var i = cnt - 1; i >= 0; i--)
		{
			parseNode (aNode.GetChild (i) as RectTransform, aNode, displayList);
		}


		/*component.att ("size", aNode.get ("width") + "," + aNode.get ("height"));
		var displayList = component.ele ("displayList");

		var cnt = aNode.children ().length;
		for (var i = cnt - 1; i >= 0; i--)
		{
			parseNode (aNode.children ()[i], aNode, displayList);
		}*/

		return createPackageItem ("component", (name != null ? name : aNode.name) + ".xml",null);
	}

	void createButton (Transform aNode, string instProps)
	{
		/*var component = xmlbuilder.create ("component");
		component.att ("size", aNode.get ("width") + "," + aNode.get ("height"));
		component.att ("extention", "Button");

		var images = [];
		var imagePages = [];
		var imageCnt = 0;
		aNode.descendants ().forEach (function (childNode) {
			var nodeName = childNode.get ("name");
		for (var i in buttonStatusSuffix)
			{
				if (nodeName.indexOf (buttonStatusSuffix[i]) != -1)
				{
					images[i] = childNode;
					imageCnt++;
				}
			};
		});
	for (var i in buttonStatusSuffix)
		{
			imagePages[i] = [];
			if (!images[i])
			{
				if (i == 3 && images[1]) //if no "selectedOver", use "down"
					imagePages[1].push (i);
				else //or else, use "up"
					imagePages[0].push (i);
			}
			else
			{
				imagePages[i].push (i);
			}
		}

		var onElementCallback = function (child, node) {
			var nodeName = node.get ("name");
			var j = images.indexOf (node);
			if (j != -1)
			{
				var gear = child.ele ("gearDisplay");
				gear.att ("controller", "button");
				gear.att ("pages", imagePages[j].join (","));
			}

			if (nodeName.indexOf ("@title") != -1)
			{
				if (child.attributes["text"])
				{
					instProps["@title"] = child.attributes["text"].value;
					child.removeAttribute ("text");
				}
			}
			else if (nodeName.indexOf ("@icon") != -1)
			{
				if (child.attributes["url"])
				{
					instProps["@icon"] = child.attributes["url"].value;
					child.removeAttribute ("url");
				}
			}
		};

		var controller = component.ele ("controller");
		controller.att ("name", "button");
		controller.att ("pages", "0,up,1,down,2,over,3,selectedOver");

		var displayList = component.ele ("displayList");
		var cnt = aNode.children ().length;
		for (i = cnt - 1; i >= 0; i--)
		{
			parseNode (aNode.children ()[i], aNode, displayList, onElementCallback);
		}

		var extension = component.ele ("Button");
		if (aNode.get ("name").indexOf (checkButtonPrefix) == 0)
		{
			extension.att ("mode", "Check");
			instProps["@checked"] = "true";
		}
		else if (aNode.get ("name").indexOf (radioButtonPrefix) == 0)
			extension.att ("mode", "Radio");

		if (imageCnt == 1)
		{
			extension.att ("downEffect", "scale");
			extension.att ("downEffectValue", "1.1");
		}

		return createPackageItem ("component", aNode.get ("name") + ".xml", component.end ({ pretty: true }));*/
	}

	UIPackage targetPackage;

	class TestItem
	{
		public string type;
		public string id;
	}
	PackageItem createPackageItem (string type, string fileName, string data)
	{
		string dataForHash;
		if (type == "image")
		{ } //data should a psd layer
			//dataForHash = Buffer.from (data.get ("image").pixelData);
		else
			dataForHash = data;
		var hash = "";
	/*	var hash = crypto.createHash ("md5").update (dataForHash).digest ("hex");*/
		var item = targetPackage.sameDataTestHelper[hash];
		if (item == null)
		{
			item = new PackageItem();
			item.type = type;
			item.id = targetPackage.getNextItemId;

			var i = fileName.LastIndexOf (".");
			var basename = fileName.Substring (0, i);
			var ext = fileName.Substring (i);
			//var str = "/[\@\"\"\\\/\b\f\n\r\t\$\%\*\:\?\<\>\|]/g";
			//basename = basename.Replace (, "_");
			while (true)
			{
				var j = targetPackage.sameNameTestHelper[basename];
				if (j == 0)
				{
					targetPackage.sameNameTestHelper[basename] = 1;
					break;
				}
				else
				{
					targetPackage.sameNameTestHelper[basename] = j + 1;
					basename = basename + "_" + j;
				}
			}
			fileName = basename + ext;
			item.name = fileName;
			item.data = data;
			targetPackage.resources.Add (item);
			targetPackage.sameDataTestHelper[hash] = item;
		}

		return item;
	}

	XmlElement parseNode (RectTransform aNode, RectTransform rootNode, XmlElement displayList, Action onElementCallback = null)
	{
		XmlElement child = null;
		/*		
				var packageItem;
				var instProps;
				var str;

				var nodeName = aNode.name;
				var specialUsage;
				if (nodeName.IndexOf ("@title") != -1)
					specialUsage = "title";
				else if (nodeName.IndexOf ("@icon") != -1)
					specialUsage = "icon";

				if (aNode.isGroup ())
				{
					if (nodeName.indexOf (componentPrefix) == 0)
					{
						packageItem = createComponent (aNode);
						child = xmlbuilder.create ("component");
						str = "n" + (displayList.children.length + 1);
						child.att ("id", str + "_" + targetPackage.itemIdBase);
						child.att ("name", specialUsage ? specialUsage : str);
						child.att ("src", packageItem.id);
						child.att ("fileName", packageItem.name);
						child.att ("xy", (aNode.left - rootNode.left) + "," + (aNode.top - rootNode.top));
					}
					else if (nodeName.indexOf (commonButtonPrefix) == 0 || nodeName.indexOf (checkButtonPrefix) == 0 || nodeName.indexOf (radioButtonPrefix) == 0)
					{
						instProps = { };
						packageItem = createButton (aNode, instProps);
						child = xmlbuilder.create ("component");
						str = "n" + (displayList.children.length + 1);
						child.att ("id", str + "_" + targetPackage.itemIdBase);
						child.att ("name", specialUsage ? specialUsage : str);
						child.att ("src", packageItem.id);
						child.att ("fileName", packageItem.name);
						child.att ("xy", (aNode.left - rootNode.left) + "," + (aNode.top - rootNode.top));
						child.ele ({ Button: instProps });
					}
					else
					{
						var cnt = aNode.children ().length;
						for (var i = cnt - 1; i >= 0; i--)
							parseNode (aNode.children ()[i], rootNode, displayList, onElementCallback);
					}
				}
				else
				{
					var typeTool = aNode.get ("typeTool");
					if (typeTool)
					{
						child = xmlbuilder.create ("text");
						str = "n" + (displayList.children.length + 1);
						child.att ("id", str + "_" + targetPackage.itemIdBase);
						child.att ("name", specialUsage ? specialUsage : str);
						child.att ("text", typeTool.textValue);
						if (specialUsage == "title")
						{
							child.att ("xy", "0," + (aNode.top - rootNode.top - 4));
							child.att ("size", rootNode.width + "," + (aNode.height + 8));
							child.att ("align", "center");
						}
						else
						{
							child.att ("xy", (aNode.left - rootNode.left - 4) + "," + (aNode.top - rootNode.top - 4));
							child.att ("size", (aNode.width + 8) + "," + (aNode.height + 8));
							str = typeTool.alignment ()[0];
							if (str != "left")
								child.att ("align", str);
						}
						child.att ("vAlign", "middle");
						child.att ("autoSize", "none");
						if (!(targetPackage.exportOption & exports.constants.IGNORE_FONT))
							child.att ("font", typeTool.fonts ()[0]);
						child.att ("fontSize", typeTool.sizes ()[0]);
						child.att ("color", convertToHtmlColor (typeTool.colors ()[0]));
					}
					else if (!aNode.isEmpty ())
					{
						packageItem = createImage (aNode);
						if (specialUsage == "icon")
							child = xmlbuilder.create ("loader");
						else
							child = xmlbuilder.create ("image");
						str = "n" + (displayList.children.length + 1);
						child.att ("id", str + "_" + targetPackage.itemIdBase);
						child.att ("name", specialUsage ? specialUsage : str);
						child.att ("xy", (aNode.left - rootNode.left) + "," + (aNode.top - rootNode.top));
						if (specialUsage == "icon")
						{
							child.att ("size", aNode.width + "," + aNode.height);
							child.att ("url", "ui://" + targetPackage.id + packageItem.id);
						}
						else
							child.att ("src", packageItem.id);
						child.att ("fileName", packageItem.name);
					}
				}

				if (child)
				{
					var opacity = aNode.get ("opacity");
					if (opacity < 255)
						child.att ("alpha", (opacity / 255).toFixed (2));

					if (onElementCallback)
						onElementCallback (child, aNode);

					displayList.importDocument (child);
				}*/

		return child;
	}
	string genBuildId ()
	{

		var magicNumber = System.Convert.ToString ((int)(UnityEngine.Random.value * 36), 36).Substring (0, 1);
		var s1 = "0000" + System.Convert.ToString ((int)(UnityEngine.Random.value * 1679616), 36);
		var s2 = "000" + System.Convert.ToString ((int)(UnityEngine.Random.value * 46656), 36);
		var count = 0d;
		for (var i = 0; i < 4; i++)
		{
			var c = Math.Floor (UnityEngine.Random.value * 26);
			count += Math.Pow (26, i) * (c + 10);
		}
		count += Math.Floor (UnityEngine.Random.value * 1000000) + Math.Floor (UnityEngine.Random.value * 222640);

		return magicNumber + s1.Substring (s1.Length - 4) + s2.Substring (s2.Length - 3) + System.Convert.ToString ((int)count, 36);
	}

}
