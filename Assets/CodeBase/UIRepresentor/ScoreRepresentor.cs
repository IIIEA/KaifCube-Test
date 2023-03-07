using CodeBase.Spawners;
using TMPro;
using UnityEngine;

namespace CodeBase.UIRepresentor
{
  public class ScoreRepresentor : MonoBehaviour
  {
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private CubeSpawner _cubeSpawner;

    private int _score = 0;

    private void Awake() => 
      UpdateScoreText(_score);

    private void OnEnable() =>
      _cubeSpawner.CubeDestroyed += OnCubeDestored;

    private void OnDisable() => 
      _cubeSpawner.CubeDestroyed -= OnCubeDestored;

    private void OnCubeDestored()
    {
      _score++;
      UpdateScoreText(_score);
    }

    private void UpdateScoreText(int score) => 
      _scoreText.text = score.ToString();
  }
}