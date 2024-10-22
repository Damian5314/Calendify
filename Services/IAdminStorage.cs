using StarterKit.Models;

public interface IAdminStorage
{
    Task<bool> Create(Admin admin);
    Task<bool> Delete(Guid adminId);
    Task<Admin?> Find(Guid adminId);
    Task<List<Admin>> FindMany(Guid[] adminIds);
}
