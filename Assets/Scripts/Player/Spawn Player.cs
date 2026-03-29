using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject spawnPlayer;

    void Start()
    {
        float X = spawnPlayer.transform.position.x;
        float Y = spawnPlayer.transform.position.y;
        Vector3 position = new Vector3(X, Y, 0);

        GameObject newPlayer = Instantiate(player, position, Quaternion.identity);
    }

}
