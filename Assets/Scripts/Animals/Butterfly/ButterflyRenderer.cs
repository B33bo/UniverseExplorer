using System;
using UnityEngine;

namespace Universe.Animals
{
    public class ButterflyRenderer : AnimalRenderer
    {
        public override Type AnimalType => typeof(Butterfly);

        [SerializeField]
        private SpriteRenderer leftWingPattern, rightWingPattern;

        [SerializeField]
        private SpriteMask leftWingMask, rightWingMask;

        [SerializeField]
        private Animator animator;

        private Vector2 startPos;
        private float speed;
        private const float pushRadius = .7f;
        private const float unpushRadius = .1f;
        private const float dangerZone = 4f;

        public override void Spawn(Vector2 position, int? seed, Animal species)
        {
            startPos = position;
            Butterfly butterfly = new Butterfly();
            Target = butterfly;

            if (seed.HasValue)
                butterfly.SetSeed(seed.Value);

            butterfly.CreateAnimal(species);

            SetPattern(butterfly.leftWingPattern, leftWingMask, leftWingPattern);
            SetPattern(butterfly.rightWingPattern, rightWingMask, rightWingPattern);

            speed = butterfly.speed;
            animator.SetFloat("speed", butterfly.speed);
            transform.localScale *= butterfly.size;
        }

        private float centerPush = 0;
        public override void OnUpdate()
        {
            float rotation = Mathf.Atan2(startPos.y - transform.position.y, startPos.x - transform.position.x);
            Quaternion targetRotation = Quaternion.Euler(0, 0, rotation * Mathf.Rad2Deg);
            Quaternion startRotation = transform.rotation;

            Vector2 pos = transform.position + speed * Time.deltaTime * transform.up;

            if (centerPush != 0)
                pos += Vector2.Lerp(Vector2.zero, startPos - pos, Time.deltaTime) * centerPush;

            pos = PushIfOutOfBounds(pos);

            transform.SetPositionAndRotation(
                pos,
                Quaternion.Lerp(startRotation, targetRotation, Time.deltaTime * speed * 5));
        }

        private Vector2 PushIfOutOfBounds(Vector2 pos)
        {
            if (ShouldPush(pos, out bool changeX))
            {
                if (DangerZone(pos))
                    return startPos;

                if (changeX)
                    pos.x = transform.position.x;
                else
                    pos.y = transform.position.y;
                centerPush = Mathf.Abs(Mathf.Sin(Time.time)) + 1;
            }
            else if (ShouldUnpush(pos))
                centerPush = 0;
            return pos;
        }

        private bool ShouldPush(Vector2 pos, out bool isX)
        {
            isX = pos.x > startPos.x + pushRadius || pos.x < startPos.x - pushRadius;
            return isX ||
                pos.y > startPos.y + pushRadius || pos.y < startPos.y - pushRadius;
        }

        private bool DangerZone(Vector2 pos)
        {
            return pos.x > startPos.x + dangerZone || pos.x < startPos.x - dangerZone ||
                pos.y > startPos.y + dangerZone || pos.y < startPos.y - dangerZone;
        }

        private bool ShouldUnpush(Vector2 pos)
        {
            return pos.x < startPos.x + unpushRadius && pos.x > startPos.x - unpushRadius ||
                pos.y < startPos.y + unpushRadius && pos.y > startPos.y - unpushRadius;
        }
    }
}
