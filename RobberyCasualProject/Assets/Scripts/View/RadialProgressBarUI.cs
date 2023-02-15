using UnityEngine.UI;
using UnityEngine;

public class RadialProgressBarUI : MonoBehaviour
{
    [SerializeField]
    private Image _imageBar;

    private void OnEnable() => Progress = 0f;

    public float Progress
    {
        set
        {
            _imageBar.fillAmount = value;
        }
    }
}
