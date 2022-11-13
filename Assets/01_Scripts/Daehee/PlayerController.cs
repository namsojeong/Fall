using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _playerRB;

    private Animator _playerAnim;
    #region PlayerMoveSettingValue
    [SerializeField]
    private float _walkSpeed = 10f;
    [SerializeField]
    private float _runSpeed = 20f;
    [SerializeField]
    private float curSpeed;
    [SerializeField]
    public float _jumpHeight = 10f;
    [SerializeField]
    private float _dashLength = 30f;
    [SerializeField]
    private float _rotateSpeed = 3f;

    public bool _isGround = false;

    private bool _isMove = false;

    public LayerMask _playerLayer;

    private Vector3 _moveDir = Vector3.zero;
    private Vector3 _moveRotDir = Vector3.zero;
    #endregion

    public CinemachineFreeLook vcam;

    void Start()
    {
        _playerRB = this.GetComponent<Rigidbody>();
        _playerAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _moveDir.x = Input.GetAxis("Horizontal");
        _moveDir.z = Input.GetAxis("Vertical");
        _moveDir.Normalize();
        CheckGround();

        if (Input.GetButtonDown("Jump")&&_isGround)
        {
            _playerAnim.SetTrigger("jump");
            _playerRB.drag = 3;
            Vector3 jumpPower = Vector3.up * _jumpHeight;
            _playerRB.AddForce(jumpPower, ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Dash");
            _playerRB.drag = 10;
            Vector3 dashPower = transform.forward * (-Mathf.Log(1 / _playerRB.drag)) *_dashLength;
            _playerRB.AddForce(dashPower, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.LeftShift) && _isMove)
        {
            curSpeed = Mathf.Lerp(curSpeed,_runSpeed,2) ;
        }
        else if (_isMove)
            curSpeed = Mathf.Lerp(curSpeed, _walkSpeed, 2);
        else
            curSpeed = Mathf.Lerp(curSpeed,0, 2);

        _playerAnim.SetFloat("speed", curSpeed);
    }
    private void FixedUpdate()
    {
        if (_moveDir == Vector3.zero)
            _isMove = false;
        else
        {
            _isMove = true;
            
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(_moveDir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(_moveDir.z))
                transform.Rotate(0, 1, 0);
            transform.forward = Vector3.Lerp(transform.forward, _moveDir, Time.deltaTime * _rotateSpeed);
        }
        _playerRB.MovePosition(this.gameObject.transform.position + _moveDir * curSpeed * Time.deltaTime);
    }


    void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + (Vector3.up * 0.2f), Vector3.down, out hit, 0.4f, _playerLayer))
            _isGround = true;
        else
            _isGround = false;
    }
}
