using TMPro;
using UnityEngine;

public class CheckObjetive : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI objetiveCompleteUI;

    public void ShowObjetiveComplete()
    {
        if (objetiveCompleteUI != null)
        {
            objetiveCompleteUI.color = Color.green;
        }
    }
}
