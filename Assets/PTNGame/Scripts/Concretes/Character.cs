using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private CharacterAnimations characterAnimations;
    private Rigidbody rb;

    private Vector3 spawnPos;
    private bool isSpawning;

    protected bool IsFalling;
    protected bool CanMove;
    protected float ObstacleForce = 2000f;
    private float RunAniSpeed => SetRunAniSpeed();

    protected virtual float SetRunAniSpeed() => 0f;

    protected void Init()
    {
        isSpawning = true;
        CanMove = false;

        rb = GetComponent<Rigidbody>();
        characterAnimations = new CharacterAnimations(GetComponent<Animator>());

        SetSpawnPos((int) transform.position.z);
    }

    private void LateUpdate()
    {
        characterAnimations.SetRun(RunAniSpeed);

        if (!IsFalling && transform.position.y < 0)
            IsFalling = true;
        else if (IsFalling && transform.position.y >= 0)
        {
            IsFalling = false;
        }

        if (IsFalling && !isSpawning && transform.position.y < -10f)
            StartCoroutine(StartCharacterSpawning());
    }


    private IEnumerator StartCharacterSpawning()
    {
        isSpawning = true;

        OnCharacterSpawning();

        CanMove = false;
        characterAnimations.SetFalling(true);

        yield return new WaitForSeconds(1f);

        rb.velocity = Vector3.zero;
        transform.position = spawnPos;
    }

    private void SetSpawnPos(int posZ)
    {
        if ((int) spawnPos.z == posZ)
            return;
        spawnPos = new Vector3(0, 25f, posZ);
    }

    // for standing up animation event
    protected internal void StandingAniFinished()
    {
        characterAnimations.SetStandingUp(false);

        StandingUpAniFinished();
    }

    // for falling down animation event
    protected internal void FallingDownAniFinished()
    {
        characterAnimations.SetFallingDown(false);

        characterAnimations.SetStandingUp(true);
    }


    #region Collision

    private void OnCollisionStay(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("RotatingPlatform"))
        {
            OnRotatingPlatform();

            var turnPos = collisionInfo.gameObject.GetComponent<ObstacleMover>().PlayerRotationPos;
            transform.Translate(turnPos * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CanMove && other.gameObject.CompareTag("SpawnPoint"))
        {
            OnSpawnPoint();

            SetSpawnPos((int) other.transform.position.z);
        }

        if (CanMove && other.gameObject.CompareTag("FinishLine"))
        {
            OnFinishLine();

            CanMove = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CanMove &&
            (collision.gameObject.CompareTag("AddForceObstacle") || collision.gameObject.CompareTag("Player")))
        {
            OnAddForceObstacle();

            CanMove = false;
            characterAnimations.SetFallingDown(true);
            rb.AddExplosionForce(ObstacleForce, collision.transform.position, 360, 0.2f);
        }

        if (CanMove && collision.gameObject.CompareTag("SpawnObstacle"))
        {
            OnSpawnObstacle();

            StartCoroutine(StartCharacterSpawning());
        }

        if (isSpawning && !CanMove && collision.gameObject.CompareTag("Platform"))
        {
            isSpawning = false;
            OnPlatform();
            characterAnimations.SetFalling(false);
            characterAnimations.SetFallingDown(false);
            characterAnimations.SetStandingUp(true);
        }
    }

    #endregion

    #region Override Methods

    /// <summary>
    /// This function runs when standing up animation finished
    /// </summary>
    protected virtual void StandingUpAniFinished()
    {
        CanMove = true;
    }

    /// <summary>
    /// This function runs when character is spawning from air
    /// </summary>
    protected virtual void OnCharacterSpawning()
    {
    }

    /// <summary>
    /// This function runs when character is on finish line
    /// </summary>
    protected virtual void OnFinishLine()
    {
    }

    /// <summary>
    /// This function runs when character hit spawn obstacle
    /// </summary>
    protected virtual void OnSpawnObstacle()
    {
    }

    /// <summary>
    /// This function runs when character hit AddForce obstacle
    /// </summary>
    protected virtual void OnAddForceObstacle()
    {
    }

    /// <summary>
    /// This function runs when character hit spawn point(position) object
    /// </summary>
    protected virtual void OnSpawnPoint()
    {
    }

    /// <summary>
    /// This function runs when character hit the platform first time after spawning
    /// </summary>
    protected virtual void OnPlatform()
    {
    }

    /// <summary>
    /// This function runs during character  is on rotating platform
    /// </summary>
    protected virtual void OnRotatingPlatform()
    {
    }

    #endregion
}