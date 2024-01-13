using System;
using System.Linq;
using NUnit.Framework;
using Universe;

public class PlanetPath
{
    [Test]
    public void PlanetPathLocated()
    {
        var planets = GetPlanetRenderers();
        for (int i = 0; i < planets.Length; i++)
        {
            Type type = planets[i].PlanetType;
            var inst = Activator.CreateInstance(type);
            string pos = type.GetProperty("ObjectFilePos").GetValue(inst).ToString();

            var bodyFound = ObjectPaths.Instance.GetCelestialBody(pos);
            Assert.IsNotNull(bodyFound);
        }
    }

    private static PlanetRenderer[] GetPlanetRenderers()
    {
        return ObjectPaths.Instance.objects.Where(x => x.Object is PlanetRenderer).Select(x => x.Object as PlanetRenderer).ToArray();
    }
}
