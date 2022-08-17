using System;
using System.Collections.Generic;
using System.Linq;
using HaCreator.MapEditor;
using System.Runtime.InteropServices;
using MapleLib.WzLib;
using HaCreator.GUI;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using HaCreator.Wz;

namespace HaCreator
{
    static class Program
    {
        public static WzFileManager WzManager;
        public static WzInformationManager InfoManager = new WzInformationManager();
        public static WzSettingsManager SettingsManager;
        public static bool AbortThreads = false;
        public static bool Restarting;

        public const string APP_NAME = "HaCreator";


        public static string GetLocalSettingsFolder()
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string our_folder = Path.Combine(appdata, APP_NAME);
            if (!Directory.Exists(our_folder))
                Directory.CreateDirectory(our_folder);
            return our_folder;
        }

        public static string GetLocalSettingsPath()
        {
            return Path.Combine(GetLocalSettingsFolder(), "Settings.wz");
        }

        /// <summary>
        /// Allows customisation of display text during runtime..
        /// </summary>
        /// <param name="ci"></param>
        /// <returns></returns>
        private static CultureInfo GetMainCulture(CultureInfo ci)
        {
            if (!ci.Name.Contains("-"))
                return ci;
            switch (ci.Name.Split("-".ToCharArray())[0])
            {
                case "ko":
                    return new CultureInfo("ko");
                case "ja":
                    return new CultureInfo("ja");
                case "en":
                    return new CultureInfo("en");
                case "zh":
                    if (ci.EnglishName.Contains("Simplified"))
                        return new CultureInfo("zh-CHS");
                    else
                        return new CultureInfo("zh-CHT");
                default:
                    return ci;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //new ThreadExceptionDialog((Exception)e.ExceptionObject).ShowDialog();
            Environment.Exit(-1);
        }
    }
}

