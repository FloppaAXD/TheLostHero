using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkinSelect : MonoBehaviour
{
    [Header("Список кнопок")]
    [SerializeField] private List<Button> buttons;

    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => SelectSkin(index));
        }
    }

    private void SelectSkin(int index)
    {
        PlayerPrefs.SetInt("SelectedSkin", index);
        PlayerPrefs.Save();

        Debug.Log("Выбран скин: " + index);
    }
}
