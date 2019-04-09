using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDropSpawner : MonoBehaviour
{ 
    public GameObject dropBlockPrefab;

    // When game object is enabled
    void OnEnable()
    {
        VoxelChunk.OnEventBlockDestroyed += SpawnDropBlock;
    }
    // When game object is disabled
    void OnDisable()
    {
        VoxelChunk.OnEventBlockDestroyed -= SpawnDropBlock;
    }

    void SpawnDropBlock(int blockType, Vector3 blockPosition)
    {
        GameObject newDroppedBlock = Instantiate(dropBlockPrefab, this.transform);
        newDroppedBlock.transform.position = new Vector3(blockPosition.x + 0.5f, blockPosition.y + 0.5f, blockPosition.z + 0.5f);

        newDroppedBlock.GetComponent<VoxelBlock>().CreateBlock(blockType);
    }
}
