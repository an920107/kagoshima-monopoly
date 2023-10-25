using OverlayEvents;
using System.Collections.Generic;
using System.Text.Json;
using UnityEngine;

public class GameBoardController : MonoBehaviour {

    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject turnDisplay;
    [SerializeField] private GameObject block;
    [SerializeField] private GameObject dice;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject flag;
    [SerializeField] private TextAsset blocksDataJson;

    public event DiceHasResultEventHandler DiceHasResult;

    private const int BLOCKS_COUNT = 28;
    private readonly List<GameObject> blocks = new();
    private readonly List<GameObject> dices = new();
    private readonly List<GameObject> players = new();
    private readonly BlockData[] blocksData = new BlockData[BLOCKS_COUNT];
    private bool toGetDiceResult = true;
    private bool diceIsThrowable = false;
    private int turn = -1;
    private OverlayController overlayController;

    void Awake() {
        DiceHasResult += GameBoardController_DiceHasResult;
    }

    void Start() {
        overlayController = overlay.GetComponent<OverlayController>();

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
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.A))
            RotateClockwise();

        if (Input.GetKeyDown(KeyCode.D))
            RotateCounterClockwise();

        if (Input.GetKeyDown(KeyCode.Space))
            ThrowDice();

        if (Input.GetKeyDown(KeyCode.Tab))
            ShowStatus();

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
        DiceHasResult?.Invoke(this, new(sum));
    }

    private void GameBoardController_DiceHasResult(object sender, DiceHasResultEventArgs e) {
        var pc = players[turn].GetComponent<PlayerController>();
        Queue<Vector3> dest = new();
        pc.StepLeft = 0;
        for (int i = 0; i < e.Points; i++) {
            pc.AtBlock = (++pc.AtBlock) % BLOCKS_COUNT;
            players[turn].transform.SetParent(blocks[pc.AtBlock].transform);
            dest.Enqueue(GetPlayerMovedPosition(pc.AtBlock));
            if (pc.AtBlock == 0) {
                pc.StepLeft = e.Points - i - 1;
                break;
            }
        }
        pc.Jump(dest);
    }

    private void GameBoardController_PlayerArrived(object sender, PlayerArrivedEventArgs e) {
        var bc = blocks[(sender as PlayerController).AtBlock].GetComponent<BlockController>();
        if (bc.Owner != -1) {
            overlayController.ShowPayMoney(
                bc, sender as PlayerController,
                players[bc.Owner].GetComponent<PlayerController>(),
                out PayMoneyController controller
            );
            controller.PayMoneyComplete += GameBoardController_PayMoneyComplete;
        } else if (bc.Data.Type == BlockType.Land) {
            overlayController.ShowBuyLand(
                bc, sender as PlayerController, out BuyLandController controller);
            controller.BuyLandConplete += GameBoardController_BuyLandComplete;
        } else if (bc.Data.Type == BlockType.Facility) {
            overlayController.ShowBuyFacility(
                bc, sender as PlayerController, out BuyFacilityController controller);
            controller.BuyFacilityComplete += GameBoardController_BuyFacilityComplete;
        } else if (bc.Data.Type == BlockType.Start) {
            overlayController.ShowSalary(
                bc, sender as PlayerController, out SalaryController controller);
            controller.SalaryComplete += Controller_SalaryComplete;
        } else {
            NextTurn();
        }
    }

    private void GameBoardController_PayMoneyComplete(object sender, PayMoneyCompleteEventArgs e) {
        var toll = (e.Block.Data.Type == BlockType.Land)
            ? e.Block.Data.Tolls[e.To.Lands[e.Block.Number]]
            : e.Block.Data.Tolls[e.To.Facilities.Count];
        e.From.Money -= toll;
        e.To.Money += toll;
        overlay.SetActive(false);
        NextTurn();
    }

    private void GameBoardController_BuyLandComplete(object sender, BuyLandCompleteEventArgs e) {
        if (e.ToBuy == true) {
            if (e.Player.Money >= e.Block.Data.Price) {
                e.Player.Money -= e.Block.Data.Price;
                e.Player.Lands.Add(e.Player.AtBlock, 0);
                blocks[e.Player.AtBlock].GetComponent<BlockController>().Owner = e.Player.Number;
                var flagPrefab = Instantiate(flag, blocks[e.Player.AtBlock].transform, true);
                flagPrefab.GetComponent<FlagController>().SurfaceColor = e.Player.SkinColor;
                overlay.SetActive(false);
                NextTurn();
            } else {
                overlayController.ShowNoMoney(out NoMoneyController controller);
                controller.NoMoneyComplete += GameBoardController_NoMoneyComplete;
            }
        } else {
            overlay.SetActive(false);
            NextTurn();
        }
    }

    private void GameBoardController_BuyFacilityComplete(object sender, BuyFacilityCompleteEventArgs e) {
        if (e.ToBuy == true) {
            if (e.Player.Money >= e.Block.Data.Price) {
                e.Player.Money -= e.Block.Data.Price;
                e.Player.Facilities.Add(e.Player.AtBlock);
                blocks[e.Player.AtBlock].GetComponent<BlockController>().Owner = e.Player.Number;
                var flagPrefab = Instantiate(flag, blocks[e.Player.AtBlock].transform, true);
                flagPrefab.GetComponent<FlagController>().SurfaceColor = e.Player.SkinColor;
                overlay.SetActive(false);
                NextTurn();
            } else {
                overlayController.ShowNoMoney(out NoMoneyController controller);
                controller.NoMoneyComplete += GameBoardController_NoMoneyComplete;
            }
        } else {
            overlay.SetActive(false);
            NextTurn();
        }
    }

    private void Controller_SalaryComplete(object sender, SalaryCompleteEventArgs e) {
        e.Player.Money += e.StartBlock.Data.Price;
        overlay.SetActive(false);
        if (e.Player.StepLeft == 0)
            NextTurn();
        else DiceHasResult.Invoke(this, new(e.Player.StepLeft));
    }

    private void GameBoardController_NoMoneyComplete(object sender, NoMoneyCompleteEventArgs e) {
        overlay.SetActive(false);
        NextTurn();
    }

    private void GameBoardController_StatusComplete(object sender, StatusCompleteEventArgs e) {
        overlay.SetActive(false);
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

    public void ShowStatus() {
        overlayController.ShowStatus(
            blocks.ConvertAll<BlockController>(new(x => x.GetComponent<BlockController>())),
            players.ConvertAll<PlayerController>(new(x => x.GetComponent<PlayerController>())),
            out StatusController controller);
        controller.StatusComplete += GameBoardController_StatusComplete;
    }

    public void RotateClockwise() {
        GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), -90f, 2f);
    }

    public void RotateCounterClockwise() {
        GetComponent<RotationController>().Rotate(new Vector3(0f, 1f, 0f), 90f, 2f);
    }

    public void GeneratePlayerAndStart(List<Color> playersColor) {
        for (int i = 0; i < playersColor.Count; i++) {
            var tmpPlayer = Instantiate(player);
            players.Add(tmpPlayer);
            tmpPlayer.transform.SetParent(blocks[0].transform);
            tmpPlayer.transform.position = GetPlayerMovedPosition(0);
            tmpPlayer.name = $"Player_{i}";
            var pc = tmpPlayer.GetComponent<PlayerController>();
            pc.Number = i;
            pc.SkinColor = playersColor[i];
            pc.PlayerArrived += GameBoardController_PlayerArrived;
        }
        NextTurn();
    }

    public void NextTurn() {
        if (++turn == players.Count)
            turn = 0;

        var pc = players[turn].GetComponent<PlayerController>();
        turnDisplay.GetComponent<Renderer>().material.color =
            pc.SkinColor;

        if (pc.IsPause) {
            pc.IsPause = false;
            NextTurn();
        }
        diceIsThrowable = true;
    }
}
