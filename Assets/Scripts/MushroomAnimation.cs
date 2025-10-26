using UnityEngine;
using System.Collections;

public class MushroomAnimation : MonoBehaviour
{
    public GameObject otherObject;
    public float waitTime = 3f;
    public float firstObjectTime = 0.3f; // Время для первого объекта
    public float secondObjectTime = 0.35f; // Время для второго объекта

    void Start()
    {
        StartCoroutine(AnimationLoop());
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            // Запускаем первый объект
            GetComponent<Animator>().SetBool("TimeGas", true);
            StartCoroutine(DisableFirstObject());

            // Запускаем второй объект через 0.30 секунды
            yield return new WaitForSeconds(0.30f);
            otherObject.GetComponent<Animator>().SetBool("TimeGas", true);
            StartCoroutine(DisableSecondObject());
        }
    }

    IEnumerator DisableFirstObject()
    {
        yield return new WaitForSeconds(firstObjectTime);
        GetComponent<Animator>().SetBool("TimeGas", false);
    }

    IEnumerator DisableSecondObject()
    {
        yield return new WaitForSeconds(secondObjectTime);
        otherObject.GetComponent<Animator>().SetBool("TimeGas", false);
    }
}
