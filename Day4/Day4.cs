using System;
using System.Collections.Generic;
using System.IO;

class Day4 {
    static void _Main(string[] args) {
        int passwordCount = 0;
        for (int i = 231832; i < 767346; i++) {
            if (IsValidPassword(i)) {
                passwordCount++;
            }
        }
        Console.WriteLine("Password count: " + passwordCount);
    }

    static bool IsValidPassword(int index) {
        string passAsString = index.ToString();
        // check for non-ascending values
        for (int i = 1; i < passAsString.Length; i++) {
            if (passAsString[i] < passAsString[i - 1]) {
                return false;
            }
        }
        
        // check for doubles
        bool hasIsolatedDouble = false;
        for (int i = 1; i < passAsString.Length; i++) {
            if (passAsString[i] == passAsString[i - 1]) {
                // potential double
                hasIsolatedDouble = true;
                i++;
                while (i < passAsString.Length && passAsString[i] == passAsString[i - 1]) {
                    hasIsolatedDouble = false;
                    i++;
                }

                if (hasIsolatedDouble) {
                    return true;
                }
            }
        }

        return false;
    }
}