using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MarbleController : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    //�{�[���𓮂������x
    [SerializeField] float addMoveSpeed;
    public float nowAddMoveSpeed;
    //�{�[���̍ő呬�x
    [SerializeField] float maxMoveSpeed;
    public float nowMaxMoveSpeed;
    //�{�[�����~�܂�܂łɂ����鎞��
    [SerializeField] float stopTime;

    //�{�[���̔{���x
    public float MuntiSpeed { set; get; }
    public float MuntiSpeedTime { set; get; }

    //�{�[���̗͂������ꏊ
    [SerializeField] Vector3 forcePoint;
    //�{�[����]���x
    [SerializeField] float rotateSpeed;
    //�}�E�X���x
    [SerializeField] float mouseSensitivity;

    //�n�ʃ��C���[
    [SerializeField] LayerMask groundLayers;
    //�W�����v����ۂɉ����͂̑傫��
    [SerializeField] Vector3 jumpForce;

    //�v���C���[�t�H�����[
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
                //�{�[������L�[�œ�����
                Move();

                //��ɂ���ă{�[���̑��x��������
                AddVelocity();

                //�X�s�[�h��������
                AdjustMuntiVelocity();

                //���x�ɂ���ĉ�]�����𒲐�
                ControllRotation();

                //�X�y�[�X�L�[�ŃW�����v����
                Jump();
            }
            

            //���_���}�E�X�ő��삷��
            ControllView();

            //�v���C���[�t�H�����[�𒲐�
            AdjustPlayerFollowerPosition();
        }
    }

    private void Move()
    {
        //�܂��{�[���ɗ͂�������
        Vector3 moveDir = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical") );


        nowAddMoveSpeed = addMoveSpeed * MuntiSpeed;
        nowMaxMoveSpeed = maxMoveSpeed * MuntiSpeed;

        //������̑��x�������𒴂����ꍇ�A���̕����ɗ͉͂����Ȃ�
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


        //�L�[�̓��͂��Ȃ��ꍇ�A���x�𗎂Ƃ�
        Vector3 velocity = rb.linearVelocity;
        //������̑��x�������𒴂����ꍇ�A���̕�������͕ω����Ȃ�
        
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

                //���x�����Ƃ�
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
        //�ϐ��̃��[�U�[�̃}�E�X�̓������i�[
        Vector3 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity,
            0
            );

        //�}�E�X��x���̓����𔽉f
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

    //�n�ʂɂ��Ă����true
    public bool IsGround()
    {
        //���肵��bool�l��Ԃ�
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
