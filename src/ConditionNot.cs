using System;
using JetBrains.Annotations;

namespace GoodCat.Conditions
{
    internal class ConditionNot : ICondition
    {
        internal static ConditionNot GetTmp([NotNull] ICondition condition)
        {
            _conditionNot._condition = condition;
            return _conditionNot;
        }
        private static ConditionNot _conditionNot { get; } = new ConditionNot(Condition.True());
        
        [NotNull] private ICondition _condition;

        internal ConditionNot([NotNull] ICondition condition)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public bool IsTrue() => !_condition.IsTrue();

        private bool Equals(ConditionNot other) => _condition.Equals(other._condition);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConditionNot) obj);
        }

        public override int GetHashCode() => _condition.GetHashCode();

        public static bool operator ==(ConditionNot left, ConditionNot right) => Equals(left, right);
        public static bool operator !=(ConditionNot left, ConditionNot right) => !Equals(left, right);
    }
}