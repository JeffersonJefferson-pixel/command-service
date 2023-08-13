using CommandService.Models;

namespace CommandService.Data
{
  public interface ICommandRepo
  {
    bool SaveChanges();

    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform plat);
    bool PlatformExists(int platformId);

    IEnumerable<Command> GetCommandsForPlatforms(int platformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
  }
}