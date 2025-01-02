using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartPoultry.DataServices
{
    public class UserLogsServices
    {
        AppDbContext _context;
        public UserLogsServices(AppDbContext context)
        {
            _context = context;
        }
        public List<UserLogs> GetList()
        {
            var list = _context.UserLogs.OrderBy(p => p.timestamp).ToList();
            return list;
        }
        public bool Create(int userId, string action)
        {
            try
            {
                var row = new UserLogs
                {
                    user_id = userId,
                    action = action,
                    timestamp = DateTime.Now
                };
                _context.Add(row);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
