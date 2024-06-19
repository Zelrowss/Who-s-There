using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent _agent;
    private Transform _player;
    private Animator _anim;

    [Header("Speed")]
    public float normalSpeed = 3;

    [Header("Patrolling")]
    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    private bool walkPointSet;
    public GameObject[] sightPoints;
    private Vector3 lastKnownPosition;
    private bool isPatrollingLastKnownPosition;
    private float patrolTimer;

    [Header("Attack")]
    public float attackRange;
    public bool playerInAttackRange;
    public float patrolDuration = 60f;
    public float searchRadius = 10f;
    private float speedIncreaseInterval = 30f;
    private float lastSpeedIncreaseTime;

    [Header("Post Processing")]
    private Volume _volume;
    private Vignette vignette;
    private FilmGrain filmGrain;
    private ChromaticAberration chromaticAberration;
    private Bloom bloom;
    private MotionBlur motionBlur;
    private LensDistortion lensDistortion;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            _agent.SetDestination(Vector3.zero);
            transform.LookAt(null);
            print("Je l'ai touché");
        }
    }
 
    private void SearchWalkPoint() {
        int randomIndex = Random.Range(0, sightPoints.Length);
        walkPoint = sightPoints[randomIndex].transform.position;
        walkPointSet = true;
    }

    private void Patroling() {
        if (!walkPointSet) {
            SearchWalkPoint();
        } else {
            _agent.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.sqrMagnitude < 1f * 1f) {
                walkPointSet = false;
            }
        }
    }

    private void PatrolLastKnownPosition() {
        if (!walkPointSet) {
            List<GameObject> nearbySightPoints = new List<GameObject>();
            foreach (GameObject point in sightPoints) {
                if ((point.transform.position - lastKnownPosition).sqrMagnitude <= searchRadius * searchRadius) {
                    nearbySightPoints.Add(point);
                }
            }

            if (nearbySightPoints.Count > 0) {
                int randomIndex = Random.Range(0, nearbySightPoints.Count);
                walkPoint = nearbySightPoints[randomIndex].transform.position;
                walkPointSet = true;
            } else {
                walkPoint = lastKnownPosition + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
                walkPointSet = true;
            }
        } else {
            _agent.SetDestination(walkPoint);
            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.sqrMagnitude < 1f * 1f) {
                walkPointSet = false;
            }
        }

        patrolTimer -= Time.deltaTime;
        if (patrolTimer <= 0) {
            isPatrollingLastKnownPosition = false;
            walkPointSet = false;
        }
    }

    private void AttackPlayer() {
        _agent.SetDestination(_player.transform.position);
        Vector3 lookAtPosition = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.LookAt(lookAtPosition);
    }

    private void AdjustPostProcessingEffects(){
        float distance = Vector3.Distance(transform.position, _player.position);
        float effectIntensity = Mathf.Clamp01((10 - distance) / 10);

        if (vignette != null) {
            vignette.intensity.value = Mathf.Lerp(0.2f, 0.6f, effectIntensity);
        }

        if (filmGrain != null) {
            filmGrain.intensity.value = Mathf.Lerp(0.1f, 0.3f, effectIntensity);
        }

        if (chromaticAberration != null) {
            chromaticAberration.intensity.value = Mathf.Lerp(0.1f, 0.5f, effectIntensity);
        }

        if (bloom != null) {
            bloom.intensity.value = Mathf.Lerp(0.5f, 1.5f, effectIntensity);
        }

        if (motionBlur != null) {
            motionBlur.intensity.value = Mathf.Lerp(0.1f, 0.4f, effectIntensity);
        }

        if (lensDistortion != null) {
            lensDistortion.intensity.value = Mathf.Lerp(0, 0.3f, effectIntensity);
        }

        float pitchIntensity = Mathf.Lerp(1.0f, 2.0f, effectIntensity);
        _player.GetComponentInChildren<PlayerAudioManager>().hearthBeat.volume = effectIntensity;
        _player.GetComponentInChildren<PlayerAudioManager>().hearthBeat.pitch = pitchIntensity;

    }

    void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Start() {
        sightPoints = GameObject.FindGameObjectsWithTag("SightPoint");

        _volume = _player.GetComponent<PlayerManager>()._volume;
        _volume.profile.TryGet<Vignette>(out vignette);
        _volume.profile.TryGet<FilmGrain>(out filmGrain);
        _volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        _volume.profile.TryGet<Bloom>(out bloom);
        _volume.profile.TryGet<MotionBlur>(out motionBlur);
        _volume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    void Update() {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        _anim.SetBool("isWalking", walkPointSet || playerInAttackRange ? true : false);

        if (playerInAttackRange && Time.time - lastSpeedIncreaseTime >= 30f) {
            _agent.speed += 1;
            lastSpeedIncreaseTime = Time.time;
        }

        if (playerInAttackRange) {
            lastKnownPosition = _player.position;
            isPatrollingLastKnownPosition = false;
            AttackPlayer();
        } else if (isPatrollingLastKnownPosition) {
            PatrolLastKnownPosition();
        } else {
            Patroling();
        }

        if (!playerInAttackRange && !isPatrollingLastKnownPosition) {
            isPatrollingLastKnownPosition = true;
            patrolTimer = patrolDuration;
            _agent.speed = normalSpeed;
        }

        AdjustPostProcessingEffects();
    }
}
