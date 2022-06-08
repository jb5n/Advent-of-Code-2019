using System;
using System.IO;

class Day2 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day2/";
    
    static void _Main(string[] args) {
        // populate num array
        string[] values = File.ReadAllText(rootDirectory + "input.txt").Split(',');
        int[] numArray = new int[values.Length];
        for (int i = 0; i < values.Length; i++) {
            numArray[i] = int.Parse(values[i]);
        }
        
        // restore state just before the last computer caught fire
        numArray[1] = 0;
        numArray[2] = 1;
        
        // run the program
        int curIndex = 0;
        bool running = true;
        while (running) {
            switch (numArray[curIndex]) {
                case 1: // addition
                    numArray[numArray[curIndex + 3]] = numArray[numArray[curIndex + 1]] + numArray[numArray[curIndex + 2]];
                    break;
                case 2: // multiplication
                    numArray[numArray[curIndex + 3]] = numArray[numArray[curIndex + 1]] * numArray[numArray[curIndex + 2]];
                    break;
                case 99: // exit
                    running = false;
                    break;
                default:
                    Console.WriteLine("ERROR: BAD OPCODE " + numArray[curIndex]);
                    running = false;
                    break;
            }
            curIndex += 4;
        }

        foreach (int numElement in numArray) {
            Console.WriteLine(numElement);
        }
    }
}