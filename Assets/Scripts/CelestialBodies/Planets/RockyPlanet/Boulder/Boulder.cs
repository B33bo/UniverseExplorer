using UnityEngine;

namespace Universe.CelestialBodies.Planets.Rocky
{
    public class Boulder : CelestialBody
    {
        public override string TypeString => "Boulder";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        public Mesh rock;
        public Color color;

        public static Mesh GetRock(System.Random randomNumberGenerator)
        {
            const int points = 6;
            float scale = RandomNum.GetFloat(.2f, 1f, randomNumberGenerator);
            Mesh rock = ShapeMaker.RandomizeMesh(ShapeMaker.SubdivideVerts(ShapeMaker.GetRegularShape(points, 1), 1), .2f, randomNumberGenerator);
            Vector3[] newVerticies = new Vector3[rock.vertexCount];

            for (int j = 0; j < rock.vertexCount; j++)
                newVerticies[j] = new Vector3(rock.vertices[j].x, rock.vertices[j].y * .5f, rock.vertices[j].z) * scale;
            rock.vertices = newVerticies;
            return rock;
        }

        public override void Create(Vector2 pos)
        {
            Position = pos; // so the random number generator seed gets triggered
            Create(pos, GetRock(RandomNumberGenerator));
        }

        public void Create(Vector2 pos, Mesh mesh)
        {
            Position = pos;
            rock = mesh;

            Vector2 bottomLeft = Vector2.zero, topRight = Vector2.zero;
            for (int i = 0; i < rock.vertexCount; i++)
            {
                if (bottomLeft.x > rock.vertices[i].x)
                    bottomLeft.x = rock.vertices[i].x;
                else if (topRight.x < rock.vertices[i].x)
                    topRight.x = rock.vertices[i].x;

                if (bottomLeft.y > rock.vertices[i].y)
                    bottomLeft.y = rock.vertices[i].y;
                else if (topRight.y < rock.vertices[i].y)
                    topRight.y = rock.vertices[i].y;
            }

            Width = (topRight.x - bottomLeft.x) * Measurement.M;
            Height = (topRight.y - bottomLeft.y) * Measurement.M;
            Mass = RandomNum.Get(100, 500, RandomNumberGenerator) * Width * Height * 1_000_000 * Measurement.Kg;
            Name = "Boulder";
        }
    }
}
