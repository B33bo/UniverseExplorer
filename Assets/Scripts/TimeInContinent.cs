using System;
using Universe.CelestialBodies.Biomes;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public static class TimeInContinent
    {
        // There's an easter egg where setting a star's seed to 0 will result in it creating our solar system
        // When the earth is spawned it has all the continents in it as well
        // Each continent has a date time controller so I want to make it the actual time on that continent for a bonus easter egg

        private static float Africa => ToDayNightSystemTime(DateTime.UtcNow.AddHours(2));
        private static float NorthAmerica => ToDayNightSystemTime(DateTime.UtcNow.AddHours(-6.5f));
        private static float SouthAmerica => ToDayNightSystemTime(DateTime.UtcNow.AddHours(-3.333f));
        private static float Europe => ToDayNightSystemTime(DateTime.UtcNow.AddHours(1));
        private static float Asia => ToDayNightSystemTime(DateTime.UtcNow.AddHours(6.375f));
        private static float Oceania => ToDayNightSystemTime(DateTime.UtcNow.AddHours(9.1666f));
        private static float Antarctica => AntarcticaTime(DateTime.UtcNow);

        private static float ToDayNightSystemTime(DateTime time)
        {
            float hours = time.Hour + (time.Minute * 01666f) + (time.Second * 0.0002777f);
            return hours / 12;
        }

        private static float AntarcticaTime(DateTime time)
        {
            // antarctican days last 6 months
            int month = (time.Month + 7) % 12; // so that summer is at the start
            float days = month * 30.4167f + time.Day;
            return days / 180;
        }

        public static float ContinentTime(Continent c)
        {
            return c.Seed switch
            {
                TerrestrialPlanet.NorthAmerica => NorthAmerica,
                TerrestrialPlanet.SouthAmerica => SouthAmerica,
                TerrestrialPlanet.Africa => Africa,
                TerrestrialPlanet.Europe => Europe,
                TerrestrialPlanet.Asia => Asia,
                TerrestrialPlanet.Oceania => Oceania,
                TerrestrialPlanet.Antarctica => Antarctica,
                _ => -1,
            };
        }
    }
}
