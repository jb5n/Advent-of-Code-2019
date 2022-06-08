using System;
using System.Collections.Generic;
using System.IO;

class Day3 {
    struct LineSegment {
        public int x1;
        public int y1;
        public int x2;
        public int y2;

        public LineSegment(int x1, int y1, int x2, int y2) {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        }

        public bool Intersects(LineSegment other, ref int x, ref int y) {
            if (other.IsVertical() == this.IsVertical()) {
                return false;
            }

            if (this.IsVertical()) {
                if (x1 >= Math.Min(other.x1, other.x2) && x1 <= Math.Max(other.x1, other.x2) &&
                    other.y1 >= Math.Min(y1, y2) && other.y1 <= Math.Max(y1, y2)) {
                    x = x1;
                    y = other.y1;
                    return true;
                }
                return false;
            }
            else {
                return other.Intersects(this, ref x, ref y); // swap places
            }
        }

        public bool IsVertical() {
            return x1 == x2;
        }

        public override string ToString() {
            return x1 + " " + y1 + " " + x2 + " " + y2;
        }

        public bool Equals(LineSegment other) {
            return this.x1 == other.x1 && this.x2 == other.x2 && this.y1 == other.y1 && this.y2 == other.y2;
        }

        public int GetLength() {
            return (Math.Max(x1, x2) - Math.Min(x1, x2)) + (Math.Max(y1, y2) - Math.Min(y1, y2));
        }

        public int DistanceFromOrigin(int x, int y) {
            return Math.Abs(x - x1) + Math.Abs(y - y1);
        }
    }
    
    private static string rootDirectory = "H:/AdventOfCode/AoC2019Justin/Day3/";
    
    static void Main(string[] args) {
        string[] allLines = File.ReadAllLines(rootDirectory + "input.txt");
        List<LineSegment> firstWire = ParseLine(allLines[0]);
        List<LineSegment> secondWire = ParseLine(allLines[1]);
        
        int shortestPath = int.MaxValue;
        int firstPath = 0, secondPath = 0;
        foreach (LineSegment firstSegment in firstWire) {
            foreach (LineSegment secondSegment in secondWire) {
                if (firstSegment.Equals(firstWire[0]) && secondSegment.Equals(secondWire[0])) {
                    // avoids first case, since both wires start at the same point
                    continue;
                }
                int[] intersection = new int[2];
                if (firstSegment.Intersects(secondSegment, ref intersection[0], ref intersection[1])) {
                    Console.WriteLine("Intersect at " + intersection[0] + ", " + intersection[1]);
                    int firstPathIntersectionLength = firstPath + firstSegment.DistanceFromOrigin(intersection[0], intersection[1]);
                    int secondPathIntersectionLength = secondPath + secondSegment.DistanceFromOrigin(intersection[0], intersection[1]);
                    Console.WriteLine("Dist from origin 1: " + firstSegment.DistanceFromOrigin(intersection[0], intersection[1]));
                    Console.WriteLine("Dist from origin 2: " + secondSegment.DistanceFromOrigin(intersection[0], intersection[1]));
                    Console.WriteLine("Intersection lengths: " + firstPathIntersectionLength + "," + secondPathIntersectionLength);
                    int pathDist = firstPathIntersectionLength + secondPathIntersectionLength;
                    if (pathDist < shortestPath) {
                        shortestPath = pathDist;
                    }
                }

                secondPath += secondSegment.GetLength();
            }

            secondPath = 0;
            firstPath += firstSegment.GetLength();
        }
        
        Console.WriteLine("Shortest path steps: " + shortestPath);
    }

    private static List<LineSegment> ParseLine(string line) {
        List<LineSegment> wire = new List<LineSegment>();
        int x = 0, y = 0;
        foreach (string segment in line.Split(',')) {
            int endX = x, endY = y;
            int num = int.Parse(segment.Substring(1));
            switch (segment[0]) {
                case 'R':
                    endX += num;
                    break;
                case 'L':
                    endX -= num;
                    break;
                case 'U':
                    endY += num;
                    break;
                case 'D':
                    endY -= num;
                    break;
            }
            
            wire.Add(new LineSegment(x, y, endX, endY));
            Console.WriteLine("Line segment: " + new LineSegment(x, y, endX, endY));

            x = endX;
            y = endY;
        }
        return wire;
    }
}