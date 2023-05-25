using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SensenToolkit.Rand
{
    public class RandomWeighted
    {
        private float[] probabilities;
        private int Count => probabilities.Length;
        private readonly Random rand = new();
        public bool Debugging;
        private readonly List<int> optionsBuffer = new List<int>();

        public RandomWeighted(float[] weights) {
            this.probabilities = new float[weights.Length];
            if (Count <= 1) return;

            var sum = weights.Sum();
            for (var i = 0; i < weights.Length; i++) {
                var prob = weights[i] / sum;
                if (float.IsNaN(prob) || float.IsInfinity(prob)) {
                    UnityEngine.Debug.Log("PROBABILITY IS NAN OR INFINITY");
                    prob = 0f;
                }
                probabilities[i] = prob;
            }
        }

        public int Select() {
            if (Count <= 1) return 0;
            var option = GetOption();
            if (Debugging) UnityEngine.Debug.Log($"Selected: {option} [{string.Join(" | ", probabilities)}]");
            return option;
        }

        private int GetOption()
        {
            var acc = 0f;
            var value = (float) rand.NextDouble();
            for (var i = 0; i < probabilities.Length; i++) {
                acc += probabilities[i];
                if (value <= acc) return i;
            }
            throw new Exception($"No Option found! {this} value={value}");
        }

        public override string ToString() => $"RandomWV: [{String.Join("; ", probabilities)}] (Sum: {probabilities.Sum()})";
    }
}
