using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartPoultry.App;

namespace SmartPoultry.DataServices
{
    public class SupplierOrdersServices
    {
        public AppDbContext _context;
        public SupplierOrdersServices(AppDbContext context) 
        {
            _context = context;
        }
        public bool UpdatePrice(int id, decimal price)
        {
            try
            {
                var suppOrder = _context.SupplierOrders.FirstOrDefault(p => p.id == id);
                suppOrder.price = price;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public SupplierOrders GetById(int id)
        {
            try
            {
                var suppOrder = _context.SupplierOrders.FirstOrDefault(p => p.id == id);

                return suppOrder;
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
        public int Create(int supplierID, string productList, string productQTY, DateTime delivery_date)
        {
            try
            {
                SupplierOrders Order = new SupplierOrders()
                {
                    supplierID = supplierID,
                    productList = productList,
                    orderQty = productQTY,
                    Added_Date = DateTime.Now,
                    Delivery_Date = delivery_date,
                    employee_incharge = UserContext.CurrentUserId
                };
                _context.SupplierOrders.Add(Order);
                _context.SaveChanges();
                return Order.id;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
            
        }

    }
}
