using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject block;

    private List<GameObject> blocks = new();

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
            blocks[i].GetComponent<BlockController>().number = (i % 4) * 7 + i / 4;
        blocks.Sort((x, y) => x.GetComponent<BlockController>().number.CompareTo(y.GetComponent<BlockController>().number));
        for (int i = 0; i < blocks.Count; i++)
        {
            BlockController bc = blocks[i].GetComponent<BlockController>();
            bc.name += $"_{bc.number,2:00}";
            bc.transform.parent = gameObject.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), 90f, 2f);
        if (Input.GetKeyDown(KeyCode.D))
            GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), -90f, 2f);
    }
}
