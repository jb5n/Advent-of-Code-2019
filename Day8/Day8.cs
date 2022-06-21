using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Day8 {
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day8/";

    static void _Main(string[] args) {
        string input = File.ReadAllText(rootDirectory + "input.txt");
        int width = 25;
        int height = 6;

        List<string> layers = new List<string>();
        int layerPullIndex = 0;
        while (layerPullIndex < input.Length) {
            layers.Add(input.Substring(layerPullIndex, width * height));
            layerPullIndex += width * height;
        }

        char[] colors = "".PadRight(width * height, '.').ToCharArray();
        for (int i = 0; i < colors.Length; i++) {
            foreach (string layer in layers) {
                if (layer[i] != '2') {
                    colors[i] = layer[i];
                    break;
                }
            }
        }

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                Console.BackgroundColor = colors[width * y + x] == '1' ? ConsoleColor.White : ConsoleColor.Black;
                Console.Write(" ");
            }
            Console.Write("\n");
        }
    }
}