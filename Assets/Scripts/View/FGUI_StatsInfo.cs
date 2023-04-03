/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
    public partial class FGUI_StatsInfo
    {

        void OnCreate()
        {
            _Label_Dex._Btn_Add.onClick.Add(() => OnClick(true, MapleStat.Id.DEX));
            _Label_Int._Btn_Add.onClick.Add(() => OnClick(true, MapleStat.Id.INT));
            _Label_Luck._Btn_Add.onClick.Add(() => OnClick(true, MapleStat.Id.LUK));
            _Label_Str._Btn_Add.onClick.Add(() => OnClick(true, MapleStat.Id.STR));
        }

        private void OnClick(bool add, MapleStat.Id id)
        {
            Refresh();
            new SpendApPacket(id).dispatch();
        }

        private CharStats _CharStats;

        public CharStats CharStats => _CharStats ??= ms.Stage.get()?.get_player()?.get_stats();

        public void Refresh()
        {
            var AP = CharStats?.get_stat(MapleStat.Id.AP) ?? 0;
            var DEX = CharStats?.get_stat(MapleStat.Id.DEX) ?? 0;
            var INT = CharStats?.get_stat(MapleStat.Id.INT) ?? 0;
            var LUK = CharStats?.get_stat(MapleStat.Id.LUK) ?? 0;
            var STR = CharStats?.get_stat(MapleStat.Id.STR) ?? 0;

            bool hasAp = AP > 0;

            _Txt_RemainAP.SetVar("count", AP.ToString()).FlushVars();

            _Label_Dex._Btn_Add.enabled = hasAp;
            _Label_Dex._Btn_Add.enabled = hasAp;
            _Label_Int._Btn_Add.enabled = hasAp;
            _Label_Luck._Btn_Add.enabled = hasAp;
            _Label_Str._Btn_Add.enabled = hasAp;

            _Label_Dex.GetTextField().SetVar("count", DEX.ToString()).FlushVars();
            _Label_Int.GetTextField().SetVar("count", INT.ToString()).FlushVars();
            _Label_Luck.GetTextField().SetVar("count", LUK.ToString()).FlushVars();
            _Label_Str.GetTextField().SetVar("count", STR.ToString()).FlushVars();

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Refresh();
        }
    }
}