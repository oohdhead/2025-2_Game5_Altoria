using Common;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Recorder.OutputPath;

namespace GameUI
{
    public class UI_LobbyButtons : UIWidget
    {
        [SerializeField] Button startBtn;
        [SerializeField] Button settingBtn;
        [SerializeField] Button eixtBtn;

        public override bool Init()
        {
            if (!base.Init()) return false;

            return true;
        }

        #region OnClick Event
        public void OnClickStartButton()
        {
            GameSystem.Init();
            Manager.UI.ShowHUD<CustomizingMenu>();
        }        

        public void OnClickSettingButton()
        {
            Manager.UI.ShowPopup<SettingPopUp>();
        }
        public void OnClickExitButton()
        {
            Manager.UserData.SaveAllUserData();
            Manager.UI.ShowPopup<ExitPopUp>();
        }
        #endregion
    }
}