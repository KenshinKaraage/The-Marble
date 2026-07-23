using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MarbleController : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    // ボールを動かす速度
    [SerializeField] float addMoveSpeed;
    public float nowAddMoveSpeed;
    // ボールの最大速度
    [SerializeField] float maxMoveSpeed;
    public float nowMaxMoveSpeed;
    // ボールが止まるまでにかかる時間
    [SerializeField] float stopTime;

    // ボールの倍率速度
    public float MuntiSpeed { set; get; }
    public float MuntiSpeedTime { set; get; }

    // ボールに力を加える場所
    [SerializeField] Vector3 forcePoint;
    // ボールの回転速度
    [SerializeField] float rotateSpeed;
    // マウス感度
    [SerializeField] float mouseSensitivity;

    // 地面レイヤー
    [SerializeField] LayerMask groundLayers;
    // ジャンプする際に加える力の大きさ
    [SerializeField] Vector3 jumpForce;

    // プレイヤーフォロワー
    [SerializeField] Transform playerFollowerTransform;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        MuntiSpeed = 1;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (InputManager.instance.IsAttractive)
            {
                // ボールをキーで動かす
                Move();

                // 床によってボールの速度を加える
                AddVelocity();

                // 速度の倍率を調整
                AdjustMuntiVelocity();

                // 速度によって回転方向を調整
                ControllRotation();

                // スペースキーでジャンプする
                Jump();
            }
            

            // 視点をマウスで操作する
            ControllView();

            AdjustPlayerFollowerPosition();
        }
    }

    private void Move()
    {
        // まずボールに力を加える方向を求める
        Vector3 moveDir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical") );


        nowAddMoveSpeed = addMoveSpeed * MuntiSpeed;
        nowMaxMoveSpeed = maxMoveSpeed * MuntiSpeed;

        // 決めた最大速度を超えた場合、その方向に力が入らない
        if (Input.GetAxisRaw("Horizontal") > 0 && Vector3.Dot(playerFollowerTransform.right, rb.linearVelocity) > nowMaxMoveSpeed)
        {
            moveDir.x = 0;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && Vector3.Dot(playerFollowerTransform.right, rb.linearVelocity) < -nowMaxMoveSpeed)
        {
            moveDir.x = 0;
        }

        if (Input.GetAxisRaw("Horizontal") > 0 && Vector3.Dot(playerFollowerTransform.forward, rb.linearVelocity) > nowMaxMoveSpeed)
        {
            moveDir.z = 0;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && Vector3.Dot(playerFollowerTransform.forward, rb.linearVelocity) < -nowMaxMoveSpeed)
        {
            moveDir.z = 0;
        }

        Vector3 force = (playerFollowerTransform.forward * moveDir.z + playerFollowerTransform.right * moveDir.x).normalized * nowAddMoveSpeed * Time.deltaTime;

        rb.AddForce(force, ForceMode.Force);


        // キーの入力がない場合、速度を落とす
        Vector3 velocity = rb.linearVelocity;
        // 決めた最大速度を超えた場合、その方向の速度は変化しない
        
        if (Input.GetAxisRaw("Horizontal") == 0 && Mathf.Abs(Vector3.Dot(playerFollowerTransform.right, rb.linearVelocity)) < nowMaxMoveSpeed)
        {
            velocity.x = Mathf.Lerp(velocity.x, 0, stopTime * Time.deltaTime);
        }

        if (Input.GetAxisRaw("Vertical") == 0 && Mathf.Abs(Vector3.Dot(playerFollowerTransform.forward, rb.linearVelocity)) < nowMaxMoveSpeed)
        {
            velocity.z = Mathf.Lerp(velocity.z, 0, stopTime * Time.deltaTime);
        }

        if (velocity.magnitude > nowMaxMoveSpeed)
        {
            velocity = velocity.normalized * nowMaxMoveSpeed;
        }

        rb.linearVelocity = velocity;
    }

    void AddVelocity()
    {
        Ray ray = new Ray(playerFollowerTransform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, 0.5f, groundLayers))
        {
            I_VelocityAdder velocityAdder = hit.collider.gameObject.GetComponent<I_VelocityAdder>();
            if (velocityAdder != null)
            {
                rb.linearVelocity += velocityAdder.GetAddSpeed(transform.position);
            }
        }
    }

    void AdjustMuntiVelocity()
    {
        if (MuntiSpeedTime > 0)
        {
            MuntiSpeedTime -= Time.deltaTime;
            if (MuntiSpeedTime <= 0)
            {
                MuntiSpeed = 1;

                // 速度を元に戻す
                Vector3 velocity = rb.linearVelocity;

                velocity.x = Mathf.Clamp(velocity.x, -maxMoveSpeed, maxMoveSpeed);
                velocity.z = Mathf.Clamp(velocity.z, -maxMoveSpeed, maxMoveSpeed);
                rb.linearVelocity = velocity;
            }
        }
    }

    void ControllRotation()
    {
        transform.Rotate(Vector3.Dot(Vector3.forward, rb.linearVelocity) * rotateSpeed * Time.deltaTime, 0, -Vector3.Dot(Vector3.right, rb.linearVelocity) * rotateSpeed * Time.deltaTime, Space.World);
    }

    void ControllView()
    {
        Vector3 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity,
            0
            );

        // マウスのx軸の動きを反映
        playerFollowerTransform.rotation = Quaternion.Euler(playerFollowerTransform.eulerAngles.x,
            playerFollowerTransform.eulerAngles.y + mouseInput.x,
            playerFollowerTransform.eulerAngles.z);
    }

    void Jump()
    {
        if (IsGround() && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(jumpForce, ForceMode.Impulse);
        }
    }

    // 地面についていたらtrue
    public bool IsGround()
    {
        return Physics.Raycast(playerFollowerTransform.position, Vector3.down, 0.75f, groundLayers);
    }

    private void AdjustPlayerFollowerPosition()
    {
        playerFollowerTransform.position = transform.position;
        playerFollowerTransform.eulerAngles = new Vector3(
            0,
            playerFollowerTransform.eulerAngles.y,
            0
            );
    }

    public void Bound(Vector3 collisionPosition, float bounseStrength)
    {
        Vector3 boundVector = (transform.position - collisionPosition).normalized * bounseStrength;
        rb.AddForce(boundVector, ForceMode.Impulse);
    }
}
