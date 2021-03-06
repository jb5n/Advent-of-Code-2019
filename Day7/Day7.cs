using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

class Day7 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day7/";

    private static Dictionary<int, Queue<int>> intcodeQueues = new Dictionary<int, Queue<int>>();
    private static int thrusterOutput = 0;

    private static Stopwatch timer;

    static void _Main(string[] args) {
        // populate num array
        string[] values = File.ReadAllText(rootDirectory + "input.txt").Split(',');
        timer = Stopwatch.StartNew();
        int[] numArray = new int[values.Length];
        for (int i = 0; i < values.Length; i++) {
            numArray[i] = int.Parse(values[i]);
        }
        
        List<int[]> phaseSequencePermutations = new List<int[]>();
        int[] phaseSequence = {5, 6, 7, 8, 9};
        GetPermutations(phaseSequence, 0, phaseSequence.Length - 1, ref phaseSequencePermutations);
        foreach (int phase in phaseSequence) {
            intcodeQueues.Add(phase, new Queue<int>());
        }

        int maxOutput = 0;
        foreach (int[] sequencePermutation in phaseSequencePermutations) {
            thrusterOutput = 0;
            foreach (int phase in phaseSequence) {
                intcodeQueues[phase].Clear();
            }

            IEnumerator[] computers = new IEnumerator[sequencePermutation.Length];
            for(int i = 0; i < sequencePermutation.Length; i++) {
                int nextSequence = i == sequencePermutation.Length - 1 ? sequencePermutation[0] : sequencePermutation[i + 1];
                intcodeQueues[sequencePermutation[i]].Enqueue(sequencePermutation[i]);
                if (i == 0) {
                    intcodeQueues[sequencePermutation[i]].Enqueue(0);
                }
                
                computers[i] = IntcodeComputer(sequencePermutation[i], nextSequence, numArray, i == sequencePermutation.Length - 1);
            }

            bool allComputersComplete = false;
            while (!allComputersComplete) {
                allComputersComplete = true;
                for (int i = 0; i < sequencePermutation.Length; i++) {
                    if (computers[i].MoveNext()) {
                        allComputersComplete = false;
                    }
                }
            }

            if (thrusterOutput > maxOutput) {
                maxOutput = thrusterOutput;
            }
        }
        timer.Stop();

        Console.WriteLine("Highest thruster output: " + maxOutput + ". Duration ms: " + timer.Elapsed.TotalMilliseconds);
    }

    static void GetPermutations(int[] phaseSequence, int startIndex, int endIndex, ref List<int[]> permutations) {
        if (startIndex == endIndex) {
            int[] newSequence = new int[phaseSequence.Length];
            for (int i = 0; i < phaseSequence.Length; i++) {
                newSequence[i] = phaseSequence[i];
            }
            permutations.Add(newSequence);
        }
        else
        { 
            for (int i = startIndex; i <= endIndex; i++) 
            { 
                Swap(ref phaseSequence[startIndex], ref phaseSequence[i]);
                GetPermutations(phaseSequence, startIndex + 1, endIndex, ref permutations); 
                Swap(ref phaseSequence[startIndex], ref phaseSequence[i]);
            } 
        } 
    }

    static void Swap(ref int a, ref int b) {
        (a, b) = (b, a);
    }
    
    static IEnumerator IntcodeComputer(int phase, int outputPhase, int[] numArrayOriginal, bool sendOutputToThrusters) {
        // copy the numArray
        int[] numArrayCopy = new int[numArrayOriginal.Length];
        for (int i = 0; i < numArrayOriginal.Length; i++) {
            numArrayCopy[i] = numArrayOriginal[i];
        }
        // run the program
        int curIndex = 0;
        bool running = true;
        string paramModes = "";
        while (running) {
            int instruction = numArrayCopy[curIndex];
            int opcode = instruction;
            if (numArrayCopy[curIndex] > 99) {
                opcode = int.Parse(instruction.ToString().Substring(instruction.ToString().Length - 2));
                paramModes = instruction.ToString().Substring(0, instruction.ToString().Length - 2);
                paramModes = paramModes.PadLeft(3, '0');
            }
            else {
                paramModes = "0000";
            }

            int[] parameters = new int[] { 0, 0, 0 };
            for (int i = 1; i <= 3; i++) {
                // don't go past the end of the array
                if (curIndex + i >= numArrayCopy.Length) {
                    break;
                }
                parameters[i - 1] = paramModes[3 - i] == '0' ? numArrayCopy[curIndex + i] : curIndex + i;
            }

            switch (opcode) {
                case 1: // addition
                    numArrayCopy[parameters[2]] = numArrayCopy[parameters[0]] + numArrayCopy[parameters[1]];
                    curIndex += 4;
                    break;
                case 2: // multiplication
                    numArrayCopy[parameters[2]] = numArrayCopy[parameters[0]] * numArrayCopy[parameters[1]];
                    curIndex += 4;
                    break;
                case 3:
                    while (intcodeQueues[phase].Count == 0) {
                        //Console.WriteLine("Waiting " + phase);
                        yield return null;
                    }
                    numArrayCopy[parameters[0]] = intcodeQueues[phase].Dequeue();
                    curIndex += 2;
                    break;
                case 4:
                    int output = numArrayCopy[parameters[0]];
                    intcodeQueues[outputPhase].Enqueue(output);
                    if (sendOutputToThrusters) {
                        thrusterOutput = output;
                    }
                    curIndex += 2;
                    break;
                case 5:
                    if (numArrayCopy[parameters[0]] != 0) {
                        curIndex = numArrayCopy[parameters[1]];
                    }
                    else {
                        curIndex += 3;
                    }
                    break;
                case 6:
                    if (numArrayCopy[parameters[0]] == 0) {
                        curIndex = numArrayCopy[parameters[1]];
                    }
                    else {
                        curIndex += 3;
                    }
                    break;
                case 7:
                    if (numArrayCopy[parameters[0]] < numArrayCopy[parameters[1]]) {
                        numArrayCopy[parameters[2]] = 1;
                    }
                    else {
                        numArrayCopy[parameters[2]] = 0;
                    }
                    curIndex += 4;
                    break;
                case 8:
                    if (numArrayCopy[parameters[0]] == numArrayCopy[parameters[1]]) {
                        numArrayCopy[parameters[2]] = 1;
                    }
                    else {
                        numArrayCopy[parameters[2]] = 0;
                    }
                    curIndex += 4;
                    break;
                case 99: // exit
                    running = false;
                    break;
                default:
                    Console.WriteLine("ERROR: BAD OPCODE " + numArrayCopy[curIndex]);
                    running = false;
                    break;
            }
        }
        yield break;
    }
}