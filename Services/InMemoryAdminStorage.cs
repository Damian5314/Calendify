namespace StarterKit.Services;

using StarterKit.Models;
using StarterKit.Utils;
public class InMemoryAdminStorage : IAdminStorage
{
    public static List<Admin> admins = new();

    public DatabaseContext db;

    public InMemoryAdminStorage(DatabaseContext db)
    {
        this.db = db;
    }
    public async Task<bool> Create(Admin admin)
    {
        admin.AdminId = Guid.NewGuid();
        await Task.Delay(0);

        // adds admin to a list of admins? : Deze code was al in de template
        admins.Add(admin);

        await db.Admin.AddAsync(admin);
        if (await db.SaveChangesAsync() > 0) return true;
        return false;
    }

    public async Task<bool> Delete(Guid adminId)
    {
        await Task.Delay(0);
        admins.Remove(admins.Find(a => a.AdminId == adminId));
        if (await db.SaveChangesAsync() > 0) return true;
        return false;
    }

    public async Task<Admin?> Find(Guid adminId)
    {
        await Task.Delay(0);
        return admins.Find(a => a.AdminId == adminId);
    }

    public async Task<List<Admin>> FindMany(Guid[] adminIds)
    {
        List<Admin> found = new();
        await Task.Delay(0);

        admins.ForEach(a => 
        {
            adminIds.ToList().ForEach(id =>
            {
                if (a.AdminId == id)
                    found.Add(a);
            });
        });
        return found;
    }
}