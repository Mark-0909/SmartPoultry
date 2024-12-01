using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using static SmartPoultry.App;
using iTextSharp.text.pdf.parser.clipper;

namespace SmartPoultry.DataServices
{
    public class UserServices
    {
        private readonly AppDbContext _context;
        
        

        public UserServices(AppDbContext context)
        {
            _context = context;
        }

        public bool UpdatePassword(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(p => p.Username == username);
            if (user == null) 
            {
                return false;
            }

            string hashnewpass = HashValue(password);
            user.Password = hashnewpass; 
            _context.SaveChanges(); 
            return true;
        }

        public bool ForgotPassVerification(string username, int question, string answer)
        {
            User user = _context.Users.FirstOrDefault(p => p.Username == username);
            if (user == null)
            {
                return false; 
            }

            string hashanswer = HashValue(answer);

            switch (question)
            {
                case 1:
                    if (hashanswer != user.Q1) return false;
                    break;
                case 2:
                    if (hashanswer != user.Q2) return false;
                    break;
                case 3:
                    if (hashanswer != user.Q3) return false;
                    break;
                default:
                    return false; 
            }

            return true;
        }




        public bool LoginVerification(string username, string password)
        {
            User user = _context.Users.FirstOrDefault(p => p.Username == username);

            if (user == null)
            {
                return false;
            }
            string hashvalpass = HashValue(password);

            if (user.Password != hashvalpass)
            {
                return false;
            }

            UserContext.CurrentUserId = user.Id;
            return true;        
        }

        public bool IsThereAdmin()
        {
            try 
            { 
                bool isPresent = _context.Users.Any(p => p.Role == "admin");
                return isPresent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public bool IsUserNamePresent(string username)
        {
            try
            {
                bool isPresent = _context.Users.Any(p => p.Username == username);

                return isPresent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        public bool CreateAccount(string username, string password, string q1, string q2, string q3, string role)
        {
            try
            {
                var user = new User()
                {
                    Username = username,
                    Password = HashValue(password),
                    Q1 = HashValue(q1),  
                    Q2 = HashValue(q2),
                    Q3 = HashValue(q3),
                    Role = role,
                    Status = "active"
                };

                
                _context.Users.Add(user);
                _context.SaveChanges();

                return true;  
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"An error occurred while creating the account: {ex.Message}");
                return false;  
            }
        }

        private string HashValue(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hashedBytes);
            }
        }

    }
}
