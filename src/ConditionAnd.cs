using System;
using JetBrains.Annotations;

namespace GoodCat.Conditions
{
    internal class ConditionAnd : ICondition
    {
        internal static ConditionAnd GetTmp([NotNull] ICondition leftCondition, [NotNull] ICondition rightCondition)
        {
            ConditionAndTmp._leftCondition = leftCondition;
            ConditionAndTmp._rightCondition = rightCondition;
            return ConditionAndTmp;
        }

        private static ConditionAnd ConditionAndTmp { get; } = new ConditionAnd(Condition.True(), Condition.False());

        [NotNull] private ICondition _leftCondition;
        [NotNull] private ICondition _rightCondition;

        internal ConditionAnd([NotNull] ICondition leftCondition, [NotNull] ICondition rightCondition)
        {
            _leftCondition = leftCondition ?? throw new ArgumentNullException(nameof(leftCondition));
            _rightCondition = rightCondition ?? throw new ArgumentNullException(nameof(rightCondition));
        }

        public bool IsTrue() => _leftCondition.IsTrue() && _rightCondition.IsTrue();

        private bool Equals(ConditionAnd other)
        {
            return _leftCondition.Equals(other._leftCondition) && _rightCondition.Equals(other._rightCondition);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConditionAnd)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_leftCondition.GetHashCode() * 397) ^ _rightCondition.GetHashCode();
            }
        }

        public static bool operator ==(ConditionAnd left, ConditionAnd right) => Equals(left, right);
        public static bool operator !=(ConditionAnd left, ConditionAnd right) => !Equals(left, right);
    }
}