using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Rand
{
    public class RandomEnumerableWithVariability<T>
    {
        private readonly RandomWithVariability rand;
        private readonly Dictionary<int, T> inxToValue;

        public RandomEnumerableWithVariability(IEnumerable<T> values, float percentReductionOnSelect = 0.25f, Dictionary<int, float> absoluteIncrease = null, bool noSequentialRepetition = false)
        {
            this.inxToValue = new Dictionary<int, T>();
            var count = 0;
            foreach (T key in values)
            {
                inxToValue[count] = key;
                count++;
            }

            this.rand = new RandomWithVariability(
                optionsAmount: count,
                percentReductionOnSelect: percentReductionOnSelect,
                absoluteIncrease: absoluteIncrease,
                noSequentialRepetition: noSequentialRepetition
            );
        }

        public T Select()
        {
            return inxToValue[this.rand.Select()];
        }
    }
}
