using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Orbit
{
    /// <summary>
    /// Resposible for spawning collectibles and tracking current collectible amount.
    /// </summary>
    public class CollectibleManager : MonoBehaviour
    {
        /// <summary>
        /// The text displayed when a collectible has been picked up.
        /// </summary>
        public Text collectibleText;

        /// <summary>
        /// Collectible prefab.
        /// </summary>
        public GameObject collectiblePrefab;

        /// <summary>
        /// Time between attempts at placing collectibles.
        /// </summary>
        public float timeBetweenCollectibleSpawns;

        /// <summary>
        /// Chance of spawning.
        /// </summary>
        [Range(0f, 90f)]
        public float
            percentageChanceOfSpawn = 80f;

        /// <summary>
        /// CHance to spawn an additional collectible.
        /// </summary>
        [Range(0f, 100f)]
        public float
            percentageChanceToSpawnAdditional = 5;

        /// <summary>
        /// The current collectible amount.
        /// </summary>
        public int collected { get; private set; }

        private CollectibleSpawnLocation[] _spawnLocations;

        private const float MINIMUM_SPAWN_TIME = 2f;
        private const float MAXIMUM_PERCENTAGE_CHANGE = 85f;

        private List<GameObject> _spawnedCollectibles = new List<GameObject>();

        private float _currentSpawnTime;
        private const int COLLECTIBLE_SPAWN_ATTEMPTS = 10;
        private Animator _textAnimator;

        private static CollectibleManager _instance;

        /// <summary>
        /// Singleton. returns an instance of CollectibleManager.
        /// </summary>
        public static CollectibleManager instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindObjectOfType<CollectibleManager>();
                    _instance.Initialise();
                }

                return _instance;
            }
        }

        void Awake()
        {
            if (!_instance)
            {
                _instance = this;
                _instance.Initialise();
            }
        }

        private void Initialise()
        {
            _spawnLocations = GetComponentsInChildren<CollectibleSpawnLocation>();
            _textAnimator = collectibleText.GetComponent<Animator>();
            collectibleText.enabled = false;
        }

        void OnEnable()
        {
            _currentSpawnTime = 0f;
        }

        /// <summary>
        /// Increments collected, shows and animates collectible text.
        /// </summary>
        /// <param name="collectible">The collectible consumed by player.</param>
        public void Collected(Collectible collectible)
        {
            _spawnedCollectibles.Remove(collectible.gameObject);

            collected += collectible.increaseAmount;
            collectibleText.transform.position = Camera.main.WorldToScreenPoint(collectible.transform.position);

            StartCoroutine(UpdateCollectibleText(collected));
        }

        /// <summary>
        /// Increments collected amount.
        /// </summary>
        /// <param name="amount"></param>
        public void Collected(int amount = 1)
        {
            collected += amount;
        }

        /// <summary>
        /// Increments chance of spawning collectibles (used when player buys collectible upgrade).
        /// </summary>
        /// <param name="percentageIncrease">The percentage chance to spawn a collectible.</param>
        /// <param name="spawnTimeDecrease">The time between collectible spawns.</param>
        /// <param name="additionalPercentageIncrease">The percentage chance to spawn additional collectibles.</param>
        public void IncreaseSpawnChance(int percentageIncrease = -1, float spawnTimeDecrease = -1f, int additionalPercentageIncrease = -1)
        {
            if (percentageIncrease > 0)
            {
                percentageChanceOfSpawn += percentageIncrease;

                if (percentageChanceOfSpawn > MAXIMUM_PERCENTAGE_CHANGE)
                    percentageChanceOfSpawn = MAXIMUM_PERCENTAGE_CHANGE;
            }

            if (spawnTimeDecrease > 0)
            {
                timeBetweenCollectibleSpawns -= spawnTimeDecrease;

                if (timeBetweenCollectibleSpawns < MINIMUM_SPAWN_TIME)
                {
                    timeBetweenCollectibleSpawns = MINIMUM_SPAWN_TIME;
                }
            }

            if (additionalPercentageIncrease > 0)
            {
                percentageChanceToSpawnAdditional += additionalPercentageIncrease;

                if (percentageChanceToSpawnAdditional > MAXIMUM_PERCENTAGE_CHANGE)
                {
                    percentageChanceToSpawnAdditional = MAXIMUM_PERCENTAGE_CHANGE;
                }
            }
        }

        /// <summary>
        /// Reduces current collected amount. Invoked when upgrade are bought.
        /// </summary>
        /// <param name="amount"></param>
        public void Spent(int amount = 1)
        {
            collected -= amount;

            if (collected < 0)
            {
                collected = 0;
            }
        }

        /// <summary>
        /// Remove all currently spawned collectibles. Called on round end.
        /// </summary>
        public void RemoveAllCollectibles()
        {
            for (int i = 0; i < _spawnedCollectibles.Count; i++)
            {
                Destroy(_spawnedCollectibles[i]);

            }
        }

        private void SpawnCollectible()
        {
            _spawnLocations.Shuffle();

            for (int i = 0; i < _spawnLocations.Length; i++)
            {
                if (_spawnLocations[i].isFree)
                {

                    _spawnedCollectibles.Add(
                        (GameObject)Instantiate(collectiblePrefab, _spawnLocations[i].transform.position, _spawnLocations[i].transform.rotation)
                    );

                    float additionalSpawnChance = i > 1 ? percentageChanceToSpawnAdditional / i : percentageChanceToSpawnAdditional;

                    if (Random.value > additionalSpawnChance / 100)
                    {
                        break;
                    }

                }
            }
        }

        void Update()
        {
            if (GameManager.Instance.gameState != GameState.Play)
            {
                ResetTimer();
                return;
            }

            _currentSpawnTime += Time.deltaTime;

            if (OkToSpawn())
            {
                ResetTimer();
                SpawnCollectible();
            }
        }

       

        private IEnumerator UpdateCollectibleText(int score)
        {
            collectibleText.text = score.ToString();
            collectibleText.enabled = true;

            yield return new WaitForSeconds(1f);

            _textAnimator.Play("textFadeOut");

            collectibleText.enabled = false;

        }

        private void ResetTimer()
        {
            _currentSpawnTime = 0f;
        }

     

        private bool OkToSpawn()
        {
            return (_currentSpawnTime >= timeBetweenCollectibleSpawns) && (Random.value < percentageChanceOfSpawn / 100);
        }
    }
}