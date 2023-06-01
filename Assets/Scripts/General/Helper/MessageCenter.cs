using ms_Unity;
using System;
using System.Collections.Generic;

namespace ms.Util
{
    public class MessageCenter : Singleton<MessageCenter>
    {
        public Action<List<MaplePartyCharacter>, int> PartyDataChanged;


       
        public MessageCenter()
        {
            //PartyDataChanged += UIUserList.OnPartyDataChanged;
            //PartyDataChanged += UIPartyMember_HP.OnPartyDataChanged;

            PartyDataChanged += FGUI_Manager.Instance.GetFGUI<FGUI_StatusBar>()._QuestLogMini.OnPartyDataChanged;
        }
        ~MessageCenter()
        {
            //PartyDataChanged -= UIUserList.OnPartyDataChanged;
            //PartyDataChanged -= UIPartyMember_HP.OnPartyDataChanged;

            PartyDataChanged -= FGUI_Manager.Instance.GetFGUI<FGUI_StatusBar>()._QuestLogMini.OnPartyDataChanged;
        }

        #region Account Password
        public event EventHandler<EditTextInfo> UILogin_Show_Account;
        public event EventHandler<EditTextInfo> UILogin_Show_Password;
        public event EventHandler UILogin_Hide_Account;
        public event EventHandler UILogin_Hide_Password;
        public void ShowAccount(object sender, EditTextInfo info)
        {
            UILogin_Show_Account?.Invoke(sender, info);
        }

        public void HideAccount(object sender)
        {
            UILogin_Hide_Account?.Invoke(sender, EventArgs.Empty);
        }

        public void ShowPassword(object sender, EditTextInfo info)
        {
            UILogin_Show_Password?.Invoke(sender, info);
        }

        public void HidePassword(object sender)
        {
            UILogin_Hide_Password?.Invoke(sender, EventArgs.Empty);
        }
        #endregion

        #region Game
        public event EventHandler GameConstruct;
        public event EventHandler GameInitialize;
        public event EventHandler GameLoadContent;
        public event EventHandler GameUpdate;
        public event EventHandler GameDraw;

        public void ConstructGame(object sender)
        {
            GameConstruct?.Invoke(sender, EventArgs.Empty);
        }
        public void InitializeGame(object sender)
        {
            GameInitialize?.Invoke(sender, EventArgs.Empty);
        }
        public void LoadContentGame(object sender)
        {
            GameLoadContent?.Invoke(sender, EventArgs.Empty);
        }
        public void UpdateGame(object sender)
        {
            GameUpdate?.Invoke(sender, EventArgs.Empty);
        }
        public void DrawGame(object sender)
        {
            GameDraw?.Invoke(sender, EventArgs.Empty);
        }
        #endregion

        #region ExplorerCreation chaName
        public event EventHandler<EditTextInfo> UIExplorerCreation_Show_CharName;
        public event EventHandler UIExplorerCreation_Hide_CharName;

        public void ShowCharName(object sender, EditTextInfo info)
        {
            UIExplorerCreation_Show_CharName?.Invoke(sender, info);
        }

        public void HideCharName(object sender)
        {
            UIExplorerCreation_Hide_CharName?.Invoke(sender, EventArgs.Empty);
        }
        #endregion

        #region Chat
        public event EventHandler<EditTextInfo> UIChat_Show;
        public event EventHandler UIChat_Hide;

        public void ShowChat(object sender, EditTextInfo info)
        {
            UIChat_Show?.Invoke(sender, info);
        }

        public void HideChat(object sender)
        {
            UIChat_Hide?.Invoke(sender, EventArgs.Empty);
        }
        #endregion
    }

    public struct EditTextInfo
    {
        public int posX;
        public int posY;
        public int width;
        public int height;

        public EditTextInfo(int posX, int posY, int width, int height)
        {
            this.posX = posX;
            this.posY = posY;
            this.width = width;
            this.height = height;
        }
    }
}