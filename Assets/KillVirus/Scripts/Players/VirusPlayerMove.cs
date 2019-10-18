using Events;
using Tool;
using UnityEngine;

public class VirusPlayerMove : MonoBehaviour, IEventListener<FirstByteClick1DownEvent>,
                                              IEventListener<FirstByteClick2DownEvent>,
                                              IEventListener<FirstByteClick3DownEvent>,
                                              IEventListener<FirstByteClick4DownEvent>,

                                              IEventListener<FirstByteClick1UpEvent>,
                                              IEventListener<FirstByteClick2UpEvent>,
                                              IEventListener<FirstByteClick3UpEvent>,
                                              IEventListener<FirstByteClick4UpEvent>
{

    [SerializeField] private float xmoveSpeed;
    [SerializeField] private float ymoveSpeed;


    private bool _isLeft;
    private bool _isRight;
    private bool _isUp;
    private bool _isDown;
    private bool _isSlowDown;
    private bool _isTouchOn;
    private float _multiplier;


    public Vector3 MoveDirection { set; get; }

    public bool IsControl { set; get; }


    public void OnAwake()
    {
        _isLeft = false;
        _isRight = false;
        _isUp = false;
        _isDown = false;
        _isSlowDown = false;
        MoveDirection = Vector3.right;

        _multiplier = 1;
    }

    public void OnUpdate()
    {
        if (IsControl)
        {
            ClickKeyUpdate();
            MoveUpdate();
            MultiplierUpdate();

            TouchMoveUpdate();
        }
    }



    private void OnEnable()
    {
        EventRegister.EventStartListening<FirstByteClick1DownEvent>(this);
        EventRegister.EventStartListening<FirstByteClick2DownEvent>(this);
        EventRegister.EventStartListening<FirstByteClick3DownEvent>(this);
        EventRegister.EventStartListening<FirstByteClick4DownEvent>(this);

        EventRegister.EventStartListening<FirstByteClick1UpEvent>(this);
        EventRegister.EventStartListening<FirstByteClick2UpEvent>(this);
        EventRegister.EventStartListening<FirstByteClick3UpEvent>(this);
        EventRegister.EventStartListening<FirstByteClick4UpEvent>(this);
    }

    private void OnDisable()
    {
        EventRegister.EventStopListening<FirstByteClick1DownEvent>(this);
        EventRegister.EventStopListening<FirstByteClick2DownEvent>(this);
        EventRegister.EventStopListening<FirstByteClick3DownEvent>(this);
        EventRegister.EventStopListening<FirstByteClick4DownEvent>(this);

        EventRegister.EventStopListening<FirstByteClick1UpEvent>(this);
        EventRegister.EventStopListening<FirstByteClick2UpEvent>(this);
        EventRegister.EventStopListening<FirstByteClick3UpEvent>(this);
        EventRegister.EventStopListening<FirstByteClick4UpEvent>(this);
    }

    private void ClickKeyUpdate()
    {

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick1DownEvent());
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick1UpEvent());
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick2DownEvent());
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick2UpEvent());
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick3DownEvent());
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick3UpEvent());
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick4DownEvent());
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            EventManager.TriggerEvent(new FirstByteClick4UpEvent());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            EventManager.TriggerEvent(new FirstByteClick5DownEvent());
        }

    }

    private void MoveUpdate()
    {
        Vector3 pos = transform.position;
        float xdelta = xmoveSpeed * Time.deltaTime * _multiplier;
        float ydelta = ymoveSpeed * Time.deltaTime * _multiplier;
        if (_isRight)
        {
            transform.position += new Vector3(xdelta, 0, 0);
            if (transform.position.x > 4.8f)
            {
                transform.position = new Vector3(4.8f, transform.position.y, 0);
            }
        }
        if (_isLeft)
        {
            transform.position += new Vector3(-xdelta, 0, 0);
            if (transform.position.x < -4.8f)
            {
                transform.position = new Vector3(-4.8f, transform.position.y, 0);
            }
        }
        if (_isUp)
        {
            transform.position += new Vector3(0, ydelta, 0);
            if (transform.position.y > 9f)
            {
                transform.position = new Vector3(transform.position.x, 9, 0);
            }
        }
        if (_isDown)
        {
            transform.position += new Vector3(0, -ydelta, 0);
            if (transform.position.y < -8f)
            {
                transform.position = new Vector3(transform.position.x, -8, 0);
            }
        }
        Vector3 d = transform.position - pos;
        if (Mathf.Abs(d.x) > 0.01f || Mathf.Abs(d.y) > 0.01f)
        {
            MoveDirection = d;
        }
    }

    private void MultiplierUpdate()
    {
        float delta = Time.deltaTime * 10;
        if (_isSlowDown)
        {
            _multiplier -= delta;
            if (_multiplier < 0.4f)
            {
                _multiplier = 0.4f;
            }
        }
        else
        {
            _multiplier += delta;
            if (_multiplier >= 1)
            {
                _multiplier = 1;
            }
        }
    }

    private void TouchMoveUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(pos, 1 << LayerMask.NameToLayer("Player"));
            if (col != null)
            {
                _isTouchOn = true;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (_isTouchOn)
            {
                ClickPlayer();
            }
            _isTouchOn = false;
        }
        if (Input.GetMouseButton(0))
        {
            if (_isTouchOn)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10);
                if (transform.position.x > 4.8f)
                    transform.position = new Vector3(4.8f, transform.position.y, 0);
                if (transform.position.x < -4.8f)
                    transform.position = new Vector3(-4.8f, transform.position.y, 0);
                if (transform.position.y > 9f)
                    transform.position = new Vector3(transform.position.x, 9, 0);
                if (transform.position.y < -8f)
                    transform.position = new Vector3(transform.position.x, -8, 0);
            }
        }
    }


    private void ClickPlayer()
    {
        EventManager.TriggerEvent(new FirstByteClick5DownEvent());
    }
    public void Decelerate()
    {
        _isSlowDown = true;
    }

    public void Recover()
    {
        _isSlowDown = false;
    }


    #region Events

    public void OnEvent(FirstByteClick1DownEvent eventType)
    {
        _isLeft = true;
    }

    public void OnEvent(FirstByteClick2DownEvent eventType)
    {
        _isRight = true;
    }

    public void OnEvent(FirstByteClick3DownEvent eventType)
    {
        _isUp = true;
    }

    public void OnEvent(FirstByteClick4DownEvent eventType)
    {
        _isDown = true;
    }



    public void OnEvent(FirstByteClick1UpEvent eventType)
    {
        _isLeft = false;
    }

    public void OnEvent(FirstByteClick2UpEvent eventType)
    {
        _isRight = false;
    }

    public void OnEvent(FirstByteClick3UpEvent eventType)
    {
        _isUp = false;
    }

    public void OnEvent(FirstByteClick4UpEvent eventType)
    {
        _isDown = false;
    }

    
    #endregion


}