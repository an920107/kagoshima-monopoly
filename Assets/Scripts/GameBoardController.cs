using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject block;
    [SerializeField] private TextAsset blocksDataJsonFile;

    private readonly List<GameObject> blocks = new();
    private const int BLOCKS_PER_SIDE = 7;

    void Start()
    {
        for (int i = -4; i < 3; i++)
        {
            blocks.Add(Instantiate(block, new Vector3(-(i * 11 + 5.5f), 0.5f, -38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(-38.5f, 0.5f, i * 11 + 5.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(i * 11 + 5.5f, 0.5f, 38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(38.5f, 0.5f, -(i * 11 + 5.5f)), Quaternion.identity));
        }

        for (int i = 0; i < blocks.Count; i++)
            blocks[i].GetComponent<BlockController>().Number = (i % 4) * BLOCKS_PER_SIDE + i / 4;
        blocks.Sort((x, y) => x.GetComponent<BlockController>().Number.CompareTo(y.GetComponent<BlockController>().Number));
        for (int i = 0; i < blocks.Count; i++)
        {
            BlockController bc = blocks[i].GetComponent<BlockController>();
            switch (i / BLOCKS_PER_SIDE)
            {
                case 0:
                    bc.Location = (i % BLOCKS_PER_SIDE == 0) ? BlockLocation.RightDown : BlockLocation.Down;
                    break;
                case 1:
                    bc.Location = (i % BLOCKS_PER_SIDE == 0) ? BlockLocation.LeftDown : BlockLocation.Left;
                    break;
                case 2:
                    bc.Location = (i % BLOCKS_PER_SIDE == 0) ? BlockLocation.LeftUp : BlockLocation.Up;
                    break;
                case 3:
                    bc.Location = (i % BLOCKS_PER_SIDE == 0) ? BlockLocation.RightUp : BlockLocation.Right;
                    break;
            }
            bc.name += $"_{bc.Number,2:00}";
            bc.transform.parent = gameObject.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), -90f, 2f);

        if (Input.GetKeyDown(KeyCode.D))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), 90f, 2f);
    }
}
