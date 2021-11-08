# Conditions

Light encapsulation and composition of conditions.

> Tested on unity 2020.3 and contains assembly definition for compiling to separate assembly file for performance reason.

# Installation
## As unity module
This repository can be installed as unity module directly from git url. In this way new line should be added to `Packages/manifest.json`:
```
"com.goodcat.conditions": "https://github.com/GoodCatGames/Conditions.git"
```

## As source
If you can't / don't want to use unity modules, code can be downloaded as sources archive from `Releases` page.

# Integration to startup

1. To create a condition it is enough to implement the ICondition interface
```csharp
interface ICondition
{
    bool IsTrue();
}
```

2. Simple conditions
```csharp
var condition = Condition.Get(() => ScoreGame >= 1_000);
```

```csharp
void Update()
{
   // The call `Condition.Get(Func<bool> func)` is cached.
   // There is only one allocation the first time Update() is called.
   var condition = Condition.Get(() => hp >= 10);
}
```

3. Complex conditions
```csharp
public class ConditionTargetInRange : ICondition
{
   private readonly IUnit _unit;

   public ConditionTargetInRange(IUnit unit) => _unit = unit;

   public bool IsTrue() => (_unit.TargetCurrent.position - _unit.Owner.position).sqrMagnitude < _unit.Range * _unit.Range;
}

interface IUnit
{
    Transform Owner { get; }
    Transform TargetCurrent { get; }
    float Range { get; }
}
```

4. Composition
```csharp
// (!A || B) && C
var condition 
       = new ConditionTargetInRange(unit).Not() // condition A
        .Or(Condition.Get(() => ScoreGame >= 1_000))    // condition B
        .And(Condition.Get(() => hp >= hp / 2));        // condition C
        
// !A || (B && C)
condition 
       = new ConditionTargetInRange(unit).Not()  // condition A               
        .Or(
              Condition.Get(() => ScoreGame >= 1_000)// condition B
              .And(Condition.Get(() => hp >= hp / 2))// condition C
           );
```
5. Debug
```csharp
var condition = 
    conditionDamageWasReceived.Debug("DamageWasReceived")
    .Or(conditionTargetInRange.Debug("TargetInRange"))
    .Or(conditionTimer.Debug("Timer", DebugCustomAction)));

 void DebugCustomAction(string name, bool result)
 {
     if (result == false)
         return;
     UnityEngine.Debug.Log($"{name} is true");
 }
```
```csharp
// Unity Log
DamageWasReceived - False
TargetInRange - False
Timer is true
```
```csharp
/// <summary>
/// Action<string, bool>
/// string - Name
/// bool - Result
/// </summary>
condition.Debug(string name, Action<string, bool> debugAction = null)
```
