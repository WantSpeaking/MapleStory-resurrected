using UnityEngine;
using UnityEngine.UI;

//自动生成于：6/23/2022 8:18:20 PM
namespace ms
{

	public partial class Test
	{

		private RectTransform m_Transform_A;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Transform_A = autoBindTool.GetBindComponent<RectTransform>(0);
		}
	}
}
