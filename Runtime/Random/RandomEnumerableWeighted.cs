using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Rand
{
    public class RandomEnumerableWeighted<T>
    {
        private readonly RandomWeighted rand;
        private readonly Dictionary<int, T> inxToValue;

        public RandomEnumerableWeighted(IEnumerable<T> values, IEnumerable<float> weights)
        {
            this.inxToValue = new Dictionary<int, T>();
            var count = 0;
            foreach (T val in values)
            {
                inxToValue[count] = val;
                count++;
            }

            this.rand = new RandomWeighted(
                weights: new List<float>(weights).ToArray()
            );
        }

        public T Select()
        {
            return inxToValue[this.rand.Select()];
        }
    }
}
