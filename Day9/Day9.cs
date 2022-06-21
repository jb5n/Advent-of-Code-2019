using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Numerics;

class Day9 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day9/";

    private static Dictionary<BigInteger, Queue<BigInteger>> intcodeQueues = new Dictionary<BigInteger, Queue<BigInteger>>();

    private static int relativeBase = 0;

    static void Main(string[] args) {
        // populate num array
        string[] values = File.ReadAllText(rootDirectory + "input.txt").Split(',');
        BigInteger[] numArray = new BigInteger[values.Length + 300];
        for (int i = 0; i < numArray.Length; i++) {
            if (i < values.Length) {
                numArray[i] = BigInteger.Parse(values[i]);
            }
            else {
                numArray[i] = 0;
            }
        }
        
        intcodeQueues.Add(0, new Queue<BigInteger>());
        intcodeQueues[0].Enqueue(2);
        intcodeQueues.Add(1, new Queue<BigInteger>());

        IEnumerator computer = IntcodeComputer(0, 1, numArray);
        while (computer.MoveNext()) {
        }
        Console.WriteLine("Output: " + intcodeQueues[1].Dequeue());
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
    
    static IEnumerator IntcodeComputer(int inputQueueIndex, int outputQueueIndex, BigInteger[] numArrayOriginal) {
        // copy the numArray
        BigInteger[] numArrayCopy = new BigInteger[numArrayOriginal.Length];
        for (int i = 0; i < numArrayOriginal.Length; i++) {
            numArrayCopy[i] = numArrayOriginal[i];
        }
        // run the program
        int curIndex = 0;
        bool running = true;
        string paramModes = "";
        while (running) {
            BigInteger instruction = numArrayCopy[curIndex];
            int opcode = (int)instruction;
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

                // parameter modes
                if (paramModes[3 - i] == '0') { // position mode
                    parameters[i - 1] = (int)numArrayCopy[curIndex + i];
                }
                else if (paramModes[3 - i] == '1') { // immediate mode
                    parameters[i - 1] = curIndex + i;
                }
                else if (paramModes[3 - i] == '2') { // relative mode
                    parameters[i - 1] = (int)numArrayCopy[curIndex + i] + relativeBase;
                }
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
                    while (intcodeQueues[inputQueueIndex].Count == 0) {
                        Console.WriteLine("Waiting " + inputQueueIndex);
                        yield return null;
                    }
                    numArrayCopy[parameters[0]] = intcodeQueues[inputQueueIndex].Dequeue();
                    curIndex += 2;
                    break;
                case 4:
                    BigInteger output = numArrayCopy[parameters[0]];
                    intcodeQueues[outputQueueIndex].Enqueue(output);
                    curIndex += 2;
                    break;
                case 5:
                    if (numArrayCopy[parameters[0]] != 0) {
                        curIndex = (int)numArrayCopy[parameters[1]];
                    }
                    else {
                        curIndex += 3;
                    }
                    break;
                case 6:
                    if (numArrayCopy[parameters[0]] == 0) {
                        curIndex = (int)numArrayCopy[parameters[1]];
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
                case 9:
                    relativeBase += (int)numArrayCopy[parameters[0]];
                    curIndex += 2;
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