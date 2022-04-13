using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject rankingBoard;
    [SerializeField] private Transform[] allCharacters;
    [SerializeField] private TextMeshProUGUI[] rankingTexts;

    private LevelRanking levelRanking;

    protected internal string PlayerName { get; private set; }

    private void Awake()
    {
        PlayerName = "Player";
        levelRanking = new LevelRanking(allCharacters, PlayerName);
    }

    private void OnEnable()
    {
        GameManager.FinisLineAction += SetRankingBoard;
    }

    private void OnDisable()
    {
        GameManager.FinisLineAction -= SetRankingBoard;
    }

    private void Start() => StartCoroutine(SetRankingTexts());

    private IEnumerator SetRankingTexts()
    {
        yield return new WaitUntil(() => rankingBoard.activeInHierarchy);

        var rankingCharacter = levelRanking.CharacterRanking;

        for (var i = 0; i < 4; i++)
        {
            var (characterName, rank) = rankingCharacter[i];

            rankingTexts[i].text = $"{rank}. {characterName}";
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SetRankingTexts());
    }

    private void SetRankingBoard()
    {
        rankingBoard.SetActive(!rankingBoard.activeInHierarchy);
    }
}