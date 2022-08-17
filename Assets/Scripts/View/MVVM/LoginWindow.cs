﻿

using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Views;
using Loxodon.Log;
using UnityEngine.UI;
using InputField = UnityEngine.UI.InputField;
using Text = UnityEngine.UI.Text;
using Button = UnityEngine.UI.Button;
using TMPro;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Examples;

namespace ms_Unity
{
    public partial class LoginWindow : Loxodon.Framework.Views.Window
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(LoginWindow));

        public TMP_InputField username;
        public TMP_InputField password;
        public UnityEngine.UI.Text usernameErrorPrompt;
        public UnityEngine.UI.Text passwordErrorPrompt;
        public UnityEngine.UI.Button confirmButton;
        public UnityEngine.UI.Button cancelButton;

        LoginViewModel viewModel;
        private Localization localization;

        protected override void OnCreate (IBundle bundle)
        {
			username = InputField_UserName;
			password = InputField_Password;
			confirmButton = Button_Login;

			ApplicationContext context = Context.GetApplicationContext ();
            this.localization = context.GetService<Localization> ();
            var accountService = context.GetService<IAccountService> ();
            var globalPreferences = context.GetGlobalPreferences ();

            this.viewModel = new LoginViewModel (accountService, localization, globalPreferences);

            BindingSet<LoginWindow, LoginViewModel> bindingSet = this.CreateBindingSet<LoginWindow, LoginViewModel> (viewModel);
            bindingSet.Bind ().For (v => v.OnInteractionFinished).To (vm => vm.InteractionFinished);
            bindingSet.Bind ().For (v => v.OnToastShow).To (vm => vm.ToastRequest);

            bindingSet.Bind (this.username).For (v => v.text, v => v.onEndEdit).To (vm => vm.Username).TwoWay ();
            //bindingSet.Bind(this.usernameErrorPrompt).For(v => v.text).To(vm => vm.Errors["username"]).OneWay();
            bindingSet.Bind (this.password).For (v => v.text, v => v.onEndEdit).To (vm => vm.Password).TwoWay ();
            //bindingSet.Bind(this.passwordErrorPrompt).For(v => v.text).To(vm => vm.Errors["password"]).OneWay();
            bindingSet.Bind (this.confirmButton).For (v => v.onClick).To (vm => vm.LoginCommand);
            //bindingSet.Bind(this.cancelButton).For(v => v.onClick).To(vm => vm.CancelCommand);
            bindingSet.Build ();
        }

        public virtual void OnInteractionFinished (object sender, InteractionEventArgs args)
        {
            this.Dismiss ();
        }

        public virtual void OnToastShow (object sender, InteractionEventArgs args)
        {
            Notification notification = args.Context as Notification;
            if (notification == null)
                return;

            Toast.Show (this, notification.Message, 2f);
        }
    }
}