using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day7 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day7/";
    
    static void Main(string[] args) {
        // populate num array
        string[] values = File.ReadAllText(rootDirectory + "input.txt").Split(',');
        int[] numArray = new int[values.Length];
        for (int i = 0; i < values.Length; i++) {
            numArray[i] = int.Parse(values[i]);
        }
        
        List<int[]> phaseSequencePermutations = new List<int[]>();
        int[] phaseSequence = {0, 1, 2, 3, 4};
        GetPermutations(phaseSequence, 0, phaseSequence.Length - 1, ref phaseSequencePermutations);

        int highestSignal = 0;
        foreach (int[] sequencePermutation in phaseSequencePermutations) {
            int prevOutput = 0;
            foreach (int phase in sequencePermutation) {
                int[] output = IntcodeComputer(new int[] { phase, prevOutput }, numArray);
                if (output.Length != 1) {
                    Console.WriteLine("BAD OUTPUT LENGTH: " + output.Length);
                    break;
                }
                prevOutput = output[0];
            }

            if (prevOutput > highestSignal) {
                highestSignal = prevOutput;
            }
        }
        Console.WriteLine("Highest signal output: " + highestSignal);
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
    
    static int[] IntcodeComputer(int[] inputs, int[] numArray) {
        List<int> outputs = new List<int>();
        int inputCounter = 0;

        // run the program
        int curIndex = 0;
        bool running = true;
        string paramModes = "";
        while (running) {
            int instruction = numArray[curIndex];
            int opcode = instruction;
            if (numArray[curIndex] > 99) {
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
                if (curIndex + i >= numArray.Length) {
                    break;
                }
                parameters[i - 1] = paramModes[3 - i] == '0' ? numArray[curIndex + i] : curIndex + i;
            }

            switch (opcode) {
                case 1: // addition
                    numArray[parameters[2]] = numArray[parameters[0]] + numArray[parameters[1]];
                    curIndex += 4;
                    break;
                case 2: // multiplication
                    numArray[parameters[2]] = numArray[parameters[0]] * numArray[parameters[1]];
                    curIndex += 4;
                    break;
                case 3:
                    numArray[parameters[0]] = inputs[inputCounter];
                    inputCounter++;
                    curIndex += 2;
                    break;
                case 4:
                    int output = numArray[parameters[0]];
                    outputs.Add(output);
                    Console.WriteLine("OUTPUT: " + output);
                    curIndex += 2;
                    break;
                case 5:
                    if (numArray[parameters[0]] != 0) {
                        curIndex = numArray[parameters[1]];
                    }
                    else {
                        curIndex += 3;
                    }
                    break;
                case 6:
                    if (numArray[parameters[0]] == 0) {
                        curIndex = numArray[parameters[1]];
                    }
                    else {
                        curIndex += 3;
                    }
                    break;
                case 7:
                    if (numArray[parameters[0]] < numArray[parameters[1]]) {
                        numArray[parameters[2]] = 1;
                    }
                    else {
                        numArray[parameters[2]] = 0;
                    }
                    curIndex += 4;
                    break;
                case 8:
                    if (numArray[parameters[0]] == numArray[parameters[1]]) {
                        numArray[parameters[2]] = 1;
                    }
                    else {
                        numArray[parameters[2]] = 0;
                    }
                    curIndex += 4;
                    break;
                case 99: // exit
                    running = false;
                    break;
                default:
                    Console.WriteLine("ERROR: BAD OPCODE " + numArray[curIndex]);
                    running = false;
                    break;
            }
        }
        return outputs.ToArray();
    }
}