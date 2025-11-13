using System;


//Define base token types

//Ask Token
public sealed class Ask<T>
{
    public readonly string Name;
    public Ask(string name = null) { Name = name; }
    public override string ToString() => Name ?? base.ToString();
}

//Act Token
public sealed class Act<TArg>
{
    public readonly string Name;
    public Act(string name = null) { Name = name; }
    public override string ToString() => Name ?? base.ToString();
}

//Reply Token
public sealed class Act<TArg, TResult>
{
    public readonly string Name;
    public Act(string name = null) { Name = name; }
    public override string ToString() => Name ?? base.ToString();
}