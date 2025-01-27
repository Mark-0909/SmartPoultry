using SmartPoultry.DataAccess;
using SmartPoultry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SmartPoultry.App;

namespace SmartPoultry.DataServices
{
    public class DeliveriesServices
    {
        private readonly AppDbContext _context;
        MainWindow mainWindow = UserContext.mainWindow;
        public DeliveriesServices(AppDbContext context)
        {
            _context = context;
        }

        public void UpdatePaymentStatus(int id)
        {
            var delivery = _context.Deliveries.FirstOrDefault(x => x.Id == id);
            delivery.payment_status = "paid";
            _context.SaveChanges();
        }
        public List<Deliveries> GetDeliveriesList()
        {
            try
            {
                var deliveris = _context.Deliveries.OrderByDescending(x => x.added_date).ToList();
                return deliveris;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Deliveries>();
            }
        }
        public bool MarkAsVoided(long receiptid, string remarks)
        {
            try
            {
                var delivery = _context.Deliveries.FirstOrDefault(p => p.order_id == receiptid);

                if (delivery == null)
                {
                    return true;
                }

                delivery.delivery_status = "voided";
                delivery.Remarks = remarks;


                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public Deliveries GetByReceiptId(long Id)
        {
            var itemrow = _context.Deliveries.FirstOrDefault(x => x.order_id == Id);
            return itemrow;
        }
        public bool UpdateDelivery(int id, string name, string address, string type, DateTime date, decimal price, string contacts, decimal charge, string remarks)
        {
            try
            {
                var delivery = _context.Deliveries.FirstOrDefault(x => x.Id == id);
                delivery.name = name;
                delivery.address = address;
                delivery.type = type;
                delivery.delivery_date = date;
                delivery.price = price;
                delivery.contact_no = contacts;
                delivery.charges = charge;
                delivery.Remarks = remarks;

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }   
        }
        public bool MarkAsPaid(long orderid, string remarks)
        {
            try
            {
                var itemrow = _context.Deliveries.FirstOrDefault(p => p.order_id == orderid);
                itemrow.payment_status = "paid";
                itemrow.Remarks = remarks;
                _context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool UpdateDelivered(int id, string deliveryman, string remarks)
        {
            try
            {
                var row = _context.Deliveries.FirstOrDefault(x => x.Id == id);
                row.delivery_status = "delivered";
                row.delivery_man = deliveryman;
                row.Remarks = remarks;
                row.delivery_date = DateTime.Now;
                _context.SaveChanges();

                return true;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public Deliveries GetById(int Id)
        {
            var itemrow = _context.Deliveries.FirstOrDefault(x => x.Id == Id);
            return itemrow;
        }


        public List<Deliveries> GetList(string filter) {
            _context.ChangeTracker.Clear();
            List<Deliveries> list = _context.Deliveries.Where(p => p.delivery_status != "delivered" && p.delivery_status != "voided" && p.type == filter).OrderBy(p => p.delivery_date).ToList();
            return list;
        }
        public int CountDeliveries()
        {
            try
            {
                DateTime dateTime = DateTime.Now;

                int count = _context.Deliveries.Count(p =>
                    p.delivery_date <= dateTime &&
                    p.type == "To Deliver" &&
                    p.delivery_status == "pending" 
                );

                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }


        public bool Create(long orderid, string name, string type, decimal price, string address, string status, string contact, DateTime deliverydate, string deliveryman, decimal charge)
        {
            try 
            {
                var newDelivery = new Deliveries() {
                    order_id = orderid,
                    name = name,
                    type = type.Trim(),
                    price = price,
                    address = address,
                    payment_status = status,
                    delivery_status = "pending",
                    contact_no = contact,
                    delivery_date = deliverydate,
                    delivery_man = deliveryman,
                    added_date = DateTime.Now,
                    charges = charge,
                    employee_incharge = 1,
                    Remarks = "Add New Delivery."
                };
                _context.Deliveries.Add(newDelivery);
                _context.SaveChanges();

                mainWindow.recordsControl.DeliveryControl.DisplayDeliveries();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                return false;
            }
            
        }
    }
}
