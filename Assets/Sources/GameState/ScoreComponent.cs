using System.Xml;
using Entitas;
using Entitas.CodeGeneration.Attributes;

//[GameState]
[Unique]
[Event(EventTarget.Any)]
public sealed class ScoreComponent : IUniqueComponent //ValueComponent<ScoreComponent, int>
{
    public int value;

}

//public class ValueComponent<T2, T> : IValueComponent<T> where T2 : IValueComponent<T>
//{
//    public T value;

//    public static implicit operator this(T value)
//    {
//        return new ValueComponent<T> { value = value };
//    }

//    public static T2 ToComponent()
//    {
//        return new T2().SetValue(value);
//    }

//    public void SetValue(T v)
//    {
//        value = v;
//    }

//    public T GetValue()
//    {
//        return value;
//    }
//}

