using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodCat.Conditions
{
    internal class ConditionDebug : ICondition
    {
        internal static ConditionDebug GetTmp([NotNull] ICondition condition, string name, Action<string, bool> debugAction = null)
        {
            _ConditionDebug._condition = condition;
            _ConditionDebug._name = name;
            _ConditionDebug._debugAction = debugAction;
            return _ConditionDebug;
        }
        private static ConditionDebug _ConditionDebug { get; } = new ConditionDebug(Condition.True(), "");
        
        [NotNull] private string _name;
        [NotNull] private ICondition _condition;
        private Action<string, bool> _debugAction;
        public ConditionDebug([NotNull] ICondition condition, string name, Action<string, bool> debugAction = null)
        {
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
            _name = name;
            if (debugAction == null)
            {
                debugAction = DebugActionDefault;
            }
            SetDebugAction(debugAction);
        }

        public bool IsTrue()
        {
            var result = _condition.IsTrue();
            _debugAction.Invoke(_name, result);
            return result;
        }

        /// <summary>
        /// string - Name
        /// bool - Result
        /// </summary>
        /// <param name="action"></param>
        private void SetDebugAction(Action<string, bool> action) => _debugAction = action;

        private void DebugActionDefault(string name, bool result) => Debug.Log($"{_name} - {result}");

        private bool Equals(ConditionDebug other) => _name == other._name && _condition.Equals(other._condition) &&
                                                     Equals(_debugAction, other._debugAction);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ConditionDebug) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _name.GetHashCode();
                hashCode = (hashCode * 397) ^ _condition.GetHashCode();
                hashCode = (hashCode * 397) ^ (_debugAction != null ? _debugAction.GetHashCode() : 0);
                return hashCode;
            }
        }
        
        public static bool operator ==(ConditionDebug left, ConditionDebug right) => Equals(left, right);
        public static bool operator !=(ConditionDebug left, ConditionDebug right) => !Equals(left, right);
    }
}