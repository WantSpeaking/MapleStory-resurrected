﻿

using System;
using System.Text.RegularExpressions;

using Loxodon.Log;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Prefs;
using Loxodon.Framework.Asynchronous;
using Loxodon.Framework.Commands;
using Loxodon.Framework.ViewModels;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Examples;
using ms;

namespace ms_Unity
{
    public class LoginViewModel : ViewModelBase
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ViewModelBase));

        private const string LAST_USERNAME_KEY = "LAST_USERNAME";

        private ObservableDictionary<string, string> errors = new ObservableDictionary<string, string>();
        private string username;
        private string password;
        private SimpleCommand loginCommand;
        private SimpleCommand cancelCommand;

        private Loxodon.Framework.Examples.Account account;

        private Preferences globalPreferences;
        private IAccountService accountService;
        private Localization localization;

        private InteractionRequest interactionFinished;
        private InteractionRequest<Notification> toastRequest;

        public LoginViewModel(IAccountService accountService, Localization localization, Preferences globalPreferences)
        {
            this.localization = localization;
            this.accountService = accountService;
            this.globalPreferences = globalPreferences;

            this.interactionFinished = new InteractionRequest(this);
            this.toastRequest = new InteractionRequest<Notification>(this);

            if (this.username == null)
            {
                this.username = globalPreferences.GetString(LAST_USERNAME_KEY, "");
            }

            this.loginCommand = new SimpleCommand(this.Login);
            this.cancelCommand = new SimpleCommand(() =>
            {
                this.interactionFinished.Raise();/* Request to close the login window */
            });
        }

        public IInteractionRequest InteractionFinished
        {
            get { return this.interactionFinished; }
        }

        public IInteractionRequest ToastRequest
        {
            get { return this.toastRequest; }
        }

        public ObservableDictionary<string, string> Errors { get { return this.errors; } }

        public string Username
        {
            get { return this.username; }
            set
            {
                if (this.Set<string>(ref this.username, value, "Username"))
                {
                    this.ValidateUsername();
                }
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.Set<string>(ref this.password, value, "Password"))
                {
                    this.ValidatePassword();
                }
            }
        }

        private bool ValidateUsername()
        {
            if (string.IsNullOrEmpty(this.username) || !Regex.IsMatch(this.username, "^[a-zA-Z0-9_-]{4,12}$"))
            {
                this.errors["username"] = localization.GetText("login.validation.username.error", "Please enter a valid username.");
                return false;
            }
            else
            {
                this.errors.Remove("username");
                return true;
            }
        }

        private bool ValidatePassword()
        {
            if (string.IsNullOrEmpty(this.password) || !Regex.IsMatch(this.password, "^[a-zA-Z0-9_-]{4,12}$"))
            {
                this.errors["password"] = localization.GetText("login.validation.password.error", "Please enter a valid password.");
                return false;
            }
            else
            {
                this.errors.Remove("password");
                return true;
            }
        }

        public ICommand LoginCommand
        {
            get { return this.loginCommand; }
        }

        public ICommand CancelCommand
        {
            get { return this.cancelCommand; }
        }

        public Loxodon.Framework.Examples.Account Account
        {
            get { return this.account; }
        }

        public async void Login()
        {
            try
            {
                /*                if (log.IsDebugEnabled)
                                    log.DebugFormat("login start. username:{0} password:{1}", this.username, this.password);

                                this.account = null;
                                this.loginCommand.Enabled = false;*//*by databinding, auto set button.interactable = false. *//*
                                if (!(this.ValidateUsername() && this.ValidatePassword()))
                                    return;

                                var result = this.accountService.Login(this.username, this.password);
                                var account = await result;
                                if (result.Exception != null)
                                {
                                    if (log.IsErrorEnabled)
                                        log.ErrorFormat("Exception:{0}", result.Exception);

                                    var tipContent = this.localization.GetText("login.exception.tip", "Login exception.");
                                    this.toastRequest.Raise(new Notification(tipContent));*//* show toast *//*
                                    return;
                                }

                                if (account != null)
                                {
                                    *//* login success *//*
                                    globalPreferences.SetString(LAST_USERNAME_KEY, this.username);
                                    globalPreferences.Save();
                                    this.account = account;
                                    this.interactionFinished.Raise();*//* Interaction completed, request to close the login window *//*
                                }
                                else
                                {
                                    *//* Login failure *//*
                                    var tipContent = this.localization.GetText("login.failure.tip", "Login failure.");
                                    this.toastRequest.Raise(new Notification(tipContent));*//* show toast *//*
                                }*/

                this.loginCommand.Enabled = false;

                string account_text = "admin";
                string password_text = "admin";

                new LoginPacket (account_text, password_text).dispatch ();
                this.interactionFinished.Raise ();
                AppDebug.Log ("loginViewModel");
            }
            finally
            {
                this.loginCommand.Enabled = true;/*by databinding, auto set button.interactable = true. */
            }
        }

        public IAsyncResult<Loxodon.Framework.Examples.Account> GetAccount()
        {
            return this.accountService.GetAccount(this.Username);
        }
    }
}