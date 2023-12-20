using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ShopCart
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items {  get; set; }  

        public  ShoppingCart() { 
           this.Items = new List<ShoppingCartItem>();
        }

        public int count { get; set; }

        public decimal Tong
        {
            get { return Items.Sum(x => x.TotalPrice); }
            set { }
        }
        public decimal TongAll { get;set; }

        public void AddToCart(ShoppingCartItem item ,int quantity)
        {
            var checkExitst = Items.FirstOrDefault(x=>x.ProductId == item.ProductId);
            if (checkExitst != null)
            {
                checkExitst.Quantity += quantity;
                checkExitst.TotalPrice = checkExitst.Price * checkExitst.Quantity;
                checkExitst.AllPrice = checkExitst.TotalPrice;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void Remove(int id ) {
            var checkExitst = Items.FirstOrDefault(x=>x.ProductId==id);
            if (checkExitst != null)
            {
                Items.Remove(checkExitst);
             
            }
         
        }

        public void UpdateQuantity(int id,int quantity)
        {
            var checkExits = Items.SingleOrDefault(x=>x.ProductId==id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
                checkExits.AllPrice = checkExits.TotalPrice;
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x=>x.TotalPrice);

        }
        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }


    public class ShoppingCartItem
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Alias { get; set; }

        public string CategoryName { get; set; }

        public string ProductImg { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; } // giá

        public decimal TotalPrice { get; set; } //giá cộng lại


        public decimal AllPrice { get; set; } //giá cộng lại vs mã giảm giá
    }
}
