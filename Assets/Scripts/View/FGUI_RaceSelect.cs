using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
    public partial class FGUI_RaceSelect
    {
        private UIRaceSelect _uiRaceSelect;
        public void OnCreate()
        {
            _GList_Job.onClickItem.Add (OnClickItem);
            _Btn_Back.onClick.Add (OnClick_Btn_Back);
            _Btn_Choose.onClick.Add (OnClick_Btn_Choose);
        }

        private void OnClick_Btn_Choose (EventContext context)
        {
            _uiRaceSelect.deactivate();
            UIExplorerCreation.SelectedJobId = index_to_jobId (_GList_Job.selectedIndex);
            ms.UI.Instance.emplace<UIExplorerCreation> ();
        }

        private void OnClick_Btn_Back (EventContext context)
        {
            _uiRaceSelect.show_charselect ();
        }

        private void OnClickItem (EventContext context)
        {
            var jobId = index_to_jobId (_GList_Job.selectedIndex);
            var jobName = Job.get_name ((ushort)jobId);
            _Txt_JobName.text = jobName;
            

        }

        private int index_to_jobId (int index)
        {
            int number = index + 1;
            int jobId = 112;
            switch (number)
            {
                case 1:
                    jobId = 112;
                    break;
                case 2:
                    jobId = 122;
                    break;
                case 3:
                    jobId = 132;
                    break;
                case 4:
                    jobId = 212;
                    break;
                case 5:
                    jobId = 222;
                    break;
                case 6:
                    jobId = 232;
                    break;
                case 7:
                    jobId = 312;
                    break;
                case 8:
                    jobId = 322;
                    break;
                case 9:
                    jobId = 412;
                    break;
                case 10:
                    jobId = 422;
                    break;
            }
            return jobId;
        }

        public void OnActivityChange (UIRaceSelect uiRaceSelect)
        {
            _uiRaceSelect = uiRaceSelect;
            
        }
    }
}