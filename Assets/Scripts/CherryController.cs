using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    private GameObject cherry = null;

    private float timer = 0.0f;
    private float spawnInterval = 10.0f;

    private Vector3[] spawnPositions;
    private Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        InitializeSpawnPositions();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnInterval)
        {
            SpawnCherry();
            timer = 0;
        }
    }

    private void InitializeSpawnPositions()
    {
        float offset = 70.0f;
        spawnPositions = new Vector3[]
        {
            new Vector3(Random.Range(-offset, offset), offset, 0), //top 
            new Vector3(Random.Range(-offset, offset), -offset, 0), //bottom
            new Vector3(offset, Random.Range(-offset, offset), 0), //right
            new Vector3(-offset, Random.Range(-offset, offset), 0) // left


        };

    }

    private void SpawnCherry()
    {
        Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Length)];
        cherry = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
        Vector3 centerPosition = Vector3.zero;
        Vector3 oppositePosition = GetOppositePosition(spawnPosition);
        if(tweener != null)
        {
            StartCoroutine(HandleCherryMovement(spawnPosition, centerPosition, oppositePosition));
        }
    }

    private IEnumerator HandleCherryMovement(Vector3 start, Vector3 center, Vector3 end)
    {
        tweener.AddTween(cherry.transform, start, center, 3f);
        yield return new WaitForSeconds(3f);

        tweener.AddTween(cherry.transform, center, end, 3f);
        yield return new WaitForSeconds(3f);

        Destroy(cherry);
    }

    private Vector3 GetOppositePosition(Vector3 spawnPosition)
    {

        return new Vector3(-spawnPosition.x, -spawnPosition.y, spawnPosition.z);
    }


}
