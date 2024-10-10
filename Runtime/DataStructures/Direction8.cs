using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sensen.Toolkit.Utils
{
    public sealed class Direction8
    {
        public enum Direction {
            Right,
            Down,
            Left,
            Up,
            DownRight,
            DownLeft,
            UpRight,
            UpLeft,
        }
        public static readonly Direction8 Right = new(Vector2.right);
        public static readonly Direction8 Down = new(Vector2.down);
        public static readonly Direction8 Left = new(Vector2.left);
        public static readonly Direction8 Up = new(Vector2.up);
        public static readonly Direction8 DownRight = new((Vector2.down + Vector2.right).normalized);
        public static readonly Direction8 DownLeft = new((Vector2.down + Vector2.left).normalized);
        public static readonly Direction8 UpRight = new((Vector2.up + Vector2.right).normalized);
        public static readonly Direction8 UpLeft = new((Vector2.up + Vector2.left).normalized);
        private static readonly List<(Direction, Direction8)> s_enumMap = new() {
            (Direction.Up, Up),
            (Direction.UpRight, UpRight),
            (Direction.Right, Right),
            (Direction.DownRight, DownRight),
            (Direction.Down, Down),
            (Direction.DownLeft, DownLeft),
            (Direction.Left, Left),
            (Direction.UpLeft, UpLeft),
        };
        private static readonly Dictionary<Direction8, Direction8> s_oppositeMap = new() {
            {Right, Left},
            {Down, Up},
            {Left, Right},
            {Up, Down},
            {DownRight, UpLeft},
            {DownLeft, UpRight},
            {UpRight, DownLeft},
            {UpLeft, DownRight}
        };

        public static IEnumerable<Direction8> All
        {
            get
            {
                foreach ((_, Direction8 direction) in s_enumMap)
                {
                    yield return direction;
                }
            }
        }

        public Vector2 Vector { get; }
        public Vector2Int VectorInt { get; }
        public Vector3 Vector3 { get; }
        public Vector3Int Vector3Int { get; }
        public float Angle { get; }
        public bool IsVertical => Vector.x == 0f;
        public bool IsHorizontal => Vector.y == 0f;
        public Direction8 Opposite => s_oppositeMap[this];

        private Direction8(Vector2 vector)
        {
            this.Vector = vector;
            this.Vector3 = vector;
            this.VectorInt = new Vector2Int(x: Mathf.CeilToInt(vector.x), y: Mathf.CeilToInt(vector.y));
            this.Vector3Int = new Vector3Int(VectorInt.x, VectorInt.y, 0);
            this.Angle = Vector2.SignedAngle(Vector2.right, vector);
        }

        public static Direction8 FromEnum(Direction v)
        {
            return v switch
            {
                Direction.Right => Right,
                Direction.Down => Down,
                Direction.Left => Left,
                Direction.Up => Up,
                Direction.DownRight => DownRight,
                Direction.DownLeft => DownLeft,
                Direction.UpRight => UpRight,
                Direction.UpLeft => UpLeft,
                _ => throw new System.Exception("Unrecognized Direction"),
            };
        }
    }
}
