using System;

public struct Maybe<T>
{
  public bool HasValue { get; }
  public T Value { get; }
  public Maybe(T value)
  {
    HasValue = true;
    Value = value;
  }

  public static Maybe<T> None => new Maybe<T>();
  public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);
  public static implicit operator T(Maybe<T> maybe) => maybe.Value;
  public T GetValueOrDefault(T defaultValue) => HasValue ? Value : defaultValue;
  public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) => HasValue ? some(Value) : none();
}
