using UnityEngine;
using Cinemachine;
using DG.Tweening;

public enum PlayerState { walking, crouching, sprinting, still };

public class PlayerController : MonoBehaviour
{
    [Range(0,10)]       public float speed;
    [Range(0,10)]       public float sprintSpeed;
    [Range(0, 5)]       public float crouchSpeed;
    [Range(0, 1)]       public float rotationSpeed;
    [Range(0, 30)]      public float scrollSpeed;

    public static PlayerController player;

    public CinemachineVirtualCamera vCam;
    public PlayerState currentState;
    public AudioClip walk, sprint, crouch;
    public AudioSource camSound;
    public bool canMove;

    private CinemachinePOV vCamPOV;
    private Rigidbody myRigidbody;
    private Vector3 motion;
    private float mouseScroll;
    private Quaternion rotation;
    private AudioSource footing;

    private float hrzMov, vrtMov;
    private float originalSpeed;
    private float standPos, crouchPos;

    private void Awake()
    {
        if (player == null)
            player = this;

        canMove = false;
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        footing = GetComponent<AudioSource>();
        vCamPOV = vCam.GetCinemachineComponent<CinemachinePOV>();

        canMove = false;
        Cursor.visible = false;
        currentState = PlayerState.still;
        originalSpeed = speed;
        standPos = transform.position.y;
        crouchPos = standPos - .8f;
    }

    void FixedUpdate()
    {
            Movement();
    }

    void Movement()
    {
        if (canMove)
        {
            hrzMov = Input.GetAxisRaw("Horizontal");
            vrtMov = Input.GetAxisRaw("Vertical");
            mouseScroll = Input.mouseScrollDelta.y;
        }
        else
        {
            hrzMov = 0;
            vrtMov = 0;
            mouseScroll = 0;
        }

        if (Input.GetButton("Run") & vrtMov == 1)
        { Sprint(); }
        else if (Input.GetButton("Crouch"))
        { Crouch(); }
        else if (vrtMov != 0 || hrzMov != 0)
        { Walk();   }
        else
        { Still();  }

        motion = new Vector3(hrzMov,0,vrtMov).normalized * speed * Time.deltaTime;
        motion = transform.TransformPoint(motion);
        
        myRigidbody.MoveRotation(vCam.transform.rotation);
        myRigidbody.MovePosition(new Vector3(motion.x, 0, motion.z));

        if(mouseScroll != 0)
        {
            float fov = vCam.m_Lens.FieldOfView;
            vCam.m_Lens.FieldOfView = Mathf.Clamp(fov - mouseScroll * scrollSpeed * Time.deltaTime, 20, 40);
            if(!camSound.isPlaying)
                camSound.Play();
        }
    }
    
    void Walk()
    {
        if (!currentState.Equals(PlayerState.walking))
        {
            speed = originalSpeed;
            DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, 40, .5f);
            transform.position = new Vector3(motion.x, standPos, motion.z);
            currentState = PlayerState.walking;
            FootingSFX(currentState);
        }
    }

    void Crouch()
    {
        if (!currentState.Equals(PlayerState.crouching))
        {
            speed = crouchSpeed;
            transform.position = new Vector3(motion.x, crouchPos, motion.z);
            currentState = PlayerState.crouching;
            FootingSFX(currentState);
        }
    }
        
    void Sprint()
    {
        Camera.main.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        if (currentState.Equals(PlayerState.walking))
        {
            speed = sprintSpeed;
            DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, 50, .5f);
            currentState = PlayerState.sprinting;
            FootingSFX(currentState);
        }
    }

    void Still()
    {
        if (!currentState.Equals(PlayerState.still))
        {
            DOTween.To(() => vCam.m_Lens.FieldOfView, x => vCam.m_Lens.FieldOfView = x, 40, .5f);
            currentState = PlayerState.still;
            FootingSFX(currentState);
        }
    }

    void FootingSFX(PlayerState state)
    {
        if (state.Equals(PlayerState.still))             { footing.DOFade(0, 0.1f);  }
        else if (state.Equals(PlayerState.walking))      { footing.clip = walk;      }
        else if (state.Equals(PlayerState.sprinting))    { footing.clip = sprint;    }
        else if (state.Equals(PlayerState.crouching)
            &  (!hrzMov.Equals(0) || !vrtMov.Equals(0))) { footing.clip = crouch;    }

        if (!state.Equals(PlayerState.still))
        {
            AudioManager.manager.UseSound(footing);
            footing.DOFade(0, 0.3f).OnComplete(() => footing.DOFade(1, 0.3f));
        }
        else
            AudioManager.manager.RemoveSound(footing);
    }

    public void SetCanMove(bool move)
    {
        if (move)
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                        RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        }
        else
        {
            myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        canMove = move;
    }

    public CinemachinePOV getPOV() { return vCamPOV; }
    public float Distance(GameObject obj) { return Vector3.Distance(obj.transform.position, transform.position); }
}