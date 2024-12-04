using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StarterKit.Models;
using StarterKit.Services;

namespace StarterKit.Services
{
    public interface IAdminStorage
    {
        Task<Admin?> Find(int adminId);  // Verander Guid naar int
        Task<bool> Create(Admin admin);
        Task<bool> Delete(int adminId);  // Verander Guid naar int
        Task<List<Admin>> GetAll();
    }

    public class InMemoryAdminStorage : IAdminStorage
    {
        private readonly DbContext _db;
        private readonly List<Admin> _admins;

        public InMemoryAdminStorage(DbContext db)
        {
            _db = db;
            _admins = new List<Admin>();
        }

        public async Task<bool> Create(Admin admin)
        {
            // Gebruik hier geen Guid, aangezien AdminId een int is
            // Je kunt een automatisch gegenereerde waarde van de database gebruiken
            _admins.Add(admin);

            await _db.Set<Admin>().AddAsync(admin);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int adminId)
        {
            var adminToRemove = _admins.FirstOrDefault(a => a.AdminId == adminId);
            if (adminToRemove == null) return false;

            // Remove from both in-memory list and database
            _admins.RemoveAll(a => a.AdminId == adminId);
            _db.Set<Admin>().Remove(adminToRemove);

            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<Admin?> Find(int adminId)
        {
           
            var admin = _admins.FirstOrDefault(a => a.AdminId == adminId);
            if (admin != null)
            {
                return admin;
            }

           
            return await _db.Set<Admin>().FindAsync(adminId);
        }

        public async Task<List<Admin>> GetAll()
        {
            return await Task.FromResult(_admins);
        }
    }
}
