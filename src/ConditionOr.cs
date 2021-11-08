using System;
using JetBrains.Annotations;

namespace GoodCat.Conditions
{
    internal class ConditionOr : ICondition
    {
        internal static ConditionOr GetTmp([NotNull] ICondition leftCondition, [NotNull] ICondition rightCondition)
        {
            _conditionOrTmp._leftCondition = leftCondition;
            _conditionOrTmp._rightCondition = rightCondition;
            return _conditionOrTmp;
        }

        private static ConditionOr _conditionOrTmp { get; } = new ConditionOr(Condition.True(), Condition.False());

        [NotNull] private ICondition _leftCondition;
        [NotNull] private ICondition _rightCondition;

        internal ConditionOr([NotNull] ICondition leftCondition, [NotNull] ICondition rightCondition)
        {
            _leftCondition = leftCondition ?? throw new ArgumentNullException(nameof(leftCondition));
            _rightCondition = rightCondition ?? throw new ArgumentNullException(nameof(rightCondition));
        }

        public bool IsTrue() => _leftCondition.IsTrue() || _rightCondition.IsTrue();

        private bool Equals(ConditionOr other) => _leftCondition.Equals(other._leftCondition) &&
                                                  _rightCondition.Equals(other._rightCondition);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConditionOr)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_leftCondition.GetHashCode() * 397) ^ _rightCondition.GetHashCode();
            }
        }

        public static bool operator ==(ConditionOr left, ConditionOr right) => Equals(left, right);
        public static bool operator !=(ConditionOr left, ConditionOr right) => !Equals(left, right);
    }
}