using System;
using System.IO;

class Day1 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day1/";
    
    static void Main(string[] args) {
        int totalMass = 0;
        foreach (string line in File.ReadLines(rootDirectory + "input.txt")) {
            totalMass += GetFuelRequired(int.Parse(line));
        }

        Console.WriteLine("Total mass required: " + totalMass);
    }

    static int GetFuelRequired(int fuelInput) {
        int fuelRequired = (int)Math.Floor(fuelInput / 3.0f) - 2;
        if (fuelRequired < 0) {
            return 0;
        }
        return fuelRequired + GetFuelRequired(fuelRequired);
    }
}
