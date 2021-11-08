using System;
using JetBrains.Annotations;

namespace GoodCat.Conditions
{
    internal class ConditionSimple : ICondition
    {
        internal static ConditionSimple GetTmp([NotNull] Func<bool> func)
        {
            ConditionSimpleTmp._func = func;
            return ConditionSimpleTmp;
        }

        private static ConditionSimple ConditionSimpleTmp { get; } = new ConditionSimple(() => true);

        [NotNull] private Func<bool> _func;

        internal ConditionSimple([NotNull] Func<bool> func) =>
            _func = func ?? throw new ArgumentNullException(nameof(func));

        public bool IsTrue() => _func.Invoke();
        private bool Equals(ConditionSimple other) => _func.Equals(other._func);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ConditionSimple)obj);
        }

        public override int GetHashCode() => _func.GetHashCode();
        public static bool operator ==(ConditionSimple left, ConditionSimple right) => Equals(left, right);
        public static bool operator !=(ConditionSimple left, ConditionSimple right) => !Equals(left, right);
    }
}