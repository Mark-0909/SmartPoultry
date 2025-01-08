using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartPoultry.Models;
using SmartPoultry.DataServices;
using SmartPoultry.DataAccess;
using static SmartPoultry.App;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;

namespace SmartPoultry
{
    /// <summary>
    /// Interaction logic for Sales_OrderInfo.xaml
    /// </summary>
    public partial class Sales_OrderInfo : Window
    {
        public AppDbContext context = new AppDbContext();
        public ProductServices productServices;
        public ProductVariationServices productVariationServices;
        public UserServices userServices;
        public SalesServices salesServices;
        MainWindow mainWindow;
        Sales sale;
        Add_FinancialLiabilities financeform;
        Add_Delivery deliveryForm;

        string openedby = "default";
        public Sales_OrderInfo(Sales sales, MainWindow window)
        {
            InitializeComponent();

            ToInitialize(sales, window);
        }

        public Sales_OrderInfo(Sales sales, MainWindow window, string openedBY, Add_FinancialLiabilities paymentform)
        {
            InitializeComponent();

            ToInitialize(sales, window);

            openedby = openedBY;

            financeform = paymentform;
        }
        public Sales_OrderInfo(Sales sales, MainWindow window, string openedBY, Add_Delivery deliveryform)
        {
            InitializeComponent();

            ToInitialize(sales, window);

            openedby = openedBY;

            deliveryForm = deliveryform;
        }
        public void ToInitialize(Sales sales, MainWindow window)
        {

            sale = sales;
            userServices = new UserServices(context);

            OrderIdLabel.Content = sales.receipt_id;
            totalPricelabel.Content = sales.total_price.ToString("N2");
            PayMethodLabel.Content = sales.payment_mode.ToString().ToUpper();
            PaymentStatusLabel.Content = sales.status.ToString().ToUpper();
            PurchaseMethodlabel.Content = sales.purchase_method.ToString().ToUpper();
            PurchaseDatelabel.Content = sales.purchase_date.ToString();

            CashierLabel.Content = userServices.GetUser(sales.employee_incharge).Username.ToString();

            productServices = new ProductServices(context);
            productVariationServices = new ProductVariationServices(context);
            salesServices = new SalesServices(context);

            List<string> productvarids = sales.product_list.Split(',').ToList();
            List<string> pricelist = sales.price_list.Split(',').ToList();
            List<string> qtylist = sales.quantity_list.Split(',').ToList();
            List<string> varlist = sales.variation_list.Split(',').ToList();
            List<string> prodname = new List<string>();
            for (int i = 0; i < productvarids.Count; i++)
            {
                int prodid = productVariationServices.GetProductVariationById(int.Parse(productvarids[i])).product_id;
                string name = productServices.FetchProduct(prodid).product_name;
                prodname.Add(name);
            }
            GenerateList(productvarids, qtylist, varlist, pricelist, prodname);

            mainWindow = UserContext.mainWindow;
        } 


        public void GenerateList(List<string> prodvarid, List<string> qty, List<string> varSpec, List<string> priceList, List<string> prodname)
        {
            OrderWPanel.Children.Clear();

            for (int i = 0; i < prodvarid.Count; i++)
            {
                Border orderBorder = new Border
                {
                    BorderBrush = Brushes.Transparent,
                    BorderThickness = new Thickness(1),
                    Height = 35,
                    Width = 255
                };

                WrapPanel wrapPanel = new WrapPanel();

                Label itemNameLabel = new Label
                {
                    Content = $"({varSpec[i]}) {prodname[i]}",
                    Height = 33,
                    Width = 126,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                Label qtyLabel = new Label
                {
                    Content = qty[i],
                    Height = 33,
                    Width = 43,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };

                string formattedPrice = decimal.TryParse(priceList[i], out decimal price)
                    ? price.ToString("N2")
                    : "Invalid";

                Label priceLabel = new Label
                {
                    Content = formattedPrice,
                    Height = 33,
                    Width = 83,
                    HorizontalContentAlignment = HorizontalAlignment.Right,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Background = Brushes.Transparent
                };
                wrapPanel.Children.Add(itemNameLabel);
                wrapPanel.Children.Add(qtyLabel);
                wrapPanel.Children.Add(priceLabel);

                orderBorder.Child = wrapPanel;

                OrderWPanel.Children.Add(orderBorder);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            if(openedby == "delivery")
            {
                deliveryForm.ActiveOverlay(false);
                this.Close();
                return;
            }
            if (openedby == "payment")
            {
                financeform.ActiveOverlay(false);
                this.Close();
                return;
            }
            mainWindow.ActiveOverlay(false);
            this.Close();
        }

        private void VoidBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void GenerateReceiptBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayReceipt(sale.receipt_id, salesServices, context);
        }
        
        public static void DisplayReceipt(long salesid, SalesServices salesServices, AppDbContext context)
        {
            try
            {
                Sales sales = salesServices.GetSales(salesid);
                ProductServices productServices = new ProductServices(context);
                ProductVariationServices productVariationServices = new ProductVariationServices(context);
                UserServices userServices = new UserServices(context);
                DeliveriesServices deliveriesServices = new DeliveriesServices(context);



                int employeeId = UserContext.CurrentUserId;
                string employeename = userServices.GetUser(employeeId).Username;

                List<string> itemid = sales.product_list.Split(',').ToList();
                List<string> pricelist = sales.price_list.Split(',').ToList();
                List<string> quantitylist = sales.quantity_list.Split(',').ToList();
                List<string> varlist = sales.variation_list.Split(',').ToList();

                int itemCount = Math.Min(Math.Min(itemid.Count, pricelist.Count), Math.Min(quantitylist.Count, varlist.Count));
                itemid = itemid.Take(itemCount).ToList();
                pricelist = pricelist.Take(itemCount).ToList();
                quantitylist = quantitylist.Take(itemCount).ToList();
                varlist = varlist.Take(itemCount).ToList();

                List<string> itemnames = new List<string>();
                List<string> originalprice = new List<string>();
                List<string> totalPrices = new List<string>();

                float heightcalculation = 75 + (3 * itemid.Count);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    float width = 30f * 2.83465f;
                    float height = heightcalculation * 2.83465f;
                    iTextSharp.text.Rectangle pageSize = new iTextSharp.text.Rectangle(width, height);

                    Document doc = new Document(pageSize, 5f, 5f, 5f, 5f);
                    PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
                    writer.CloseStream = false;

                    doc.Open();
                    Font font = new Font(Font.FontFamily.HELVETICA, 3.8f, Font.NORMAL);
                    Font font2 = new Font(Font.FontFamily.HELVETICA, 3f, Font.NORMAL);
                    Font font3 = new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLD);
                    Font font4 = new Font(Font.FontFamily.HELVETICA, 3.3f, Font.NORMAL);
                    Font font5 = new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLD);
                    // Receipt header
                    doc.Add(new iTextSharp.text.Paragraph($"{sales.status.ToUpper()}", font3) { Alignment = Element.ALIGN_RIGHT });

                    string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "blacklogo.png");
                    string textPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "blacktext.png");

                    if (File.Exists(logoPath))
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                        logo.ScaleToFit(40f, 15f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(logo);
                    }
                    if (File.Exists(textPath))
                    {
                        iTextSharp.text.Image text = iTextSharp.text.Image.GetInstance(textPath);
                        text.ScaleToFit(40f, 15f);
                        text.Alignment = Element.ALIGN_CENTER;
                        doc.Add(text);
                    }

                    doc.Add(new iTextSharp.text.Paragraph($"Palo Alto, Calamba, Laguna Philippines", font2) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new iTextSharp.text.Paragraph($"+63 1234567890", font2) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new iTextSharp.text.Paragraph($"gabmigspoultrysupplies@gmail.com", font2) { Alignment = Element.ALIGN_CENTER });
                    doc.Add(new iTextSharp.text.Paragraph("-----------------------------------------------------------", font));

                    doc.Add(new iTextSharp.text.Paragraph($"Order ID: {sales.receipt_id}", font));
                    doc.Add(new iTextSharp.text.Paragraph($"Cashier: {employeename}", font));
                    doc.Add(new iTextSharp.text.Paragraph($"Payment Mode: {sales.payment_mode.ToUpper()}", font));
                    doc.Add(new iTextSharp.text.Paragraph("-----------------------------------------------------------", font));
                    // Generate receipt items
                    for (int i = 0; i < itemid.Count; i++)
                    {
                        try
                        {

                            int id = int.Parse(itemid[i]);
                            ProductVariations prodvar = productVariationServices.GetProductVariationById(id);
                            Products prod = productServices.FetchProduct(prodvar.product_id);

                            itemnames.Add($"({varlist[i]}) {prod.product_name}");
                            decimal quantity = decimal.Parse(quantitylist[i]);
                            decimal totalPrice = decimal.Parse(pricelist[i]);
                            decimal initialPrice = totalPrice / quantity;

                            originalprice.Add($"{initialPrice:N2}");
                            totalPrices.Add($"{totalPrice:N2}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error processing item ID {itemid[i]}: {ex.Message}");
                        }
                    }

                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 1.5f, 2.8f, 2.5f, 3f });

                    table.AddCell(new PdfPCell(new Phrase("Qty", font4)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    table.AddCell(new PdfPCell(new Phrase("Items", font)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    table.AddCell(new PdfPCell(new Phrase("Price", font)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                    table.AddCell(new PdfPCell(new Phrase("Total", font)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });

                    for (int i = 0; i < itemnames.Count; i++)
                    {
                        table.AddCell(new PdfPCell(new Phrase(quantitylist[i], font)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
                        table.AddCell(new PdfPCell(new Phrase(itemnames[i], font)) { HorizontalAlignment = Element.ALIGN_LEFT, Border = 0 });
                        table.AddCell(new PdfPCell(new Phrase(originalprice[i], font)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                        table.AddCell(new PdfPCell(new Phrase(totalPrices[i], font)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    }

                    doc.Add(table);

                    if (sales.purchase_method == "to deliver" || sales.purchase_method == "delivered")
                    {
                        decimal charge = deliveriesServices.GetByReceiptId(sales.receipt_id).charges;
                        PdfPTable chargeTable = new PdfPTable(2);
                        chargeTable.WidthPercentage = 100;
                        chargeTable.SetWidths(new float[] { 3f, 3f });

                        chargeTable.AddCell(new PdfPCell(new Phrase("Delivery Charge", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                        chargeTable.AddCell(new PdfPCell(new Phrase($"{charge.ToString("N2")}", font)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                        doc.Add(new iTextSharp.text.Paragraph(" ", font));
                        doc.Add(chargeTable);
                    }



                    PdfPTable totalTable = new PdfPTable(2);
                    totalTable.WidthPercentage = 100;
                    totalTable.SetWidths(new float[] { 3f, 3f });

                    totalTable.AddCell(new PdfPCell(new Phrase("TOTAL:", font5)) { Border = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    totalTable.AddCell(new PdfPCell(new Phrase($"{sales.total_price:N2}", font5)) { Border = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    doc.Add(new iTextSharp.text.Paragraph(" ", font4));
                    doc.Add(totalTable);

                    doc.Add(new iTextSharp.text.Paragraph(" ", font4));
                    doc.Add(new iTextSharp.text.Paragraph($"Date: {DateTime.Now:yyyy-MM-dd}", font));
                    doc.Add(new iTextSharp.text.Paragraph($"Purchase Method: {sales.purchase_method.ToUpper()}", font));

                    if (sales.purchase_method == "to deliver" || sales.purchase_method == "delivered")
                    {
                        doc.Add(new iTextSharp.text.Paragraph($"Address: {deliveriesServices.GetByReceiptId(sales.receipt_id).address}", font));
                    }

                    doc.Add(new iTextSharp.text.Paragraph("-----------------------------------------------------------", font));

                    var thanksParagraph2 = new iTextSharp.text.Paragraph($"Thank you! Please come again!", font2);
                    thanksParagraph2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(thanksParagraph2);

                    var storeParagraph2 = new iTextSharp.text.Paragraph($"GabMig's SmartPoultry", font2);
                    storeParagraph2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(storeParagraph2);

                    var addressParagraph2 = new iTextSharp.text.Paragraph($"Palo Alto, Calamba, Laguna Philippines", font2);
                    addressParagraph2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(addressParagraph2);


                    var phoneParagraph2 = new iTextSharp.text.Paragraph($"+63 1234567890", font2);
                    phoneParagraph2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(phoneParagraph2);


                    var emailParagraph2 = new iTextSharp.text.Paragraph($"gabmigspoultrysupplies@gmail.com", font2);
                    emailParagraph2.Alignment = Element.ALIGN_CENTER;
                    doc.Add(emailParagraph2);

                    doc.Close();
                    memoryStream.Position = 0;

                    string tempFilePath = System.IO.Path.Combine(
                        System.IO.Path.GetTempPath(),
                        $"{DateTime.Now:yyyyMMddHHmmss}.pdf"
                    );
                    File.WriteAllBytes(tempFilePath, memoryStream.ToArray());

                    Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true });



                    Task.Run(() =>
                    {
                        Thread.Sleep(5000);
                        if (File.Exists(tempFilePath))
                        {
                            File.Delete(tempFilePath);
                        }
                    });

                }


            }
            catch (Exception e)
            {
                MessageBox.Show($"Error generating receipt: {e.Message}");
            }
        }

        
    }
}
