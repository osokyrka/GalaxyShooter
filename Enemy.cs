using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explosionSound;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _doubleLaser;
    private bool _isAlive;

    private float _fireRate = 3f;
    private float _canFire = -10f;

    // Update is called once per frame
    private void Start()
    {
        _isAlive = true;
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSound;
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("player is null");
        }
        _anim = gameObject.GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("anim is null");
        }
    }
    void Update()
    {
        EnemyLaser();
        EnemyMovement();
    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    void EnemyLaser()
    {
        if(Time.time > _canFire && _isAlive == true)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_doubleLaser, transform.position, Quaternion.identity);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _isAlive = false;
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.37f);
        }
        if (other.tag == "Laser")
        {
            _anim.SetTrigger("OnEnemyDeath");
            _isAlive = false;
            Destroy(other.gameObject);
            Destroy(GetComponent<Collider2D>());
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.37f);
            if(_player != null)
            {
                _player.ScoreCount(10);
            }
           
        }
    }

    
}
