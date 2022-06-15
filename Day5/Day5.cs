using System;
using System.IO;

class Day5 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day5/";
    
    static void _Main(string[] args) {
        // populate num array
        string[] values = File.ReadAllText(rootDirectory + "input.txt").Split(',');
        int[] numArray = new int[values.Length];
        for (int i = 0; i < values.Length; i++) {
            numArray[i] = int.Parse(values[i]);
        }

        int providedInput = 5;

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
                    numArray[parameters[0]] = providedInput;
                    curIndex += 2;
                    break;
                case 4:
                    Console.WriteLine("OUTPUT: " + numArray[parameters[0]]);
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
    }
}