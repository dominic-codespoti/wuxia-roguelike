using System;

public struct Maybe<T>
{
  public bool HasValue { get; }
  public T Value { get; }

  private Maybe(T value)
  {
    HasValue = true;
    Value = value;
  }

  private Maybe(bool hasValue)
  {
    HasValue = hasValue;
    Value = default;
  }

  public static Maybe<T> None => new Maybe<T>(false);
  public static Maybe<T> Some(T value) => new Maybe<T>(value);

  public static implicit operator Maybe<T>(T value) => new Maybe<T>(value);
  public static implicit operator T(Maybe<T> maybe) => maybe.Value;
  public T GetValueOrDefault(T defaultValue) => HasValue ? Value : defaultValue;
  public TOut Match<TOut>(Func<T, TOut> some, Func<TOut> none) => HasValue ? some(Value) : none();
  public Maybe<TOut> Map<TOut>(Func<T, TOut> mapper) => HasValue ? new Maybe<TOut>(mapper(Value)) : Maybe<TOut>.None;
}
