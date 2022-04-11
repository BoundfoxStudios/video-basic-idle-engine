namespace IdleEngine.SaveSystem
{
  public interface IRestorable<T>
  {
    T GetRestorableData();
    void SetRestorableData(T data);
  }
}
