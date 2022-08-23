using UnityEngine;
using System.Globalization;
using System.Collections;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Views;
using Loxodon.Log;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Services;
using Loxodon.Framework.Messaging;
using Loxodon.Framework.Examples;
using System.Collections.Generic;
using Loxodon.Framework.Binding.Converters;
using FairyGUI;
using Window = Loxodon.Framework.Views.Window;
using System;
using ms;
using Texture = UnityEngine.Texture;

namespace ms_Unity
{
	public class FGUI_Manager : SingletonMono<FGUI_Manager>
	{
		//private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		public Texture playerRenderTexture;
		private ApplicationContext context;
		ISubscription<WindowStateEventArgs> subscription;
		void Awake ()
		{
			GlobalWindowManager windowManager = FindObjectOfType<GlobalWindowManager> ();
			if (windowManager == null)
				throw new NotFoundException ("Not found the GlobalWindowManager.");

			context = Context.GetApplicationContext ();

			IServiceContainer container = context.GetContainer ();

			/* Initialize the data binding service */
			BindingServiceBundle bundle = new BindingServiceBundle (context.GetContainer ());
			bundle.Start ();

			//初始化支持FairyGUI的数据绑定相关组件，请在BindingServiceBundle启动后执行
			FairyGUIBindingServiceBundle fairyGUIBindingServiceBundle = new FairyGUIBindingServiceBundle (container);
			fairyGUIBindingServiceBundle.Start ();

			/* Initialize the ui view locator and register UIViewLocator */
			container.Register<IUIViewLocator> (new ResourcesViewLocator ());

			/* Initialize the localization service */
			//CultureInfo cultureInfo = Locale.GetCultureInfoByLanguage (SystemLanguage.English);
			CultureInfo cultureInfo = Locale.GetCultureInfo ();
			var localization = Localization.Current;
			localization.CultureInfo = cultureInfo;
			localization.AddDataProvider (new ResourcesDataProvider ("LocalizationExamples", new XmlDocumentParser ()));

			/* register Localization */
			container.Register<Localization> (localization);

			/* register AccountRepository */
			IAccountRepository accountRepository = new AccountRepository ();
			container.Register<IAccountService> (new AccountService (accountRepository));

			IConverterRegistry converterRegistry = container.Resolve<IConverterRegistry> ();
			converterRegistry.Register (typeof (ItemId_Texture_Converter).Name, new ItemId_Texture_Converter ());
			converterRegistry.Register (typeof (ItemId_NTexture_Converter).Name, new ItemId_NTexture_Converter ());

			/* Enable window state broadcast */
			GlobalSetting.enableWindowStateBroadcast = true;
			/* 
			 * Use the CanvasGroup.blocksRaycasts instead of the CanvasGroup.interactable 
			 * to control the interactivity of the view
			 */
			GlobalSetting.useBlocksRaycastsInsteadOfInteractable = true;

			/* Subscribe to window state change events */
			/*   subscription = Loxodon.Framework.Views.Window.Messenger.Subscribe<WindowStateEventArgs> (e =>
			   {
				   Debug.LogFormat ("The window[{0}] state changed from {1} to {2}", e.Window.Name, e.OldState, e.State);
			   });*/
			UIPackage.AddPackage ("UI/ms_Unity");

			ms_UnityBinder.BindAll ();

		}

		private void Start ()
		{
			//OpenFGUI<FGUI_Inventory> ();

			/*var window = (GComponent)UIPackage.CreateObject (packageName, "InventoryListItemView");
			GRoot.inst.AddChild (window);*/

			fgui_StateLogin = FGUI_StateLogin.CreateInstance ();
			fgui_StateGame = FGUI_StateGame.CreateInstance ();
		}
		/*        IEnumerator Start ()
				{
					*//* Create a window container *//*
					WindowContainer winContainer = WindowContainer.Create ("MAIN");

					yield return null;

					IUIViewLocator locator = context.GetService<IUIViewLocator> ();
					LoginWindow window = locator.LoadWindow<LoginWindow> (winContainer, "UI/LoginWindow");
					window.Create ();
					ITransition transition = window.Show ().OnStateChanged ((w, state) =>
					{
						//log.DebugFormat("Window:{0} State{1}",w.Name,state);
					});
					yield return transition.WaitForDone ();
				}*/

		private Dictionary<System.Type, Window> keyValuePairs = new Dictionary<System.Type, Window> ();
		private Dictionary<System.Type, GComponent> type_GComponent_dict = new Dictionary<System.Type, GComponent> ();
		private Dictionary<System.Type, MonoBehaviour> type_MonoB_dict = new Dictionary<System.Type, MonoBehaviour> ();

		public Window Open<T> () where T : Window
		{

			if (!keyValuePairs.TryGetValue (typeof (T), out var window))
			{
				IUIViewLocator locator = context.GetService<IUIViewLocator> ();
				window = locator.LoadWindow<T> ($"UI/{typeof (T).Name}");
				window.Create ();
			}

			window.Show ();

			return window;

		}

		public void OpenNew<Mono_View> () where Mono_View : MonoBehaviour
		{

			if (!type_MonoB_dict.TryGetValue (typeof (Mono_View), out var mono_View))
			{
				GameObject tempGobj = new GameObject (typeof (Mono_View).Name);
				mono_View = tempGobj.AddComponent (typeof (Mono_View)) as MonoBehaviour;


				/*	var fg_View = (GComponent)UIPackage.CreateObject (packageName, name);
					GRoot.inst.AddChild (fg_View);*/

				type_MonoB_dict[typeof (Mono_View)] = mono_View;
			}


			//return window;

		}
		public void OpenNew<FG_View, Mono_View> () where FG_View : GComponent, new() where Mono_View : MonoBehaviour
		{

			if (!type_MonoB_dict.TryGetValue (typeof (Mono_View), out var mono_View))
			{
				GameObject tempGobj = new GameObject (typeof (Mono_View).Name);
				mono_View = tempGobj.AddComponent (typeof (Mono_View)) as MonoBehaviour;


				/*	var fg_View = (GComponent)UIPackage.CreateObject (packageName, name);
					GRoot.inst.AddChild (fg_View);*/

				type_MonoB_dict[typeof (Mono_View)] = mono_View;
			}


			//return window;

		}
		[NonSerialized]
		public string packageName = "ms_Unity";

		[NonSerialized]
		public string classPrefix = "FGUI_";

		public GComponent OpenFGUI<FG_View, Mono_View> () where FG_View : GComponent, new() where Mono_View : MonoBehaviour
		{
			if (!type_GComponent_dict.TryGetValue (typeof (FG_View), out var window))
			{
				var viewType = typeof (FG_View);
				var name = viewType.Name;
				if (name.StartsWith (classPrefix))
				{
					name = name.Remove (0, classPrefix.Length);
				}
				window = (GComponent)UIPackage.CreateObject (packageName, name);
				GRoot.inst.AddChild (window);
				window.displayObject.gameObject.AddComponent (typeof (Mono_View));
				type_GComponent_dict[viewType] = window;
			}

			return window;
		}

/*		public FG_View GetFGUI<FG_View> () where FG_View : GComponent, new()
		{
			type_GComponent_dict.TryGetValue (typeof (FG_View), out var window);
			return (FG_View)window;
		}*/

		public FG_View OpenFGUI<FG_View> () where FG_View : GComponent, new()
		{
			if (!type_GComponent_dict.TryGetValue (typeof (FG_View), out var window))
			{
				var viewType = typeof (FG_View);
				var name = viewType.Name;
				if (name.StartsWith (classPrefix))
				{
					name = name.Remove (0, classPrefix.Length);
				}
				window = (GComponent)UIPackage.CreateObject (packageName, name);
				
				type_GComponent_dict[viewType] = window;
				FGUI_Window w = new FGUI_Window ();
				
			}
			GRoot.inst.AddChild (window);
			window.visible = true;

			return (FG_View)window;
		}

		public FG_View CloseFGUI<FG_View> () where FG_View : GComponent, new()
		{
			if (!type_GComponent_dict.TryGetValue (typeof (FG_View), out var window))
			{
				/*var viewType = typeof (FG_View);
				var name = viewType.Name;
				if (name.StartsWith (classPrefix))
				{
					name = name.Remove (0, classPrefix.Length);
				}
				window = (GComponent)UIPackage.CreateObject (packageName, name);
				GRoot.inst.AddChild (window);
				type_GComponent_dict[viewType] = window;*/
				AppDebug.Log ($"CloseFGUI: {typeof (FG_View).Name} can't find");
				return null;
			}
			GRoot.inst.RemoveChild (window);
			window.visible = false;

			return (FG_View)window;
		}

		public FG_View GetFGUI<FG_View> () where FG_View : GComponent, new()
		{
			if (!type_GComponent_dict.TryGetValue (typeof (FG_View), out var window))
			{
				var viewType = typeof (FG_View);
				var name = viewType.Name;
				if (name.StartsWith (classPrefix))
				{
					name = name.Remove (0, classPrefix.Length);
				}
				window = (GComponent)UIPackage.CreateObject (packageName, name);

				type_GComponent_dict[viewType] = window;
				FGUI_Window w = new FGUI_Window ();

			}
			//GRoot.inst.AddChild (window);
			//window.visible = true;

			return (FG_View)window;
		}

		public FGUI_StateLogin fgui_StateLogin;
		public FGUI_StateGame fgui_StateGame;
		/*public void ShowNotice (string message, NoticeType t, Text.Alignment a = Text.Alignment.CENTER, int max = 0, int count = 0, System.Action<int> numhandler = null, System.Action<bool> yesnohandler = null)
				{
					var nocice = OpenFGUI<FGUI_Notice> () as FGUI_Notice;
					nocice.ShowNotice (message, t, a, max, count, numhandler, yesnohandler);
					nocice._tet_message.text = message;

				}*/
	}
}