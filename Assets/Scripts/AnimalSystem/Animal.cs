using Codice.Client.BaseCommands;
using UnityEngine;
using Universe.Terrain;

namespace Universe.Animals
{
    public abstract class Animal : MonoBehaviour
    {
        public AnimalSpecies SpeciesInfo;

        public float SpawnChance = 100;
        public float Health;
        public float Energy;
        public float Eyesight;
        public float Intimidation;
        public float reactionTime = 0;
        public float ego = 0;

        public Leg[] legs;
        public Eye[] Eyes;
        public Torso torso;

        private State _state;
        public State AnimalState
        {
            get => _state;
            set
            {
                State oldState = _state;
                _state = value;
                StateChange(oldState);
            }
        }

        private WalkType walkType;
        private float timeOfLastWalktype;

        public float direction;
        private float timeOfLastThink;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private Transform footPosition;

        private void Update()
        {
            if (Time.time - timeOfLastThink < reactionTime)
                return;

            timeOfLastThink = Time.time;
            Think();

            if (Energy < 0 || IsUpsidedown())
                Die();
        }

        private void FixedUpdate()
        {
            if (AnimalState == State.Dead)
                return;

            Walk();

            if (IsSteep())
                TryChangeWalkType(WalkType.Running);
            else if (AnimalState == State.Foraging || AnimalState == State.Idle && Time.time - timeOfLastWalktype > 10)
                TryChangeWalkType(WalkType.Walking);

            if (GlobalTime.IsImportantFrame)
            {
                if (!IsInBounds())
                    Destroy(gameObject);
            }    
        }

        public abstract void Init(AnimalSpecies species, System.Random rand);

        private void Walk()
        {
            float Speed = GetSpeed();

            if (SpeciesInfo.Legs.Length == 1)
            {
                if (IsOnGround())
                    rb.velocity = new Vector2(direction * Speed, 0);
                return;
            }

#if UNITY_EDITOR
            Debug.DrawLine(transform.position, transform.position + transform.right);
#endif
            Vector3 movement = transform.right;
            movement.x = Mathf.Max(0, movement.x);

            transform.position += direction * Speed * Time.fixedDeltaTime * movement;
            Energy -= SpeciesInfo.neutralEnergyCost * Speed * Time.fixedDeltaTime;
        }

        protected virtual bool IsInBounds()
        {
            float xMin = Spawner.Instance.GetTopLeft(CameraControl.Instance.CameraBounds).x;
            float xMax = Spawner.Instance.GetBottomRight(CameraControl.Instance.CameraBounds).x;
            return transform.position.x > xMin && transform.position.x < xMax;
        }

        private float GetSpeed()
        {
            float speed = SpeciesInfo.walkSpeed;
            switch (walkType)
            {
                case WalkType.Walking:
                    break;
                case WalkType.Running:
                    speed *= SpeciesInfo.Run;
                    break;
                case WalkType.Sprinting:
                    speed *= SpeciesInfo.Sprint;
                    break;
                default:
                    break;
            }
            return speed;
        }

        private bool IsOnGround()
        {
            var ray = Physics2D.Raycast(footPosition.position, Vector2.down, .2f, int.MaxValue ^ 64);
#if UNITY_EDITOR
            Debug.DrawRay(footPosition.position, Vector2.down * .2f, Color.red);
#endif
            return ray.transform != null;
        }

        public virtual void Think()
        {
            switch (AnimalState)
            {
                case State.Idle:
                    break;
                case State.Foraging:
                    LookForFood();
                    break;
                case State.Attacking:
                    break;
                case State.Chasing:
                    break;
                case State.Fleeing:
                    break;
                default:
                    break;
            }
        }

        public virtual void LookForFood()
        {

        }

        public virtual void Die()
        {
            AnimalState = State.Dead;
        }

        private void ChangeDirection()
        {
            direction = Random.Range(0, 2) * 2 - 1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * direction;
            transform.localScale = scale;
        }

        public virtual void StateChange(State oldState)
        {
            switch (AnimalState)
            {
                case State.Idle:
                case State.Foraging:
                    TryChangeWalkType(WalkType.Walking);

                    if (oldState == State.Fleeing)
                        break; // don't change direction
                    ChangeDirection();

                    break;
                case State.Attacking:
                    TryChangeWalkType(WalkType.Running);
                    break;
                case State.Chasing:
                    TryChangeWalkType(WalkType.Sprinting);
                    break;
                case State.Fleeing:
                    TryChangeWalkType(WalkType.Sprinting);
                    break;
                default:
                    break;
            }
        }

        protected void GetEyesight()
        {
            if (Eyes == null)
                return;
            for (int i = 0; i < Eyes.Length; i++)
                Eyesight += Eyes[i].Strength;
        }

        private void TryChangeWalkType(WalkType target)
        {
            timeOfLastWalktype = Time.time;
            if (target == WalkType.Sprinting)
            {
                float timeSprintingPerEnergy = 1 / (SpeciesInfo.walkSpeed * SpeciesInfo.Sprint * SpeciesInfo.neutralEnergyCost);

                if (timeSprintingPerEnergy * Energy > 15)
                {
                    walkType = WalkType.Sprinting;
                    return;
                }

                target = WalkType.Running; // hmm, maybe try running
            }

            if (target == WalkType.Running)
            {
                float timeRunningPerEnergy = 1 / (SpeciesInfo.walkSpeed * SpeciesInfo.Run * SpeciesInfo.neutralEnergyCost);

                if (timeRunningPerEnergy * Energy > 30)
                {
                    walkType = WalkType.Running;
                    return;
                }

                // don't check if it can. Its the slowest.
                walkType = WalkType.Walking;
                return;
            }

            // should only check if it should STOP running lol
            if (target == WalkType.Walking)
            {
                if (IsSteep())
                    return;
                walkType = WalkType.Walking;
            }
        }

        protected virtual bool IsSteep()
        {
            if (direction > 0)
                return transform.rotation.eulerAngles.z > 15;
            return transform.rotation.eulerAngles.z < 15;
        }

        protected virtual bool IsUpsidedown()
        {
            if (direction > 0)
                return transform.rotation.eulerAngles.z > 60 && transform.rotation.eulerAngles.z < 300;
            name = transform.rotation.eulerAngles.z.ToString();
            return transform.rotation.eulerAngles.z < 220 && transform.rotation.eulerAngles.z > 90; // 300 +    60
        }
    }

    public enum State
    {
        Idle,
        Foraging,
        Attacking,
        Chasing,
        Fleeing,
        Dead,
    }

    enum WalkType
    {
        Walking,
        Running,
        Sprinting,
    }
}
