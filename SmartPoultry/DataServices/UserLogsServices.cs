using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartPoultry.App;

namespace SmartPoultry.DataServices
{
    public class UserLogsServices
    {
        AppDbContext _context;
        MainWindow mainWindow = UserContext.mainWindow;
        public UserLogsServices(AppDbContext context)
        {
            _context = context;
        }
        public List<UserLogs> GetList()
        {
            var list = _context.UserLogs.OrderByDescending(p => p.timestamp).ToList();
            return list;
        }

        public List<UserLogs> GetListOfMember(int id)
        {
            var list = _context.UserLogs.Where(p => p.user_id == id).OrderByDescending(p => p.timestamp).ToList();
            return list;
        }
        public bool Create(int userId, string action, string remarks)
        {
            try
            {
                var row = new UserLogs
                {
                    user_id = userId,
                    action = action,
                    timestamp = DateTime.Now,
                    Remarks = remarks
                };
                _context.Add(row);
                _context.SaveChanges();

                mainWindow.recordsControl.LogsControl.FetchUserLogs();
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
