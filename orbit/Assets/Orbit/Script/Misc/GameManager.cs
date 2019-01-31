using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Orbit
{
	/// <summary>
	/// Game states.
	/// </summary>
	public enum GameState
	{
		Play,
		Pause,
		GameOver,
		NotStarted
	}

	/// <summary>
	/// Manages game state.
	/// </summary>
	public class GameManager : Singleton<GameManager>
	{
		/// <summary>
		/// helpful shape spawner. Used to spawn shapes at round start.
		/// </summary>
		public HelpfulShapeSpawner helpfulShapeSpawner;

		/// <summary>
		/// Ship spawner. Used to spawn additional player ships.
		/// </summary>
		public ShipSpawner shipSpawner;

		/// <summary>
		/// Enemy spawner. Used to spawn enemies during round.
		/// </summary>
		public EnemySpawner enemySpawner;

		/// <summary>
		/// Shop controller.
		/// </summary>
		public ShopController shopController;

		/// <summary>
		/// Wall spawner. Used to spawn walls at round start.
		/// </summary>
		public WallSpawner wallSpawner;

		/// <summary>
		/// Energy manager.
		/// </summary>
		public EnergyManager energyManager;

		/// <summary>
		/// Orbit damage animator. Activates animation on orbit damage.
		/// </summary>
		public OrbitDamageAnimation orbitDamageAnimation;

		/// <summary>
		/// Particle damage prefab.
		/// </summary>
		public GameObject particleDamage;

		/// <summary>
		/// Player controller. Updated during round.
		/// </summary>  
		public PlayerController playerController;


		/// <summary>
		/// Current energy of orbit.
		/// </summary>
		public int energy = 100;

		/// <summary>
		/// Energy text object.
		/// </summary>
		public Text energyText;

		/// <summary>
		/// Energy bar,
		/// </summary>
		public Image energyBar;

		/// <summary>
		/// Round timer bar. Shows round time progress.
		/// </summary>
		public Image waveTimerBar;

		/// <summary>
		/// UI damage screen.
		/// </summary>
		public GameObject damageScreen;

		/// <summary>
		/// Orbit object.
		/// </summary>
		public GameObject orbit;

		/// <summary>
		/// Colour to change orbit damage to.
		/// </summary>
		public Color orbitDamagedColour;

		/// <summary>
		/// Current round timer.
		/// </summary>
		public float waveTimer;

		/// <summary>
		/// Current round time max.
		/// </summary>
		public float waveTimerMax;

		/// <summary>
		/// Current round index.
		/// </summary>
		public int roundIndex;

		/// <summary>
		/// Orbit scale.
		/// </summary>
		public Scale orbitScale;

		/// <summary>
		/// Orbit rotation vector. Rotates title around Orbit.
		/// </summary>
		public RotateAroundVector orbitRotateAround;

		/// <summary>
		/// Title text.
		/// </summary>
		public Text titleText;

        /// <summary>
        /// Title to be displayed during pause.
        /// </summary>
        public Text pauseText;


		private Color _tmpOrbitColor;
		private bool _orbitIsDamaged = false;
		private float _powerLevelMax = 100f;
		private float _powerLevel;
		private float _overheatLevelInc = 15f;

		private GameState _gameState;

		/// <summary>
		/// Gets and sets current game state. 
		/// </summary>
		public GameState gameState {
			get {
				return _gameState;
			}

			set {
				_gameState = value;

				if (_gameState == GameState.Play) {
					StartCoroutine (OnNewRound ());
					InitNewWave ();
				}
			}
		}

		private Renderer _damageScreenRenderer;
		private Renderer _orbitRenderer;
		private CameraShake _cameraShake;

		private bool _sceneCleared;
		private float gameOverPauseTime = 1.5f;
		private float currentGameOverTime = 0f;

        private GroupAssetLibrary<AudioClip> _audioLibrary;

		void Awake ()
		{

			if (!_damageScreenRenderer) {
				_damageScreenRenderer = damageScreen.GetComponent<Renderer> ();
				_damageScreenRenderer.enabled = false;
			}

			if (!_orbitRenderer) {
				_orbitRenderer = orbit.GetComponent<Renderer> ();
			}

			if (!_cameraShake) {
				_cameraShake = Camera.main.GetComponent<CameraShake> ();
			}

			_gameState = GameState.NotStarted;

		}

		void Start ()
		{

            _audioLibrary = new GroupAssetLibrary<AudioClip>("music_list");
			BackgroundAudio.Instance.Play (_audioLibrary.GetAssetFromName("Title"));
		}

		/// <summary>
		/// Pauses game, shows title screen and disables camera shake.
		/// </summary>
		public void Pause ()
		{
			titleText.text = "Paused";
			Time.timeScale = 0f;
			orbitRotateAround.Stop ();
            pauseText.gameObject.SetActive(true);
			_cameraShake.shake = 0f;
			_gameState = GameState.Pause;
		}

		/// <summary>
		/// Resumes game that was previously paused.
		/// </summary>
		public void Resume ()
		{
			Time.timeScale = 1f;
            pauseText.gameObject.SetActive(false);
            _gameState = GameState.Play;
		}

		/// <summary>
		/// Starts game. Changes state to playing. Scales orbit to desired scale, spawns player, updates energy, and begins wave spawning.
		/// </summary>
		public void BeginGame ()
		{
			_gameState = GameState.Play;

			BackgroundAudio.Instance.Play (_audioLibrary.GetAssetFromName("game_music"));

			orbitScale.Execute ();

			playerController.Spawn ();

			UpdateEnergy (0);
			ScoreController.Instance.OnGameStart ();

			_damageScreenRenderer.enabled = false;

			_powerLevel = _powerLevelMax;
			InitNewWave ();
		}


		private void InitNewWave ()
		{
			waveTimerMax += Random.Range (0f, 10f);
			waveTimer = waveTimerMax;
			_sceneCleared = false;

			enemySpawner.OnNewRound ();

			helpfulShapeSpawner.SpawnShape ();

			if (roundIndex > 0) {
				wallSpawner.SpawnWalls ();
			}
		}

        private void UpdateGameOver()
        {
   
                currentGameOverTime += Time.deltaTime;

                if (currentGameOverTime >= gameOverPauseTime && Input.GetMouseButton(0))
                {
                    currentGameOverTime = 0f;
                    SceneManager.LoadScene(0);
                }

            
        }

        private void UpdatePlay()
        {

            #region debug

            /* if (Input.GetKey(KeyCode.S))
            {


                updateEnergy(100);

            }

            if (Input.GetKeyUp (KeyCode.S)) {
                ScoreController.instance.UpdateScore (5000);
                CollectibleManager.instance.Collected (20);
                CollectibleManager.instance.SpawnCollectible ();
                updateEnergy (100);

            }

                 if (Input.GetKey(KeyCode.Space))
            {
                OnWaveComplete();
            }
*/




            #endregion

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    playerController.ChangeDirection(_powerLevel / _powerLevelMax);
                    _powerLevel -= 20f;
                }

                enemySpawner.Execute();



                if (_damageScreenRenderer.enabled)
                {
                    _tmpOrbitColor = _damageScreenRenderer.material.color;
                    _tmpOrbitColor.a -= Time.deltaTime * .5f;
                    _damageScreenRenderer.material.color = _tmpOrbitColor;
                    if (_tmpOrbitColor.a <= 0)
                    {
                        _damageScreenRenderer.enabled = false;
                    }
                }

                if (_orbitIsDamaged)
                {
                    _tmpOrbitColor = _orbitRenderer.material.color;
                    _tmpOrbitColor.a += Time.deltaTime * 2f;


                    if (_tmpOrbitColor.a >= 1f)
                    {
                        _tmpOrbitColor.a = 1f;
                        _orbitIsDamaged = false;
                    }

                    _orbitRenderer.material.color = _tmpOrbitColor;
                }

                if (_powerLevel < _powerLevelMax)
                {
                    _powerLevel += (_overheatLevelInc + (_powerLevelMax - _powerLevel)) * Time.deltaTime * .8f;

                    if (_powerLevel > _powerLevelMax)
                    {
                        _powerLevel = _powerLevelMax;
                    }

                }

                waveTimer -= Time.deltaTime;

                float yScale = (waveTimer / waveTimerMax) * (Screen.width * 0.7f);

                waveTimerBar.rectTransform.sizeDelta = new Vector2(waveTimerBar.rectTransform.sizeDelta.x, yScale);

                if (waveTimer <= 0 && !_sceneCleared)
                {
                    OnWaveComplete();
                    _sceneCleared = true;
                }
            
        }

		void Update ()
		{
	
            switch (_gameState)
            {
                case GameState.GameOver:
                    UpdateGameOver();
                    break;
                case GameState.Play:
                    UpdatePlay();
                    break;
            }	
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>true if energy is greater than or equal to maximum.</returns>
		public bool EnergyFull ()
		{
			return energy >= 100;
		}

		/// <summary>
		/// updates energy.
		/// </summary>
		/// <param name="val">The amount to increase/decrease energy.</param>
		public void UpdateEnergy (int val)
		{
			if (gameState != GameState.Play)
				return;

            if(val < 0)
            {
                SoundEffectPlayer.Instance.PlayClip("player_hit");
            }

			int currentEnergy = energy;

			if ((currentEnergy != 100 && val > 0) || (val < 0 && currentEnergy - val > 0)) {
				energyManager.PlayEnergyAnimation ();
			}


			energy += val;

			if (energy > 100)
				energy = 100;

			energyText.text = energy.ToString ();
			if (val < 0) {
				if (val < -5) {
					_damageScreenRenderer.enabled = true;
					_tmpOrbitColor = _damageScreenRenderer.material.color;
					_tmpOrbitColor.a = 1f;
					_damageScreenRenderer.material.color = _tmpOrbitColor;

					_cameraShake.shake = val / -15f;
				} else {
					_orbitIsDamaged = true;
					_tmpOrbitColor = _orbitRenderer.material.color;
					_tmpOrbitColor.a = 0.3f;
					_orbitRenderer.material.color = _tmpOrbitColor;

					_damageScreenRenderer.enabled = true;
					_tmpOrbitColor = _damageScreenRenderer.material.color;
					_tmpOrbitColor.a = .3f;
					_damageScreenRenderer.material.color = _tmpOrbitColor;

					_cameraShake.shake = .02f;
				}
			}

			if (energy <= 0) {
				OnGameOver ();
			}
		}

		private void OnGameOver ()
		{

			gameState = GameState.GameOver;

			BackgroundAudio.Instance.Play (_audioLibrary.GetAssetFromName("GameOver"));

			ScoreController.Instance.OnGameOver ();

			energyText.text = "Game Over";
			ClearStage ();
		}

		private IEnumerator ShowWaveCompleteText ()
		{
			waveTimerBar.enabled = false;

			energyText.text = "Wave Complete";

			yield return new WaitForSeconds (1.4f);

			shopController.OpenShop ();
		}

		private IEnumerator OnNewRound ()
		{
			waveTimerBar.enabled = true;

			roundIndex++;

			energyText.text = "Wave " + (roundIndex + 1).ToString ();

			yield return new WaitForSeconds (1.8f);

			energyText.text = energy.ToString ();
		}

		/// <summary>
		/// Clears stage.
		/// </summary>
		public void OnWaveComplete ()
		{
			_gameState = GameState.Pause;

			StartCoroutine (ShowWaveCompleteText ());

			ClearStage ();
		}

		private void ClearStage ()
		{
			enemySpawner.RemoveAllEnemies ();

			GameObject[] bullets;
			bullets = GameObject.FindGameObjectsWithTag ("bullet");
			foreach (GameObject bullet in bullets) {
				Destroy (bullet);
			}

			GameObject[] deathObjects = GameObject.FindGameObjectsWithTag ("deathObject");
			foreach (GameObject dObj in deathObjects) {
				dObj.GetComponent<DeathObject> ().OnDead ();
			}

			helpfulShapeSpawner.RemoveSpawnedShapes ();

			CollectibleManager.instance.RemoveAllCollectibles ();

			wallSpawner.RemoveWalls ();

			HealthSpawner.Instance.RemoveSpawned ();
		}

		/// <summary>
		/// Instantiates damage particle at desired position.
		/// </summary>
		/// <param name="x">x position.</param>
		/// <param name="y">y position.</param>
		public void addParticle (float x, float y)
		{
			Instantiate (particleDamage, new Vector3 (x, y, 0f), new Quaternion ());
		}

	}
}