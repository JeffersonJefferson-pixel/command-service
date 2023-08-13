using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;

namespace CommandService.EventProcessing
{
  public class EventProcessor : IEventProcessor
  {
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(
      IServiceScopeFactory scopeFactory,
      IMapper mapper)
    {
      _scopeFactory = scopeFactory;
      _mapper = mapper;
    }

    public void ProcessEvent(string message)
    {
      var eventType = DetermineEvent(message);

      switch (eventType)
      {
        case EventType.PlatformPublish:

          break;
        default:
          break;
      }
    }

    private EventType DetermineEvent(string message)
    {
      Console.WriteLine("--> Determining Event");

      var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

      switch (eventType.Event)
      {
        case "Platform_Publish":
          Console.WriteLine("--> Platform Publish Event detected");
          return EventType.PlatformPublish;
        default:
          Console.WriteLine("--> Could not determine event type");
          return EventType.Undetermined;
      }
    }

    private void AddPlatform(string platformPublishMessage)
    {
      using (var scope = _scopeFactory.CreateScope())
      {
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

        var platformPublishDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishMessage);

        try
        {
          var plat = _mapper.Map<Platform>(platformPublishDto);
          if (!repo.ExternalPlatformExists(plat.ExternalId))
          {
            repo.CreatePlatform(plat);
            repo.SaveChanges();
          }
          else
          {
            Console.WriteLine("--> Platform already exists...");
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"--> Could not add Platform to DB {ex.Message}");
        }
      }
    }
  }

  enum EventType
  {
    PlatformPublish,
    Undetermined
  }
}