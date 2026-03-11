using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;

    public int SkinSelect;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
