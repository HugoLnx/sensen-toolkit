using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mathf = UnityEngine.Mathf;

namespace SensenToolkit.Rand
{
    public class RandomWithVariability
    {
        private readonly float[] probabilities;
        private readonly float percentReductionOnSelect;
        private readonly Dictionary<int, float> absoluteIncrease;
        private readonly bool noSequentialRepetition;
        private readonly Random rand = new();

        private int Count => probabilities.Length;
        private float ProbabilitiesSum => probabilities.Sum();
        private int lastSelected = -1;
        public bool Debugging;

        public RandomWithVariability(int optionsAmount, float percentReductionOnSelect = 0.25f, Dictionary<int, float> absoluteIncrease = null, bool noSequentialRepetition = false)
        {
            this.probabilities = new float[optionsAmount];
            if (Count <= 1) return;
            var remainder = 1f;
            for (var i = 0; i < optionsAmount - 1; i++)
            {
                var probability = 1f / optionsAmount;
                probabilities[i] = probability;
                remainder -= probability;
            }
            probabilities[optionsAmount - 1] = remainder;
            this.percentReductionOnSelect = percentReductionOnSelect;
            this.absoluteIncrease = absoluteIncrease;
            this.noSequentialRepetition = noSequentialRepetition;
        }

        public int Select()
        {
            if (Count <= 1) return 0;
            var option = GetOption();
            probabilities[option] *= 1f - percentReductionOnSelect;
            RebalanceProbabilitiesAfterReduction(selectedOption: option);
            if (Debugging) UnityEngine.Debug.Log($"Before Increase: {option} [{string.Join(" | ", probabilities)}]");
            DoAbsoluteIncrease(selectedOption: option);
            if (Debugging) UnityEngine.Debug.Log($"Selected: {option} [{string.Join(" | ", probabilities)}]");
            return option;
        }

        private void DoAbsoluteIncrease(int selectedOption)
        {
            if (absoluteIncrease == null) return;
            var options = new HashSet<int>(absoluteIncrease.Keys);
            options.Remove(selectedOption);
            var maxIncrease = 0f;
            for (var i = 0; i < Count; i++)
            {
                if (options.Contains(i)) continue;
                maxIncrease += probabilities[i];
            }
            if (Debugging) UnityEngine.Debug.Log($"MaxIncrease: {maxIncrease}");

            var totalIncrease = 0f;
            foreach (var option in options)
            {
                if (Debugging) UnityEngine.Debug.Log($"ConfigIncrease {option}: {absoluteIncrease[option]}");
                if (option < 0 || option >= probabilities.Length) continue;
                var increase = Mathf.Min(maxIncrease, absoluteIncrease[option]);
                if (Debugging) UnityEngine.Debug.Log($"Increase {option}: {increase} / {absoluteIncrease[option]}");
                totalIncrease += increase;
                probabilities[option] += increase;
                maxIncrease -= increase;
            }
            if (Debugging) UnityEngine.Debug.Log($"TotalIncrease: {totalIncrease}");
            var nonIncreasedOptions = new List<int>();
            for (var i = 0; i < Count; i++)
            {
                if (options.Contains(i)) continue;
                nonIncreasedOptions.Add(i);
            }
            nonIncreasedOptions = nonIncreasedOptions.OrderBy(opt => probabilities[opt]).ToList();
            var toDecrease = totalIncrease;
            var remainderOptionsCount = nonIncreasedOptions.Count;
            foreach (var option in nonIncreasedOptions)
            {
                if (toDecrease <= 0f) continue;
                var decrease = Mathf.Min(probabilities[option], toDecrease / remainderOptionsCount);
                probabilities[option] -= decrease;
                toDecrease -= decrease;
                remainderOptionsCount--;
            }
        }

        private void RebalanceProbabilitiesAfterReduction(int selectedOption)
        {
            var remainder = 1f - ProbabilitiesSum;
            var toIncrease = remainder / (Count - 1);
            for (var i = 0; i < Count; i++)
            {
                if (i == selectedOption) continue;
                probabilities[i] += toIncrease;
            }
        }

        private int GetOption()
        {
            var selected = -1;
            var acc = 0f;
            var value = (float)rand.NextDouble();
            List<int> options = new List<int>();
            for (var i = 0; i < probabilities.Length; i++)
            {
                if (noSequentialRepetition && lastSelected == i) continue;
                options.Add(i);
            }
            foreach (var option in options)
            {
                acc += probabilities[option] + (noSequentialRepetition && lastSelected > -1 ? probabilities[lastSelected] / (Count - 1) : 0f);
                if (value <= acc)
                {
                    selected = option;
                    break;
                }
            }
            if (selected == -1)
            {
                UnityEngine.Debug.LogError($"WRONG OPTION!! Couldn't find option for {value} between probabilities: {String.Join("; ", probabilities)}");
                selected = Count - 1; // Just to don't blow up an error
            }
            lastSelected = selected;
            return selected;
        }

        public override string ToString() => $"RandomWV: [{String.Join("; ", probabilities)}] (Sum: {ProbabilitiesSum}) (lastSelected: {lastSelected})";
    }
}
