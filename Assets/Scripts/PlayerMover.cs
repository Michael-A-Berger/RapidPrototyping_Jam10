using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    // Public Properties
    [Header("General Variables")]
    public PlayerAnim animScript;
    public GameObject wandPrefab;
    public bool debug = false;
    public GameObject debugPrefab1;
    public GameObject debugPrefab2;
    [Header("Movement Settings")]
    [Space(10)]
    public List<PlayerMovementSettings> movementVars = new List<PlayerMovementSettings>();

    // Private Properties
    private PlayerMovementSettings currentMoveVars;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsuleCollider;
    private BoxCollider2D boxTrigger;
    private Vector2 lastVelocity;
    private Vector3 slopeAxis = Vector3.right;
    private float runTimeStart = 0f;
    private float stopTimeStart = 0f;
    private float decelTime = 0f;
    private float lastAxisX = 0f;
    private float lastAxisY = 0f;
    private float lastFire1 = 0f;
    private uint jumpCounter = 0;
    private GameObject spawnedWand;
    private StaffTrigger wandScript;
    private Vector3 wandSpawnLocation = new Vector3(1, 0, 0);
    private bool grounded = false;

    /// <summary>
    /// Start()
    /// </summary>
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxTrigger = GetComponent<BoxCollider2D>();

        // Setting the movement variables
        ResetMovementVars();
    }

    /// <summary>
    /// ResetMovementVars()
    /// </summary>
    private void ResetMovementVars()
    {
        // Setting the "Currrent" properties to be the "Normal" values
        currentMoveVars = GetMovementVarsByName("Normal");

        // Setting the rigidbody gravity
        rigid.gravityScale = currentMoveVars.gravity;
    }

    /// <summary>
    /// GetMovementVarsByName()
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public PlayerMovementSettings GetMovementVarsByName(string name)
    {
        PlayerMovementSettings result = null;
        for (int num = 0; num < movementVars.Count; num++)
        {
            if (movementVars[num].name == name)
            {
                result = movementVars[num];
                num = movementVars.Count;
            }
        }
        return result;
    }

    /// <summary>
    /// Update()
    /// </summary>
    void Update()
    {
        // Getting the input values
        float axisX = Input.GetAxisRaw("Horizontal");
        float axisY = Input.GetAxisRaw("Vertical");
        float fire1 = Input.GetAxisRaw("Fire1");

        // IF there is new running input...
        if (axisX != 0f && axisX != lastAxisX)
        {
            runTimeStart = Time.time;
        }

        // IF there is new stopping input...
        if (axisX == 0f && axisX != lastAxisX)
        {
            stopTimeStart = Time.time;
        }

        // Calculating the deceleration multiplier
        decelTime = (grounded) ? currentMoveVars.groundedTimeToStop : currentMoveVars.airborneTimeToStop;

        // Running
        if (axisX != 0f)
        {
            // Calculating the add velocity factor
            float addVelocityX = axisX * currentMoveVars.maxRunSpeed * (Time.deltaTime / currentMoveVars.timeToMaxSpeed);

            // Creating the move vector
            Vector2 moveVector = new Vector2();

            // IF the player is grounded, follow the slope axis, otherwise do NOT do that
            if (grounded)
                moveVector = new Vector2(slopeAxis.x * addVelocityX, slopeAxis.y * addVelocityX);
            else
                moveVector = new Vector2(addVelocityX, 0f);

            // IF the velocity and move vectors are different OR the new velocity does not exceed the max velocity...
            if (rigid.velocity.x / Mathf.Abs(rigid.velocity.x) != axisX || (rigid.velocity + moveVector).magnitude < currentMoveVars.maxRunSpeed)
            {
                rigid.velocity += moveVector;
            }
            // ELSE IF the current velocity is less than the max...
            else if (rigid.velocity.magnitude < currentMoveVars.maxRunSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * currentMoveVars.maxRunSpeed;
            }
        }

        // IF the player is not holding down a button (but is still moving)...
        if (axisX == 0f && rigid.velocity.x != 0f)
        {
            // IF the player is grounded, decelerate along the slope axis
            if (grounded)
            {
                // Calculate the deceleration vector
                Vector2 decelVector = (Time.deltaTime / decelTime) * rigid.velocity.normalized * currentMoveVars.maxRunSpeed;

                // IF the deceleration vector is LESS than the current velocity...
                if (decelVector.magnitude < rigid.velocity.magnitude)
                    rigid.velocity -= decelVector;
                else
                    rigid.velocity = new Vector2(0f, 0f);
            }
            // ELSE... (decelerate exclusively on the X axis)
            else
            {
                // Calulate the deceleration vector X value
                float decelX = (Time.deltaTime / decelTime) * (rigid.velocity.x / Mathf.Abs(rigid.velocity.x)) * currentMoveVars.maxRunSpeed;

                // IF the deceleration vector X value is LESS than the current X velocity...
                if (Mathf.Abs(decelX) < Mathf.Abs(rigid.velocity.x))
                    rigid.velocity -= new Vector2(decelX, 0f);
                else
                    rigid.velocity = new Vector2(0f, rigid.velocity.y);
            }
        }

        // Jumping
        if (axisY > 0f && axisY != lastAxisY && (jumpCounter < currentMoveVars.midairJumps + 1))
        {
            // rigid.velocity = new Vector2(rigid.velocity.x, currentJumpVelocity);
            rigid.velocity = new Vector2(rigid.velocity.x, currentMoveVars.jumpVelocity);
            grounded = false;
            jumpCounter++;
        }
        if (axisY == 0f && axisY != lastAxisY && !grounded && rigid.velocity.y > 0f)
        {
            rigid.velocity *= new Vector2(1f, 0.5f);
        }

        // Setting the Wand spawn location
        if (axisX != 0 || axisY != 0)
        {
            wandSpawnLocation = new Vector2(axisX, axisY);
            wandSpawnLocation.Normalize();
        }

        // Waving The Wand
        if (fire1 > 0f && fire1 != lastFire1)
        {
            // Spawning the Wand
            spawnedWand = Instantiate(wandPrefab, transform.position + wandSpawnLocation, Quaternion.identity);
            wandScript = spawnedWand.GetComponent<StaffTrigger>();
        }
        if (fire1 == 0f && fire1 != lastFire1)
        {
            Destroy(spawnedWand, 0f);
        }

        // IF the wand has been spawned...
        if (spawnedWand != null)
        {
            // Moving the Wand to the right place
            spawnedWand.transform.position = transform.position + wandSpawnLocation;

            // Rotating the Wand to the right rotation
            float rotation = Vector3.SignedAngle(Vector3.right, wandSpawnLocation, Vector3.forward);
            spawnedWand.transform.rotation = Quaternion.identity;
            spawnedWand.transform.Rotate(Vector3.forward, rotation);

            // IF the Wand has been hooked...
            if (wandScript.IsHooked())
            {
                Vector2 modVector = wandSpawnLocation * currentMoveVars.hookSpeed * Time.deltaTime * currentMoveVars.maxRunSpeed;
                rigid.velocity += modVector;
            }
        }

        // ANIMATING
        if (axisX > 0f)
            animScript.MoveRight();
        else if (axisX < 0f)
            animScript.MoveLeft();
        else
            animScript.StandStill();

        // Setting the "Last Run Time" variables
        lastAxisX = axisX;
        lastAxisY = axisY;
        lastFire1 = fire1;
        lastVelocity = rigid.velocity;
    }

    /// <summary>
    /// GroundPlayer()
    /// </summary>
    private void GroundPlayer(Vector3 closestPoint)
    {
        // Telling the player it has been grounded
        grounded = true;

        // Resetting the jump counter
        jumpCounter = 0;

        // Getting the new slope axis
        slopeAxis = transform.position - closestPoint;
        slopeAxis = new Vector3(slopeAxis.y, -slopeAxis.x, 0f);
        slopeAxis.Normalize();

        // Disable gravity on the rigidbody
        rigid.gravityScale = 0f;
    }

    /// <summary>
    /// OnTriggerEnter()
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        // IF the other collider is a Platform...
        if (other.gameObject.tag == "Platform")
        {
            // Casting to the correct collider type
            BoxCollider2D otherBox = (BoxCollider2D) other;

            // Get the closest point to the player
            Vector3 closestPoint = otherBox.ClosestPoint(transform.position);

            // IF the closest point is on the ground...
            if (closestPoint.y <= transform.position.y - boxTrigger.bounds.extents.y / 2)
                GroundPlayer(closestPoint);

            if (debug)
            {
                if (grounded)
                {
                    Instantiate(debugPrefab1, closestPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(debugPrefab2, closestPoint, Quaternion.identity);
                }
            }
        }

        // IF the other collider is Water...
        if (other.gameObject.tag == "Water")
        {
            // Cut velocity by 20%
            rigid.velocity *= 0.8f;

            // Setting the water movement variables
            currentMoveVars = GetMovementVarsByName("Water");
        }
    }

    /// <summary>
    /// OnTriggerStay()
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay2D(Collider2D other)
    {
        // IF the other collider is a Platform...
        if (other.gameObject.tag == "Platform")
        {
            // Casting to the correct collider type
            BoxCollider2D otherBox = (BoxCollider2D)other;

            // Get the closest point to the player
            Vector3 closestPoint = otherBox.ClosestPoint(transform.position);

            // IF the closest point is on the ground...
            if (closestPoint.y <= transform.position.y - boxTrigger.bounds.extents.y / 2)
            {
                GroundPlayer(closestPoint);
            }
        }
    }

    /// <summary>
    /// OnTriggerExit2D()
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        // IF the other collider is a Platform...
        if (other.gameObject.tag == "Platform")
        {
            // Enable gravity
            rigid.gravityScale = currentMoveVars.gravity;
            grounded = false;
        }

        // IF the other collider is Water...
        if (other.gameObject.tag == "Water")
        {
            // Resetting the normal movement variables
            ResetMovementVars();

            // Letting the player double-jump once to get out of the water
            jumpCounter = 1;
        }
    }
}

[System.Serializable]
public class PlayerMovementSettings
{
    // Public Properties (Default values are for 
    [Tooltip("What this movement setting is called (also the name to search for in the [GetMovementVarsByName()] function)")]
    public string name = "Normal";
    [Tooltip("How many units does the player move in a second?")]
    public float maxRunSpeed = 10f;
    [Tooltip("How many seconds does it take for the player to reach the max run speed?")]
    public float timeToMaxSpeed = 0.2f;
    [Tooltip("How many seconds does it take for the player, on the ground, to reach a velocity of zero?")]
    public float groundedTimeToStop = 0.2f;
    [Tooltip("How many seconds does it take for the player, in the air, to reach a velocity of zero?")]
    public float airborneTimeToStop = 1.0f;
    [Tooltip("What is the initial Y velocity when the player jumps?")]
    public float jumpVelocity = 15f;
    [Tooltip("How many midair jumps can the player perform?")]
    public uint midairJumps = 1;
    [Tooltip("What is the gravity scale of the Rigidbody?")]
    public float gravity = 3f;
    [Tooltip("What is the speed (velocity / second) of when the Wand is inside of a Lantern?")]
    public float hookSpeed = 20f;
}
