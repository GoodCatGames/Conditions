using System.Collections.Generic;

namespace GoodCat.Conditions
{
    internal class ConditionsCache
    {
        private readonly Dictionary<ICondition, ICondition> _dictionary = new Dictionary<ICondition, ICondition>();

        internal void Reset() => _dictionary.Clear();
        
        internal void Add(ICondition condition) => _dictionary.Add(condition, condition);
        
        internal bool TryGet(ICondition tmpForCompare, out ICondition result) =>
            _dictionary.TryGetValue(tmpForCompare, out result);
    }
}