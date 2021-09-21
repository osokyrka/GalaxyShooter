using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    [SerializeField]
    private bool _isTrippleShotActive = false;
    private bool _isSpeedUpActive = false;
    [SerializeField]
    private float _speedUpSpeed = 20f;
    private bool _isShieldActive = false;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private int _score = 0;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _fireLeft, _fireRight;
    [SerializeField]
    private AudioClip _laserShot;
    private AudioSource _audioSource;
    


    // Start is called before the first frame update
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shield.gameObject.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _fireLeft.gameObject.SetActive(false);
        _fireRight.gameObject.SetActive(false);

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserShot;
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL");
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        LaserShooting();
    }

    void CalculateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        

        if(_isSpeedUpActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speedUpSpeed * Time.deltaTime);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
             
        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void LaserShooting()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            _canFire = Time.time + _fireRate;
            if(_isTrippleShotActive == true)
            {
                Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            _audioSource.Play();
            
        }
    }

    public void Damage()
    {
        if(_isShieldActive == false)
        {
            _lives -= 1;
            _uiManager.UpdateLives(_lives);
            if(_lives == 2)
            {
                _fireLeft.gameObject.SetActive(true);
            }
            else if(_lives == 1)
            {
                _fireLeft.gameObject.SetActive(true);
                _fireRight.gameObject.SetActive(true);
            }
            else if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                _fireLeft.gameObject.SetActive(false);
                _fireRight.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
        }
        else
        {
            _isShieldActive = false;
            _shield.gameObject.SetActive(false);
        }
        
    }

    public void ScoreCount(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActive()
    {
        _isTrippleShotActive = true;
        StartCoroutine(TripleShotCooldown());
    }

    public void SpeedUpActive()
    {
        _isSpeedUpActive = true;
        StartCoroutine(SpeedUpCooldown());
    }
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shield.gameObject.SetActive(true);
    }
    
    
    

    IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTrippleShotActive = false;
    }

    IEnumerator SpeedUpCooldown()
    {       
        yield return new WaitForSeconds(5.0f);
        _isSpeedUpActive = false;
    }
}
