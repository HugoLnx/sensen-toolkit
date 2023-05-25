using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensenToolkit.Rand
{
    public class RandomEnumWithVariability<T>
    where T : System.Enum
    {
        private readonly RandomWithVariability rand;
        private readonly Dictionary<int, T> intToEnum;

        public RandomEnumWithVariability(float percentReductionOnSelect = 0.25f, Dictionary<T, float> absoluteIncrease = null, bool noSequentialRepetition = false)
        {
            var values = System.Enum.GetValues(typeof(T));
            var enumToInt = new Dictionary<T, int>();
            this.intToEnum = new Dictionary<int, T>();
            foreach (int value in values)
            {
                var name = System.Enum.GetName(typeof(T), value);
                var key = (T)System.Enum.Parse(typeof(T), name);
                enumToInt[key] = value;
                intToEnum[value] = key;
            }

            Dictionary<int, float> intAbsIncrease = null;
            if (absoluteIncrease != null)
            {
                intAbsIncrease = new Dictionary<int, float>();
                foreach (var pair in absoluteIncrease)
                {
                    intAbsIncrease[enumToInt[pair.Key]] = pair.Value;
                }
            }

            this.rand = new RandomWithVariability(
                optionsAmount: values.Length,
                percentReductionOnSelect: percentReductionOnSelect,
                absoluteIncrease: intAbsIncrease,
                noSequentialRepetition: noSequentialRepetition
            );
        }

        public T Select()
        {
            return intToEnum[this.rand.Select()];
        }
    }
}
