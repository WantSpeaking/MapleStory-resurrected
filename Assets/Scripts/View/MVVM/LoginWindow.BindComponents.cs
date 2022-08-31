using UnityEngine;
using UnityEngine.UI;
using TMPro;

//自动生成于：6/28/2022 2:53:29 PM
namespace ms_Unity
{

	public partial class LoginWindow
	{

		private Button m_Button_Close;
		private Button m_Text_Signup;
		private Button m_Text_ForgotPassword;
		private TMP_InputField m_InputField_UserName;
		private TMP_InputField m_InputField_Password;
		private Button m_Button_Login;

		public Button Button_Close
		{
			get
			{
				if (m_Button_Close == null)
				{
					GetBindComponents (gameObject);
				}
				return m_Button_Close;
			}
		}
		public Button Text_Signup
		{
			get
			{
				if (m_Text_Signup == null)
				{
					GetBindComponents (gameObject);
				}
				return m_Text_Signup;
			}
		}
		public Button Text_ForgotPassword
		{
			get
			{
				if (m_Text_ForgotPassword == null)
				{
					GetBindComponents (gameObject);
				}
				return m_Text_ForgotPassword;
			}
		}
		public TMP_InputField InputField_UserName
		{
			get
			{
				if (m_InputField_UserName == null)
				{
					GetBindComponents (gameObject);
				}
				return m_InputField_UserName;
			}
		}
		public TMP_InputField InputField_Password
		{
			get
			{
				if (m_InputField_Password == null)
				{
					GetBindComponents (gameObject);
				}
				return m_InputField_Password;
			}
		}
		public Button Button_Login
		{
			get
			{
				if (m_Button_Login == null)
				{
					GetBindComponents (gameObject);
				}
				return m_Button_Login;
			}
		}

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Button_Close = autoBindTool.GetBindComponent<Button>(0);
			m_Text_Signup = autoBindTool.GetBindComponent<Button>(1);
			m_Text_ForgotPassword = autoBindTool.GetBindComponent<Button>(2);
			m_InputField_UserName = autoBindTool.GetBindComponent<TMP_InputField>(3);
			m_InputField_Password = autoBindTool.GetBindComponent<TMP_InputField>(4);
			m_Button_Login = autoBindTool.GetBindComponent<Button>(5);
		}
	}
}
