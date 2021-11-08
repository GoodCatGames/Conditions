using System;

namespace GoodCat.Conditions
{
    public static class Condition
    {
        public static ICondition True() => _conditionTrue;
        public static ICondition False() => _conditionFalse;

        private static readonly ConditionTrue _conditionTrue = new ConditionTrue();
        private static readonly ConditionFalse _conditionFalse = new ConditionFalse();
        
        private static readonly ConditionsCache Cache = new ConditionsCache();

        public static ICondition Get(Func<bool> func)
        {
            var tmp = ConditionSimple.GetTmp(func);
            if (Cache.TryGet(tmp, out var result) == false)
            {
                result = new ConditionSimple(func);
                Cache.Add(result);
            }

            return result;
        }

        public static ICondition Not(this ICondition condition)
        {
            var tmp = ConditionNot.GetTmp(condition);
            if (Cache.TryGet(tmp, out var result) == false)
            {
                result = new ConditionNot(condition);
                Cache.Add(result);
            }

            return result;
        }

        public static ICondition Or(this ICondition left, ICondition right)
        {
            var tmp = ConditionOr.GetTmp(left, right);
            if (Cache.TryGet(tmp, out var result) == false)
            {
                result = new ConditionOr(left, right);
                Cache.Add(result);
            }

            return result;
        }

        public static ICondition And(this ICondition left, ICondition right)
        {
            var tmp = ConditionAnd.GetTmp(left, right);
            if (Cache.TryGet(tmp, out var result) == false)
            {
                result = new ConditionAnd(left, right);
                Cache.Add(result);
            }

            return result;
        }

        /// <summary>
        /// string - Name
        /// bool - Result
        /// </summary>
        public static ICondition Debug(this ICondition condition, string name,
            Action<string, bool> debugAction = null)
        {
            var tmp = ConditionDebug.GetTmp(condition, name, debugAction);
            if (Cache.TryGet(tmp, out var result) == false)
            {
                result = new ConditionDebug(condition, name, debugAction);
                Cache.Add(result);
            }

            return result;
        }

        /// <summary>
        /// Only for UnitTests
        /// </summary>
        public static void ResetCache() => Cache.Reset();

        private class ConditionTrue : ICondition
        {
            public bool IsTrue() => true;
        }

        private class ConditionFalse : ICondition
        {
            public bool IsTrue() => false;
        }
    }
}