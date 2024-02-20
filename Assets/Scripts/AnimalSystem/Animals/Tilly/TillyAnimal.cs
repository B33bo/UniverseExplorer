using UnityEngine;

namespace Universe.Animals
{
    [Animal(SpeciesType = typeof(TillySpecies))]
    public class TillyAnimal : Animal
    {
        [SerializeField]
        private PolygonCollider2D polygonCollider;

        [SerializeField]
        private Eye.Renderer[] eyeRenderers;

        [SerializeField]
        private Leg.LegRenderer[] legRenderers;

        [SerializeField]
        private SpriteRenderer[] torsoRenderers;

        [SerializeField]
        private SpriteRenderer[] tailRenderers;

        [SerializeField]
        private Animator animator;

        public Tail[] tails;

        public override void Init(AnimalSpecies species, System.Random rand)
        {
            SpeciesInfo = species;
            TillySpecies tillySpecies = species as TillySpecies;

            float growth = RandomNum.GetFloat(-.2f, .2f, rand);

            legs = new Leg[2]; 
            BodyPart.InitBodyParts(legs, (i, leg) =>
            {
                leg.pattern = species.Legs[i].pattern;
                leg.length = species.Legs[i].length + growth;
                legRenderers[i].Init(leg);
            });

            var colliderPoints = polygonCollider.points;
            colliderPoints[2].y *= legs[0].length;
            colliderPoints[3].y *= legs[0].length;
            polygonCollider.points = colliderPoints;

            Color eyeColor = RandomNum.GetColor(rand);
            Eyes = new Eye[1];
            BodyPart.InitBodyParts(Eyes, (i, eye) =>
            {
                eye.Radius = SpeciesInfo.Eyes[i].Radius;
                eye.Strength = SpeciesInfo.Eyes[i].Strength;
                eye.IrisColor = eyeColor;

                eyeRenderers[i].Init(eye);
            });

            torso = new Torso
            {
                pattern = species.torso.pattern
            };

            for (int i = 0; i < torsoRenderers.Length; i++)
                torso.pattern.LoadMaterial(torsoRenderers[i].material);

            tails = new Tail[1];
            BodyPart.InitBodyParts(tails, (i, tail) =>
            {
                tail.pattern = tillySpecies.Tails[i].pattern;
                tail.length = tillySpecies.Tails[i].length + RandomNum.GetFloat(-.25f, .25f, rand);
                tail.pattern.LoadMaterial(tailRenderers[i].material);
            });

            Energy = SpeciesInfo.maxEnergy;

            StateChange(State.Foraging);
        }

        public override void StateChange(State oldState)
        {
            base.StateChange(oldState);
            animator.SetInteger("State", (int)AnimalState);
        }
    }
}
