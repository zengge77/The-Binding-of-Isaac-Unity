using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class NonBreakingSpaceTextComponent : MonoBehaviour
{
    /* 我们平时所使用的空格（即键盘Sapce键输出的空格），Unicode编码为/u0020，是换行空格(Breaking Space)，
     * 本意是用在英文的自动换行，但在中文里反而是个麻烦。
     * 我们使用 不换行空格(Non-breaking space)，Unicode编码为/u00A0 ，来替换换行空格
     * 将该脚本放在需要输出文字的UI下即可
    */

    private const string no_breaking_space = "\u00A0";

    private Text text;

    void Awake()
    {
        text = this.GetComponent<Text>();
        text.RegisterDirtyVerticesCallback(OnTextChange);
    }

    public void OnTextChange()
    {
        if (text.text.Contains(" "))
        {
            text.text = text.text.Replace(" ", no_breaking_space);
        }
    }

}
