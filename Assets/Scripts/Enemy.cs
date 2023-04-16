using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _enemySpeed = 4f;

    private Player _player; // creating handle of player 

    private Animator _anim;

    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate;
    private float _canFire = -1f;

    private AudioSource _audioSource;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>(); // getting access to a player component
        _anim = gameObject.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if(_player == null)
        {
            Debug.LogError("Hey, your player component is NULL");
        }

        if (_anim == null)
        {
            Debug.LogError("Hey, your animator component is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssigningEnemyLaser();
            }
        }
    }


    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f); //Random.Range(float min, float max); returns random float number between min and max
            transform.position = new Vector3(randomX, 7f, 0);
            //Destroy(this.gameObject);
        }
        if (_player.ShowLives() == 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if(player != null) // avoid NullException error 
            {
                player.Damage();
            }
            _enemySpeed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.5f);
        }

        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddingScore(10); // calling adding score method from Player script
            }
            _enemySpeed = 0;
            _anim.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>()); // enemy explosion bug fix
            Destroy(this.gameObject, 2.5f); //Destroy(gameObject, (float)time) the Destroy() method will executed after time
        }
    }
}
