namespace CommandService.EventProcessing
{
  public interface IEventProcessor
  {
    void ProcessEvent(String message);
  }
}