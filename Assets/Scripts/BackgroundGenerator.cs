using UnityEngine;

namespace Universe
{
    public class BackgroundGenerator : MonoBehaviour
    {
        [SerializeField]
        private BackgroundSpriteType[] sprites;

        [System.Serializable]
        private class BackgroundSpriteType
        {
            public SpriteRenderer sprite;
            public float minScale;
            public float maxScale;

            [Range(0f, 1f)]
            public float dissapearChance;
        }

        private void Start()
        {
            System.Random rand = new(BodyManager.GetSeed());

            for (int i = 0; i < sprites.Length; i++)
            {
                if (rand.NextDouble() < sprites[i].dissapearChance)
                {
                    Destroy(sprites[i].sprite.gameObject);
                    continue;
                }

                float scale = RandomNum.GetFloat(sprites[i].minScale, sprites[i].maxScale, rand);
                sprites[i].sprite.transform.localScale = Vector3.one * scale;
            }
        }
    }
}
