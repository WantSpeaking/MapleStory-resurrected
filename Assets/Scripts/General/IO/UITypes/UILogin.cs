#define USE_NX

using Beebyte.Obfuscator;
using MapleLib.WzLib;
using ms.Util;




namespace ms
{
    [Skip]
    public class UILogin : UIElement
    {
        public const Type TYPE = UIElement.Type.LOGIN;
        public const bool FOCUSED = false;
        public const bool TOGGLED = false;

        public UILogin(params object[] args) : this()
        {
        }

        public UILogin() : base(new Point_short(0, 0), new Point_short(800, 600))
        {
            //AppDebug.Log("UILogin().ctor");
            this.signboard_pos = new Point_short(389, 333);
            new LoginStartPacket().dispatch();

            string LoginMusicNewtro = Configuration.get().get_login_music_newtro();

            Music.get().play(LoginMusicNewtro);

            string version_text = Configuration.get().get_version();
            version = new Text(Text.Font.A11B, Text.Alignment.LEFT, Color.Name.LEMONGRASS, "Ver. " + version_text);

            WzObject map001 = ms.wz.wzFile_map["Back"]["login.img"];
            WzObject back = map001["back"];
            WzObject ani = map001["ani"];

            WzObject Login = ms.wz.wzFile_ui["Login.img"];
            WzObject UI_Title = Login["Title"];
            WzObject MapObj_Title = ms.wz.wzFile_map["Obj"]["login.img"]["Title"];

            WzObject Common = Login["Common"];

            WzObject prettyLogo = ms.wz.wzFile_UI_Endless["Game.img"]["logo"];
            WzObject frame = ms.wz.wzFile_map["Obj"]["login.img"]["Common"]["frame"]["0"]["0"];
            //WzObject frame = nl.nx.wzFile_map["Obj"]["login.img"]["Common"]["frame"]["0"];

            sprites.Add(new Sprite(back["11"], new Point_short(400, 300)));
            sprites.Add(new Sprite(ani["17"], new Point_short(165, 276)));
            sprites.Add(new Sprite(ani["18"], new Point_short(301, 245)));
            sprites.Add(new Sprite(ani["19"], new Point_short(374, 200)));
            sprites.Add(new Sprite(ani["19"], new Point_short(348, 161)));
            sprites.Add(new Sprite(back["35"], new Point_short(399, 260)));
            sprites.Add(new Sprite(prettyLogo, new Point_short(409, 144)));
            sprites.Add(new Sprite(MapObj_Title["signboard"]["0"]["0"], signboard_pos));
            sprites.Add(new Sprite(frame, new Point_short(400, 300)));
            //sprites.Add(new Sprite(Common["frame"], new Point_short(400, 300)));

            buttons[(int)Buttons.BT_LOGIN] = new MapleButton(UI_Title["BtLogin"], new Point_short(470, 237));
            //buttons[(int)Buttons.BT_SAVEID] = new MapleButton(UI_Title["BtLoginIDSave"], signboard_pos + new Point_short(-89, 5));
            //buttons[(int)Buttons.BT_IDLOST] = new MapleButton(Title["BtLoginIDLost"], signboard_pos + new Point_short(-17, 5));
            //buttons[(int)Buttons.BT_PASSLOST] = new MapleButton(Title["BtPasswdLost"], signboard_pos + new Point_short(55, 5));
            //buttons[(int)Buttons.BT_REGISTER] = new MapleButton(Title["BtNew"], signboard_pos + new Point_short(-101, 25));
            //buttons[(int)Buttons.BT_HOMEPAGE] = new MapleButton(Title["BtHomePage"], signboard_pos + new Point_short(-29, 25));
            buttons[(int)Buttons.BT_QUIT] = new MapleButton(UI_Title["BtQuit"], new Point_short(478, 350));

            /*checkbox[false] = UI_Title["check"]["0"];
            checkbox[true] = UI_Title["check"]["1"];*/

            background = new ColorBox(dimension.x(), dimension.y(), Color.Name.BLACK, 1.0f);

            Point_short textbox_pos = new Point_short(324, 240);
            Point_short textbox_dim = new Point_short(150, 24);
            short textbox_limit = 12;

            #region Account

            account = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.JAMBALAYA, Color.Name.SMALT, 0.75f, new Rectangle_short(new Point_short(textbox_pos), textbox_pos + textbox_dim), (uint)textbox_limit);

            account.set_key_callback(KeyAction.Id.TAB, () =>
           {
               account.set_state(Textfield.State.NORMAL);
               password.set_state(Textfield.State.FOCUSED);
           });

            account.set_enter_callback((string msg) => { login(); });

            //accountbg = UI_Title["ID"];

            #endregion

            //#region Password

            textbox_pos.shift_y(26);

            password = new Textfield(Text.Font.A13M, Text.Alignment.LEFT, Color.Name.JAMBALAYA, Color.Name.PRUSSIANBLUE, 0.85f, new Rectangle_short(new Point_short(textbox_pos), textbox_pos + textbox_dim), (uint)textbox_limit);

            password.set_key_callback(KeyAction.Id.TAB, () =>
           {
               account.set_state(Textfield.State.FOCUSED);
               password.set_state(Textfield.State.NORMAL);
           });

            password.set_enter_callback((string msg) => { login(); });

            password.set_cryptchar((sbyte)'*');
            //passwordbg = UI_Title["PW"];;
            /*  
                       saveid = Setting<SaveLogin>.get().load();
           
                    if (saveid)
                       {
                           account.change_text(Setting<DefaultAccount>.get().load());
                           password.set_state(Textfield.State.FOCUSED);
                       }
                       else
                       {
                           account.set_state(Textfield.State.FOCUSED);
                       }
           */
            account.change_text(Setting<DefaultAccount>.get().load());
            password.change_text(Setting<DefaultPassword>.get().load());
            password.set_state(Textfield.State.FOCUSED);
            
            if (Configuration.get().get_var_login())
            {
                UI.get().emplace<UILoginWait>();

                var loginwait = UI.get().get_element<UILoginWait>();

                if (loginwait && loginwait.get().is_active())
                {
                    new LoginPacket(Configuration.get().get_var_acc(), Configuration.get().get_var_pass()).dispatch();
                }
            }
        }

        public override void draw(float alpha)
        {
            background.draw(position + new Point_short(0, 7));

            base.draw(alpha);

            version.draw(position + new Point_short(707, 4));
            account.draw(position + new Point_short(1, 0));
            password.draw(position + new Point_short(1, 3));
            

            /*if (account.get_state() == Textfield.State.NORMAL && account.empty())
            {
                accountbg.draw(position + signboard_pos + new Point_short(-101, -51));
            }

            if (password.get_state() == Textfield.State.NORMAL && password.empty())
            {
                passwordbg.draw(position + signboard_pos + new Point_short(-101, -25));
            }*/

            //checkbox[saveid].draw(position + signboard_pos + new Point_short(-101, 7));
            //GraphicsGL.Instance.DrawWireRectangle(account.get_bounds().get_left_top().x(), account.get_bounds().get_left_top().y(), account.get_bounds().width(), account.get_bounds().height(), Microsoft.Xna.Framework.Color.Yellow, 1);
            //GraphicsGL.Instance.DrawWireRectangle(password.get_bounds().get_left_top().x(), password.get_bounds().get_left_top().y(), password.get_bounds().width(), password.get_bounds().height(), Microsoft.Xna.Framework.Color.Yellow, 1);
        }

        public override void update()
        {
            base.update();

            account.update(new Point_short(position));
            password.update(new Point_short(position));
        }

        public override Cursor.State send_cursor(bool clicked, Point_short cursorpos)
        {
            Cursor.State new_state = account.send_cursor(new Point_short(cursorpos), clicked);
            if (new_state != Cursor.State.IDLE)
            {
                return new_state;
            }

            Cursor.State new_state1 = password.send_cursor(new Point_short(cursorpos), clicked);
            if (new_state1 != Cursor.State.IDLE)
            {
                return new_state1;
            }

            return base.send_cursor(clicked, new Point_short(cursorpos));
        }

        public override UIElement.Type get_type()
        {
            return TYPE;
        }

        public override Button.State button_pressed(ushort id)
        {
            switch ((Buttons)id)
            {
                case Buttons.BT_LOGIN:
                    {
                        login();

                        return Button.State.NORMAL;
                    }
                case Buttons.BT_REGISTER:
                case Buttons.BT_HOMEPAGE:
                case Buttons.BT_PASSLOST:
                case Buttons.BT_IDLOST:
                    {
                        open_url(id);

                        return Button.State.NORMAL;
                    }
                case Buttons.BT_SAVEID:
                    {
                        saveid = !saveid;

                        Setting<SaveLogin>.get().save(saveid);

                        return Button.State.MOUSEOVER;
                    }
                case Buttons.BT_QUIT:
                    {
                        UI.get().quit();

                        return Button.State.PRESSED;
                    }
                default:
                    {
                        return Button.State.DISABLED;
                    }
            }
        }

        private void login()
        {
            OnRemove();

			
			string account_text = "";
			string password_text = "";

#if UNITY_EDITOR || WINDOWS
            if (GameUtil.Instance.TestMode)
            {
                account_text = GameUtil.Instance.TestAccount;
                password_text = GameUtil.Instance.TestPassword;
            }
            else
            {
                if (account.isForbid && password.isForbid)
                {
                    account_text = text_account;
                    password_text = text_password;
                }
                else
                {
                    account_text = account.get_text();
                    password_text = password.get_text();
                }
            }
#else
            if (account.isForbid && password.isForbid)
            {
                account_text = text_account;
                password_text = text_password;
            }
            else
            {
                account_text = account.get_text ();
                password_text = password.get_text ();
            }
#endif

       

            account.set_state(Textfield.State.DISABLED);
            password.set_state(Textfield.State.DISABLED);

            System.Action okhandler = () =>
            {
                account.set_state(Textfield.State.NORMAL);
                password.set_state(Textfield.State.NORMAL);

                if (!string.IsNullOrEmpty(password_text))
                {
                    password.set_state(Textfield.State.FOCUSED);
                }
                else
                {
                    account.set_state(Textfield.State.FOCUSED);
                }
                OnAdd();
            };

            if (string.IsNullOrEmpty(account_text))
            {
                UI.get().emplace<UILoginNotice>(UILoginNotice.Message.NOT_REGISTERED, okhandler, null);
                return;
            }

            if (password_text.Length <= 4)
            {
                UI.get().emplace<UILoginNotice>(UILoginNotice.Message.WRONG_PASSWORD, okhandler, null);
                return;
            }

            UI.get().emplace<UILoginWait>(okhandler);

            var loginwait = UI.get().get_element<UILoginWait>();

            if (loginwait && loginwait.get().is_active())
            {
                password_global = password_text;
                new LoginPacket(account_text, password_text).dispatch();
            }
        }

        //public static string account_global;
        public static string password_global;
        
        private void open_url(ushort id)
        {
            string url;

            switch ((Buttons)id)
            {
                case Buttons.BT_REGISTER:
                    url = Configuration.get().get_joinlink();
                    break;
                case Buttons.BT_HOMEPAGE:
                    url = Configuration.get().get_website();
                    break;
                case Buttons.BT_PASSLOST:
                    url = Configuration.get().get_findpass();
                    break;
                case Buttons.BT_IDLOST:
                    url = Configuration.get().get_findid();
                    break;
                default:
                    return;
            }

            //todo 2 ShellExecuteA(null, "open", url, null, null, SW_SHOWNORMAL);
        }

        public override void OnAdd()
        {
            var bound_account = account.get_bounds();
            var pos_account = /*position + new Point_short(1, 0) +*/ bound_account.get_left_top();
            //EditTextInfo editTextInfo_account = new EditTextInfo(pos_account.x(), pos_account.y() - bound_account.height(), bound_account.Width(), bound_account.height());
            EditTextInfo editTextInfo_account = new EditTextInfo(pos_account.x(), pos_account.y(), bound_account.width(), bound_account.height());
            MessageCenter.get().ShowAccount(this, editTextInfo_account);

            var bound_password = password.get_bounds();
            var pos_password = /*position + new Point_short(1, 3) +*/ bound_password.get_left_top();
            //EditTextInfo editTextInfo_password = new EditTextInfo(pos_password.x(), pos_password.y() - bound_password.height(), bound_password.Width(), bound_password.height());
            EditTextInfo editTextInfo_password = new EditTextInfo(pos_password.x(), pos_password.y(), bound_password.width(), bound_password.height());
            MessageCenter.get().ShowPassword(this, editTextInfo_password);
        }

        public override void OnRemove()
        {
            MessageCenter.get().HideAccount(this);
            MessageCenter.get().HidePassword(this);
        }

        public void HideTextfield_account_password()
        {
            account.isForbid = true;
            password.isForbid = true;
        }
        private enum Buttons
        {
            BT_LOGIN,
            BT_REGISTER,
            BT_HOMEPAGE,
            BT_PASSLOST,
            BT_IDLOST,
            BT_SAVEID,
            BT_QUIT,
            NUM_BUTTONS
        }

        private Text version;
        private Textfield account;
        private Textfield password;
        private Texture accountbg;
        private Texture passwordbg;
        private BoolPairNew<Texture> checkbox = new BoolPairNew<Texture>();
        private ColorBox background;
        private Point_short signboard_pos;

        private bool saveid;

        public string text_account;
        public string text_password;
    }
}