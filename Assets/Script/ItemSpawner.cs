using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("References")]
    public Item itemPrefab;
    public Transform parent;
    public Transform player;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.2f;
    public float startZ = 8f;         
    public float speed = 0.08f;        

    private List<Item> activeItems = new List<Item>();
    private float[] laneX = new float[] { -2.5f, 0f, 2.5f }; 
    private Player playerComp;

    private void Start()
    {
        if (itemPrefab == null) Debug.LogError("ItemSpawner: itemPrefab missing!");
        if (player == null) Debug.LogError("ItemSpawner: Player reference missing!");

        playerComp = player.GetComponent<Player>();
        InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
    }

    void SpawnItem()
    {
        int lane = Random.Range(0, laneX.Length);

        Vector3 startPos = new Vector3(laneX[lane], -2.8f, startZ);

        Item newItem = Instantiate(itemPrefab, parent);
        newItem.itemPosition = startPos;

        newItem.transform.localScale = Vector3.one * 0.08f;

        activeItems.Add(newItem);
    }

    private void Update()
    {
        if (playerComp == null) return;

        for (int i = activeItems.Count - 1; i >= 0; i--)
        {
            Item item = activeItems[i];

            item.itemPosition.z -= speed;

            float laneXPos = (playerComp.LaneIndex - 1) * playerComp.laneDistance;
            if (Mathf.Abs(item.itemPosition.z - player.position.z) < 0.1f &&
                Mathf.Abs(item.itemPosition.x - laneXPos) < 0.5f &&
                playerComp.YOffset < 0.1f)
            {
                playerComp.TakeDamage(5);
                Destroy(item.gameObject);
                activeItems.RemoveAt(i);
                continue;
            }

            if (item.itemPosition.z < player.position.z - 0.1f)
            {
                Destroy(item.gameObject);
                activeItems.RemoveAt(i);
            }
        }
        var sorted = activeItems.OrderBy(it => it.itemPosition.z).ToList();
        for (int i = 0; i < sorted.Count; i++)
            sorted[i].transform.SetSiblingIndex(i);
    }
}
