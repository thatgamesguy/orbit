using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Orbit
{
    /// <summary>
    /// Responsible for updating, storing and displaying current and highscore.
    /// </summary>
    public class ScoreController : Singleton<ScoreController>
    {
        /// <summary>
        /// The combo text.
        /// </summary>
        public Text comboText;

        /// <summary>
        /// The score text.
        /// </summary>
        public Text scoreText;

        /// <summary>
        /// The highscore text.
        /// </summary>
        public Text highscoreText;

        /// <summary>
        /// The current score.
        /// </summary>
        public int score = 0;

        /// <summary>
        /// The current combo.
        /// </summary>
        public int combo;

        /// <summary>
        /// The curret highscore.
        /// </summary>
        public int highscore { get; private set; }

        private float currentScore = 0f;
        private float scoreInc = -0;

        private readonly string ScoreName = "high_score";

        void Start()
        {
            highscore = PlayerPrefs.GetInt(ScoreName, 0);

            highscoreText.text = highscore.ToString("D6");

            combo = 1;
        }

        /// <summary>
        /// Resets current score text.
        /// </summary>
        public void OnGameStart()
        {
            scoreText.text = "000000";

            UpdateComboText("");
        }

        /// <summary>
        /// Checks if current score is greater than the highscore, if so it is stored.
        /// </summary>
        public void OnGameOver()
        {
            if (score > highscore)
            {
                highscore = score;
                PlayerPrefs.SetInt(ScoreName, highscore);
            }
        }

        /// <summary>
        /// Updates current score.
        /// </summary>
        /// <param name="val">The value to uodate the score.</param>
        public void UpdateScore(int val)
        {
            score += val * combo;

            if (combo > 1)
            {
                UpdateComboText("chain " + combo.ToString() + "x");
            }

            combo++;

        }

        /// <summary>
        /// Resets the scores combo.
        /// </summary>
        public void ResetCombo()
        {
            combo = 1;
            UpdateComboText("");
        }

        private void UpdateComboText(object text)
        {
            if (comboText)
            {
                comboText.text = text.ToString();
            }
        }

        void Update()
        {

            if (GameManager.Instance.gameState == GameState.Play)
            {
                if (currentScore < score)
                {
                    scoreInc += 20 * Time.deltaTime;
                    currentScore += scoreInc;
                    if (currentScore > score)
                    {
                        currentScore = score;
                        scoreInc = 0f;
                    }
                    scoreText.text = Mathf.FloorToInt(currentScore).ToString("D6");
                }
            }
        }
    }
}