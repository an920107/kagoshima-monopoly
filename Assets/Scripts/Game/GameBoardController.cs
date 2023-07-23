using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

public class GameBoardController : MonoBehaviour {
    [SerializeField] private GameObject block;
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject player;
    [SerializeField] private TextAsset blocksDataJson;

    public event DiceHasResultEventHandler DiceHasResult;

    private const int BLOCKS_COUNT = 28;
    private readonly List<GameObject> blocks = new();
    private readonly List<GameObject> dices = new();
    private readonly List<GameObject> players = new();
    private readonly BlockData[] blocksData = new BlockData[BLOCKS_COUNT];
    private bool toGetDiceResult = true;
    private bool diceIsThrowable = false;
    private int turn = 0;

    void Awake() {
        DiceHasResult += This_DiceHasResult;
    }

    void Start() {
        for (int i = -4; i < 3; i++) {
            blocks.Add(Instantiate(block, new Vector3(-(i * 11 + 5.5f), 0.5f, -38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(-38.5f, 0.5f, i * 11 + 5.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(i * 11 + 5.5f, 0.5f, 38.5f), Quaternion.identity));
            blocks.Add(Instantiate(block, new Vector3(38.5f, 0.5f, -(i * 11 + 5.5f)), Quaternion.identity));
        }

        for (int i = 0; i < blocks.Count; i++)
            blocks[i].GetComponent<BlockController>().Number = (i % 4) * (BLOCKS_COUNT / 4) + i / 4;
        blocks.Sort((x, y) => x.GetComponent<BlockController>().Number.CompareTo(y.GetComponent<BlockController>().Number));
        foreach (var blockData in JsonSerializer.Deserialize<Dictionary<string, BlockData>>(blocksDataJson.text))
            blocksData[int.Parse(blockData.Key)] = blockData.Value;
        for (int i = 0; i < blocks.Count; i++) {
            BlockController bc = blocks[i].GetComponent<BlockController>();
            bc.Data = blocksData[i];
            bc.name = $"Block_{bc.Number,2:00}";
            bc.transform.SetParent(gameObject.transform);
        }

        for (int i = 0; i < 2; i++) {
            dices.Add(Instantiate(dice));
            dices[i].transform.SetParent(gameObject.transform);
            dices[i].name = $"Dice_{i}";
        }

        diceIsThrowable = true;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A))
            RotateClockwise();

        if (Input.GetKeyDown(KeyCode.D))
            RotateCounterClockwise();

        if (Input.GetKeyDown(KeyCode.Space))
            ThrowDice();

        if (!toGetDiceResult)
            GetDiceResult();
    }

    private void GetDiceResult() {
        int sum = 0;
        foreach (var dice in dices) {
            var dc = dice.GetComponent<DiceController>();
            if (!dc.IsStill)
                return;
            sum += dc.Value;
        }
        toGetDiceResult = true;
        DiceHasResult?.Invoke(this, new DiceHasResultEventArgs(sum));
    }

    private void This_DiceHasResult(object sender, DiceHasResultEventArgs e) {
        diceIsThrowable = true;
        var pc = players[turn].GetComponent<PlayerController>();
        Queue<Vector3> dest = new();
        for (int i = 0; i < e.Points; i++) {
            pc.AtBlock = (++pc.AtBlock) % BLOCKS_COUNT;
            players[turn].transform.SetParent(blocks[pc.AtBlock].transform);
            dest.Enqueue(GetPlayerMovedPosition(pc.AtBlock));
        }
        pc.Jump(dest);
        if (++turn == players.Count)
            turn = 0;
    }

    private Vector3 GetPlayerMovedPosition(int blockIndex) {
        int playersCountOnBlock = -1;
        foreach (var player in players)
            if (player.GetComponent<PlayerController>().AtBlock == blockIndex)
                playersCountOnBlock++;
        return blocks[blockIndex].transform.TransformPoint(
            new Vector3(-0.33f + 0.22f * playersCountOnBlock, 0.58f, 0.33f));
    }

    public void ThrowDice() {
        if (!diceIsThrowable)
            return;
        toGetDiceResult = false;
        diceIsThrowable = false;
        foreach (var dice in dices)
            dice.GetComponent<DiceController>().Throw();
    }

    public void RotateClockwise() {
        GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), -90f, 2f);
    }

    public void RotateCounterClockwise() {
        GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), 90f, 2f);
    }

    public void GeneratePlayerAndStart(List<Color> playersColor) {
        diceIsThrowable = true;
        for (int i = 0; i < playersColor.Count; i++) {
            var tmpPlayer = Instantiate(player);
            players.Add(tmpPlayer);
            tmpPlayer.transform.SetParent(blocks[0].transform);
            tmpPlayer.transform.position = GetPlayerMovedPosition(0);
            tmpPlayer.name = $"Player_{i}";
            tmpPlayer.GetComponent<PlayerController>().SkinColor = playersColor[i];
        }
    }
}
