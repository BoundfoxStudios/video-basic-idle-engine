namespace IdleEngine
{
  public static class NumberFormatter
  {
    public static string InScientificNotation(this double value)
    {
      return value < 1000 ? value.ToString("f") : value.ToString("e2");
    }
  }
}
