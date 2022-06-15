using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day6 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day6/";
    
    static void _Main(string[] args) {
        string[] allLines = File.ReadAllLines(rootDirectory + "input.txt");
        // value is the list of strings that orbit around key
        Dictionary<string, List<string>> orbitingDict = new Dictionary<string, List<string>>();
        // key is the orbiting object, value is the object key is orbiting
        Dictionary<string, string> orbitParentMap = new Dictionary<string, string>();
        foreach(string line in allLines) {
            string parentBody = line.Split(')')[0];
            string childBody = line.Split(')')[1];
            orbitParentMap.Add(childBody, parentBody);
            if (!orbitingDict.ContainsKey(parentBody)) {
                orbitingDict.Add(parentBody, new List<string>());
            }
            orbitingDict[parentBody].Add(childBody);
        }
        
        int totalOrbits = FindNumOrbits("COM", orbitingDict, 0);
        Console.WriteLine("Total number of orbits: " + totalOrbits);

        List<string> youChain = GetOrbitChain("YOU", orbitParentMap);
        List<string> sanChain = GetOrbitChain("SAN", orbitParentMap);
        int orbitalTransferCount = 0;
        foreach(string youChainItem in youChain)
        {
            if (sanChain.Contains(youChainItem)) {
                orbitalTransferCount = youChain.IndexOf(youChainItem) + sanChain.IndexOf(youChainItem);
                break;
            }
        }
        Console.WriteLine("Orbital transfers necessary: " + orbitalTransferCount);
    }

    static int FindNumOrbits(string targetBody, Dictionary<string, List<string>> orbitingDict, int depth) {
        Console.WriteLine("Checking orbits of " + targetBody + ", Depth " + depth);
        if (!orbitingDict.ContainsKey(targetBody)) {
            return depth;
        }

        int orbits = depth;
        foreach (string childOrbit in orbitingDict[targetBody]) {
            orbits += FindNumOrbits(childOrbit, orbitingDict, depth + 1);
        }
        return orbits;
    }

    static List<string> GetOrbitChain(string source, Dictionary<string, string> orbitParentMap) {
        string parent = source;
        List<string> chain = new List<string>();
        while (parent != "COM") {
            parent = orbitParentMap[parent];
            chain.Add(parent);
        }
        return chain;
    }
}