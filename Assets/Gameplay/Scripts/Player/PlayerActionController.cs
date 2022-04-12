using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum Weapon
{
    UNARMED = 0,
    TWOHANDSWORD = 1,
}

public enum AttackSide
{
    NO_SIDE = 0,
    LEFT = 1,
    RIGHT = 2,
    DUAL = 3
}

public class PlayerActionController : MonoBehaviour
{
    [HideInInspector]
    //which invoke when player run
    public UnityEvent OnRun = new UnityEvent();
    [HideInInspector]
    //which invoke when player cancel run
    public UnityEvent OnRunCancel = new UnityEvent();

    PlayerActionControl actionControl;
    GUIPlayer player;

    [HideInInspector]
    public UnityEvent<Vector3> OnMove = new UnityEvent<Vector3>();
    [HideInInspector]
    public UnityEvent PlayerDead = new UnityEvent();
    public Transform PlayerTr;
    public Animator PlayerAnim;
    public Rigidbody PlayerRb;
    public float knockbackMultiplier = 1f;

    [Range(2.0f, 6.0f)]
    public float DefaultSpeed;
    [SerializeField]
    [Range(0f, 1.0f)]
    float gaspValue;

    [SerializeField]
    private float playerSpeed;

    int PlayerJumpHeight = 1;
    public Weapon weapon;

    [SerializeField]
    bool isRun = false;

    private void Awake()
    {
        playerSpeed = 0;

        actionControl = new PlayerActionControl();

        SettingInputs();

        PlayerDead.AddListener(Death);
        weapon = Weapon.UNARMED;
        PlayerAnim.SetInteger("Weapon", 0);
        PlayerAnim.SetInteger("WeaponSwitch", -1);
        UnLock(true, true);
    }

    void SettingInputs()
    {
        actionControl.RPGCharacter.Move.performed += Move_performed;
        actionControl.RPGCharacter.Move.canceled += Move_canceled;
        actionControl.RPGCharacter.Jump.performed += Jump_performed;
        actionControl.RPGCharacter.AttackL.performed += AttackL_performed;
        actionControl.RPGCharacter.AttackR.performed += AttackR_performed;
        actionControl.RPGCharacter.Mouse.performed += Mouse_performed;
    }

    [SerializeField]
    Vector2 RotateInput;
    Vector3 rot;
    public float rotSpeed = 3.0f;
    private void Mouse_performed(InputAction.CallbackContext obj)
    {
        RotateInput = actionControl.RPGCharacter.Mouse.ReadValue<Vector2>();
        rot.y = RotateInput.x;
        //rot.x = RotateInput.y;
        PlayerTr.eulerAngles += rot * Time.deltaTime * DefaultSpeed * rotSpeed;
    }

    private void AttackL_performed(InputAction.CallbackContext obj)
    {
        if (canAction && canMove)
        {
            Attack((int)AttackSide.LEFT);
        }
    }

    private void AttackR_performed(InputAction.CallbackContext obj)
    {
        if (canAction && canMove)
        {
            Attack((int)AttackSide.RIGHT);
        }
    }

    bool isJump = false;
    private void Jump_performed(InputAction.CallbackContext obj)
    {
        isJump = true;
        PlayerAnim.SetInteger("Jumping", PlayerJumpHeight);
        PlayerAnim.SetBool("JumpTrigger", true);
        Lock(false, true, true, 0, PlayerJumpHeight * 0.1f);
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        canMove = false;
        isIncrease = false;
        isDecrease = false;
        StartCoroutine(DecreaseSpeed(0, false));
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        canMove = true;
        PlayerAnim.SetBool("Moving", canMove);
        isDecrease = false;
        StartCoroutine(IncreaseSpeed(DefaultSpeed));
    }

    private void OnEnable()
    {
        actionControl.Enable();
    }

    private void OnDisable()
    {
        actionControl.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GUIManager.Inst.Get<GUIPlaying>().Player;
    }

    [SerializeField]
    Vector2 movementInput;
    private bool isDead;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            SetSpeed(actionControl.RPGCharacter.SpeedUp.ReadValue<float>() == 1 ? true : false);
            movementInput = actionControl.RPGCharacter.Move.ReadValue<Vector2>();
            Move(movementInput);
        }
        if (isJump && canMove && canAction)
        {
            PlayerAnim.SetInteger("Jumping", 0);
            isJump = false;
        }
    }

    void SetSpeed(bool run)
    {
        if (player.StaminaValue <= gaspValue)
        {
            isIncrease = false;
            if (!isDecrease)
            {
                StartCoroutine(DecreaseSpeed(DefaultSpeed * 0.5f));
            }
        }
        else
        {
            if (run != isRun)
            {
                isRun = run;
                if (isRun)
                {
                    isDecrease = false;
                    if (!isIncrease)
                    {
                        StartCoroutine(IncreaseSpeed(DefaultSpeed * 2));
                        OnRun.Invoke();
                    }
                }
                else
                {
                    isIncrease = false;
                    if (!isDecrease)
                    {
                        StartCoroutine(DecreaseSpeed(DefaultSpeed));
                        OnRunCancel.Invoke();
                    }
                }
            }
        }
    }

    bool isIncrease;
    IEnumerator IncreaseSpeed(float Speed)
    {
        isIncrease = true;
        while (playerSpeed < Speed && isIncrease)
        {
            playerSpeed += Time.deltaTime * DefaultSpeed;

            yield return null;
        }
        isIncrease = false;
    }

    [SerializeField]
    bool isDecrease;
    IEnumerator DecreaseSpeed(float Speed, bool isKeyInput=true)
    {
        isDecrease = true;
        while (playerSpeed >= Speed && isDecrease)
        {
            playerSpeed -= Time.deltaTime;
            if (!isKeyInput)
            {
                Move(Vector2.up);
            }
            yield return null;
        }
        if (isDecrease)
        {
            //OnRunCancel.Invoke();
            if (!isKeyInput)
            {
                canMove = false;
                PlayerAnim.SetBool("Moving", canMove);
            }
        }

        //if (playerSpeed < DefaultSpeed)
        //{
        //    yield return new WaitForSeconds(1f);
        //    yield return StartCoroutine(IncreaseSpeed(DefaultSpeed));
        //}
        isDecrease = false;
    }
    [SerializeField]
    Vector3 pos;
    void Move(Vector2 inputPos)
    {
        //pos = PlayerTr.forward * movementInput.y * Time.deltaTime * playerSpeed;
        //rot = Vector3.up * movementInput.x * Time.deltaTime * playerSpeed * 10;

        //if (pos != PlayerTr.localPosition)
        //{
        //    PlayerTr.localPosition += pos;
        //    OnMove.Invoke(PlayerTr.localPosition);
        //}
        //PlayerTr.eulerAngles += rot;

        pos = PlayerTr.forward * inputPos.y * Time.deltaTime * playerSpeed;
        pos += PlayerTr.right * inputPos.x * Time.deltaTime * playerSpeed;
        if (pos != PlayerTr.localPosition)
        {
            PlayerTr.localPosition += pos;
            OnMove.Invoke(PlayerTr.localPosition);
        }
        PlayerAnim.SetFloat("Velocity Z", playerSpeed);
    }


    /// <summary>
    /// when Death
    /// </summary>
    void Death()
    {
        PlayerAnim.SetTrigger("DeathTrigger");
        Lock(true, true, false, 0.1f, 0f);
        isDead = true;
    }

    void Attack(int attackSide)
    {
        int attackNumber = 0;
        //Unarmed.
        if (weapon == Weapon.UNARMED)
        {
            int maxAttacks = 3;
            //Left attacks.
            if (attackSide == 1)
            {
                do
                {
                    attackNumber = Random.Range(1, maxAttacks + 1);
                } while (attackNumber != 2);

            }
            //Right attacks.
            else if (attackSide == 2)
            {
                do
                {
                    attackNumber = Random.Range(4, maxAttacks + 4);
                } while (attackNumber != 6);
            }
            //Set the Locks.
            if (attackSide != 3)
            {
                Lock(true, true, true, 0, 1f);
            }
        }
        else if (weapon == Weapon.TWOHANDSWORD)
        {
            int maxAttacks = 11;
            attackNumber = Random.Range(1, maxAttacks);
            Lock(true, true, true, 0, 1.1f);
        }
        else
        {
            int maxAttacks = 6;
            attackNumber = Random.Range(1, maxAttacks);
            if (weapon == Weapon.TWOHANDSWORD)
            {
                Lock(true, true, true, 0, 0.85f);
            }
            else
            {
                Lock(true, true, true, 0, 0.75f);
            }
        }
        //Trigger the animation.
        PlayerAnim.SetInteger("Action", attackNumber);
        if (attackSide == 3)
        {
            PlayerAnim.SetTrigger("AttackDualTrigger");
        }
        else
        {
            PlayerAnim.SetTrigger("AttackTrigger");
        }
    }

    /// <summary>
    /// Lock character movement and/or action, on a delay for a set time.
    /// </summary>
    /// <param name="lockMovement">If set to <c>true</c> lock movement.</param>
    /// <param name="lockAction">If set to <c>true</c> lock action.</param>
    /// <param name="timed">If set to <c>true</c> timed.</param>
    /// <param name="delayTime">Delay time.</param>
    /// <param name="lockTime">Lock time.</param>
    public void Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
    {
        StopCoroutine("_Lock");
        StartCoroutine(_Lock(lockMovement, lockAction, timed, delayTime, lockTime));
    }

    bool canAction = true;
    /// <summary>
    /// Keep character from doing actions.
    /// </summary>
    private void LockAction()
    {
        canAction = false;
    }

    //Timed -1 = infinite, 0 = no, 1 = yes.
    public IEnumerator _Lock(bool lockMovement, bool lockAction, bool timed, float delayTime, float lockTime)
    {
        if (delayTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
        }
        if (lockMovement)
        {
            LockMovement();
        }
        if (lockAction)
        {
            LockAction();
        }
        if (timed)
        {
            if (lockTime > 0)
            {
                yield return new WaitForSeconds(lockTime);
            }
            UnLock(lockMovement, lockAction);
        }
    }

    /// <summary>
    /// Let character move and act again.
    /// </summary>
    private void UnLock(bool movement, bool actions)
    {
        StartCoroutine(_ResetIdleTimer());
        if (movement)
        {
            UnlockMovement();
        }
        if (actions)
        {
            canAction = true;
        }
    }

    private float idleTimer;
    private float idleTrigger = 0f;

    private IEnumerator _ResetIdleTimer()
    {
        idleTrigger = Random.Range(5f, 15f);
        idleTimer = 0;
        yield return new WaitForSeconds(1f);
        PlayerAnim.ResetTrigger("IdleTrigger");
    }

    [SerializeField]
    private bool canMove;
    //Keep character from moving.
    public void LockMovement()
    {
        canMove = false;
        PlayerAnim.SetBool("Moving", canMove);
        PlayerAnim.applyRootMotion = true;
        //currentVelocity = new Vector3(0, 0, 0);
    }

    //Allow character movement.
    public void UnlockMovement()
    {
        canMove = true;
        PlayerAnim.SetBool("Moving", canMove);
        PlayerAnim.applyRootMotion = false;
    }

    /// <summary>
    /// when get hit
    /// </summary>
    public void GetHit()
    {
        int hits = 5;
        int hitNumber = Random.Range(1, hits + 1);
        PlayerAnim.SetInteger("Action", hitNumber);
        PlayerAnim.SetTrigger("GetHitTrigger");
        Lock(true, true, true, 0.1f, 0.4f);
        //Apply directional knockback force.
        if (hitNumber <= 1)
        {
            StartCoroutine(_Knockback(-PlayerTr.forward, 8, 4));
        }
        else if (hitNumber == 2)
        {
            StartCoroutine(_Knockback(PlayerTr.forward, 8, 4));
        }
        else if (hitNumber == 3)
        {
            StartCoroutine(_Knockback(PlayerTr.right, 8, 4));
        }
        else if (hitNumber == 4)
        {
            StartCoroutine(_Knockback(-PlayerTr.right, 8, 4));
        }
    }

    bool isKnockback;
    public IEnumerator _Knockback(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        isKnockback = true;
        StartCoroutine(_KnockbackForce(knockDirection, knockBackAmount, variableAmount));
        yield return new WaitForSeconds(.1f);
        isKnockback = false;
    }

    private IEnumerator _KnockbackForce(Vector3 knockDirection, int knockBackAmount, int variableAmount)
    {
        while (isKnockback)
        {
            PlayerRb.AddForce(knockDirection * ((knockBackAmount + Random.Range(-variableAmount, variableAmount)) * (knockbackMultiplier * 10)), ForceMode.Impulse);
            yield return null;
        }
    }

}
