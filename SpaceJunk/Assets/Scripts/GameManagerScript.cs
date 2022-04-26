using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{


    [Header("Hierarchy")]
    [SerializeField] Transform junkParent;
    [SerializeField] Transform asteroidParent;

    [Header("Config")]
    [SerializeField] float _gameTimeInSeconds = 300;
    [SerializeField] float _timeToSpawnJunk;
    [SerializeField] float _timeToSpawnAsteroids;

    [SerializeField] float _levelWidth;
    [SerializeField] float _levelHeight;

    [SerializeField] int _maxJunkToSpawn = 20;
    [SerializeField] int _maxAsteroidsToSpawn = 20;

    [SerializeField] float _junkSpawnMultiplier = 0;

    float _accGameTime;
    float _junkSpawnTimer;
    float _asteroidSpawnTimer;

    int _amountOfJunkSpawned = 0;
    int _amountOfAsteroidsSpawned = 0;

    [SerializeField] GameObject spaceJunk;
    [SerializeField] GameObject asteroid;

    [SerializeField] BoxCollider playZone;

    [SerializeField] GameObject levelBoundaries;

    bool _IsGameStarted = false;
    bool _IsGameFinished = false;
    bool _IsGameOvertime = false;

    HUDManager hudManager;

    List<Color> playerColors;

    // Start is called before the first frame update
    void Awake()
    {
        if (playZone != null)
        {
            playZone.size = new Vector3(_levelWidth + 5, 20, _levelHeight + 1);
        }

        levelBoundaries = Instantiate(levelBoundaries, new Vector3(0, 0, 0), new Quaternion());
        var boundaries = levelBoundaries.GetComponent<LevelBoundariesScript>();
        boundaries.SetBoundaries(_levelWidth, _levelHeight);

        hudManager = FindObjectOfType<HUDManager>();

        playerColors = FindObjectOfType<PlayerSpawner>().PlayerColors();

        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_amountOfJunkSpawned < _maxJunkToSpawn)
        {
            _junkSpawnTimer += Time.deltaTime;

            if (_junkSpawnTimer >= _timeToSpawnJunk)
            {
                SpawnJunk();
                _junkSpawnTimer = 0;
                _amountOfJunkSpawned++;
            }
        }

        if (_amountOfAsteroidsSpawned < _maxAsteroidsToSpawn)
        {
            _asteroidSpawnTimer += Time.deltaTime;

            if (_asteroidSpawnTimer >= _timeToSpawnAsteroids)
            {
                //spawn asteroid
                SpawnAsteroid();
                _asteroidSpawnTimer = 0;
                _amountOfAsteroidsSpawned++;
            }
        }


        // MAIN GAME
        if (_IsGameStarted && !_IsGameFinished && !_IsGameOvertime)
        {
            _accGameTime += Time.deltaTime;
            hudManager.SetTimer(_gameTimeInSeconds - _accGameTime);
            if (_accGameTime > _gameTimeInSeconds)
            {
                // If there is no tie
                // Show Finish Screen In Slomo

                var collectors = FindObjectsOfType<JunkCollector>();
                int playerId = 0;
                int score = 0;
                bool isTie = false;
                foreach(var collector in collectors)
                {
                    if (collector.Score > score)
                    {
                        score = collector.Score;
                        playerId = collector.playerCollectorIndex + 1;
                    }
                    else if (collector.Score == score)
                        isTie = true;
                }

                if (!isTie)
                {
                    Color playerColor = playerColors[playerId-1];
                   
                    // Get winner id
                    hudManager.SetWinnerText(playerId.ToString(), playerColor);
                    hudManager.ShowFinishScreen();
                    Time.timeScale = 0.2f;
                }
                else
                {
                    hudManager.SetTimerTextOvertime();
                    _IsGameOvertime = true;
                }
            }
            
        }
        // OVERTIME
        else if (_IsGameOvertime && !_IsGameFinished)
        {
            // Bad but don't care
            var collectors = FindObjectsOfType<JunkCollector>();
            int playerId = 0;
            int score = 0;
            // Get Highest Player
            foreach (var collector in collectors)
            {
                if (collector.Score > score)
                {
                    score = collector.Score;
                    playerId = collector.playerCollectorIndex + 1;
                }
            }
            // Check if someone has the same one 
            bool isTie = false;
            foreach (var collector in collectors)
            {
                
                if (collector.Score == score && playerId != collector.playerCollectorIndex + 1)
                    isTie = true;
            }

            if (!isTie)
            {
                // Get winner id
                Color playerColor = playerColors[playerId-1];
                
                hudManager.SetWinnerText(playerId.ToString(), playerColor);
                hudManager.ShowFinishScreen();
                Time.timeScale = 0.2f;
                _IsGameFinished = true;
            }
        }

        // MAIN GAME HUD
        if (!_IsGameFinished && !_IsGameOvertime) hudManager.SetTimer(_gameTimeInSeconds - _accGameTime);

        

    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.transform.parent.name == "SpaceJunk" || other.transform.parent.name == "SpaceJunk(Clone)")
        {
            _amountOfJunkSpawned--;
            Destroy(other.gameObject.transform.parent.gameObject);
        }

        else if (other.CompareTag("Asteroid"))
        {
            _amountOfAsteroidsSpawned--;
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }

    private void SpawnJunk()
    {
        if(_IsGameStarted)
        {
            float xpos = Random.Range(-(_levelWidth + 10) / 2, (_levelWidth + 10) / 2);
            float zpos = 0;
            Vector3 rotation = new Vector3();

            int side = Random.Range(0, 4);

            if (side < 2)
            {
                zpos = ((_levelHeight + 5) / 2);

                if (xpos <= 0)
                {
                    rotation.y = Random.Range(125.0f, 160.0f);
                }

                else
                {
                    rotation.y = Random.Range(215.0f, 240.0f);
                }
            }

            else
            {
                zpos = -((_levelHeight + 5) / 2);

                if (xpos <= 0)
                {
                    rotation.y = Random.Range(25.0f, 60.0f);
                }

                else
                {
                    rotation.y = Random.Range(310.0f, 350.0f);
                }
            }

            Vector3 pos = new Vector3(xpos, 0.0f, zpos);
            GameObject newJunk = Instantiate(spaceJunk, pos, Quaternion.Euler(rotation));
            newJunk.transform.parent = junkParent;
        }
        
    }

    private void SpawnAsteroid()
    {
        if (_IsGameStarted)
        {
            float xpos = Random.Range(-(_levelWidth + 10) / 2, (_levelWidth + 10) / 2);
            float zpos = 0;
            Vector3 rotation = new Vector3();

            int side = Random.Range(0, 4);

            if (side < 2)
            {
                zpos = ((_levelHeight + 5) / 2);

                if (xpos <= 0)
                {
                    rotation.y = Random.Range(125.0f, 160.0f);
                }

                else
                {
                    rotation.y = Random.Range(215.0f, 240.0f);
                }
            }

            else
            {
                zpos = -((_levelHeight + 5) / 2);

                if (xpos <= 0)
                {
                    rotation.y = Random.Range(25.0f, 60.0f);
                }

                else
                {
                    rotation.y = Random.Range(310.0f, 350.0f);
                }
            }

            Vector3 pos = new Vector3(xpos, 0.0f, zpos);
            GameObject newAsteroid = Instantiate(asteroid, pos, Quaternion.Euler(rotation));
            newAsteroid.transform.parent = asteroidParent;
        }
    }

    public void SpawnStartJunk()
    {
        if (playZone != null)
        {
            playZone.size = new Vector3(_levelWidth, 20, _levelHeight);
        }

        float area = _levelWidth * _levelHeight;
        float amountToSpawn = area / _junkSpawnMultiplier;
        amountToSpawn = Mathf.Round(amountToSpawn);

        for (int i = 0; i < amountToSpawn; i++)
        {
            bool getAnotherPosition = false; 

            float posX = Random.Range(playZone.bounds.min.x, playZone.bounds.max.x);
            float posZ = Random.Range(playZone.bounds.min.z, playZone.bounds.max.z);

            Vector3 pos = new Vector3(posX, 0, posZ);

            do
            {
                getAnotherPosition = false;
                var colliders = Physics.OverlapSphere(pos, 2);

                foreach (var collider in colliders)
                {
                    if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Collector"))
                    {
                        getAnotherPosition = true;
                    }
                }

                if(getAnotherPosition)
                {
                    posX = Random.Range(playZone.bounds.min.x, playZone.bounds.max.x);
                    posZ = Random.Range(playZone.bounds.min.z, playZone.bounds.max.z);
                    pos = new Vector3(posX, 0, posZ);
                }
            } 
            while (getAnotherPosition);
           

            Vector3 rotation = new Vector3();

            if (posZ <= 0)
            {
                if (posX <= 0)
                {
                    rotation.y = Random.Range(0, 110);
                }

                else
                {
                    rotation.y = Random.Range(280, 360);
                }
            }

            else
            {
                if (posX <= 0)
                {
                    rotation.y = Random.Range(100, 180);
                }

                else
                {
                    rotation.y = Random.Range(180, 260);
                }
            }

            GameObject newJunk = Instantiate(spaceJunk, pos, Quaternion.Euler(rotation));
            newJunk.transform.parent = junkParent;

        }

        if (playZone != null)
        {
            playZone.size = new Vector3(_levelWidth + 5, 20, _levelHeight + 1);
        }

        _IsGameStarted = true;
    }

    public void DecreaseJunkCount()
    {
        _amountOfJunkSpawned--;
    }

    public bool GetIsStarted()
    {
        if (!_IsGameFinished)
        {
            return _IsGameStarted;
        }

        else
            return false;
        
    }
}
