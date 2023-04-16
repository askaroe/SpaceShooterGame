using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f; // period of shooting, every laser will spawn after 0.5 seconds
    private float _canFire = -1f; // this variable determines, can I fire or not
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isShieldActice = false;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _rightEngineVisualizer;
    [SerializeField]
    private GameObject _leftEngineVisualizer;

    [SerializeField]
    private int _score = 0;

    private UIManager _UIManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>(); // getting acces to a SpawnManager script in the Spawn_Manager object
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>(); // getting access to a UI manager from Canvas object
        _audioSource = GetComponent<AudioSource>();
        if (_spawnManager == null)
        {
            Debug.LogError("Hey! Another developer says that Spawn manager is NULL, it's ok, it is just a message :)");
        }

        if(_UIManager == null)
        {
            Debug.LogError("Hey! Your addingScoresUI is NULL, dont forget to fix :D");
        }
        if(_audioSource == null)
        {
            Debug.LogError("Hey! Your audioSource on the player is NULL, that is just a message :)");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

#if UNITY_ANDROID
            if ((Input.GetKeyDown(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire")) && Time.time > _canFire) // if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                fireLaser();
            }

#else
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
            {
                fireLaser();
            }
#endif
    }
    void CalculateMovement()
    {

#if UNITY_ANDROID
        float horizontalInput = CrossPlatformInputManager.GetAxis("Horizontal"); // Input.GetAxis("Horizontal");
        float verticalInput = CrossPlatformInputManager.GetAxis("Vertical"); // Input.GetAxis("Vertical");

#else

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


#endif

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.2f)
        {
            transform.position = new Vector3(-11.2f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.2f)
        {
            transform.position = new Vector3(11.2f, transform.position.y, 0);
        }
    }

    void fireLaser()
    {
        _canFire = Time.time + _fireRate; //Time.time counts time since game was started, to this time
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(3.55f, 1.39f, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        _audioSource.Play();
    }

    public void Damage()
    {
        if(_isShieldActice == true)
        {
            _isShieldActice = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives -= 1;

        if(_lives == 2)
        {
            _rightEngineVisualizer.SetActive(true);
        }

        else if(_lives == 1)
        {
            _leftEngineVisualizer.SetActive(true);
        }

        _UIManager.UpdateLives(_lives);
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath(); // connecting with SpawnManager script
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    private IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed /= _speedMultiplier;
    }

    public void ShieldActive()
    {
        _shieldVisualizer.SetActive(true);
        _isShieldActice = true;
    }

    public void AddingScore(int points)
    {
        _score += points; // we are adding 10 score to the our current score
        _UIManager.AddingScoresUI(_score); // and passing this to the AddingScoreUI method from UIManager, it takes integer as a parameter
    }

    public int ShowLives()
    {
        return _lives; // for enemy, because I had a problem, when player is dead, one enemy was spawning
    }
}
