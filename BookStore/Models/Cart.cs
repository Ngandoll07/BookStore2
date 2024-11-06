using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Models
{
    public class CartItem
    {
        public Book _book { get; set; }
        public int quanTity { get; set; }
    }
    public class Cart
    {
        List<CartItem> items =new List<CartItem>();
        public IEnumerable<CartItem> Items 
        { 
            get {  return items; } 
        }
        public void Add_Product_Cart(Book book, int quantity = 1)
        {
            var item=Items.FirstOrDefault(s=>s._book.ID==book.ID);
            if (item == null)
                items.Add(new CartItem
                {
                    _book = book,
                    quanTity = quantity
                });
            else
            {
                item.quanTity += quantity;
            }
        }
        public int TotalQuantity()
        {
            return items.Sum(s => s.quanTity);
        }
        public decimal TotalMoney() 
        {
            var toTal=items.Sum(s=>s.quanTity*s._book.GIA);
            return(decimal)toTal;
        }
        public void UpDateQuantity(string ID,int newQuantity)
        {
            var item=items.Find(s=>s._book.ID == ID);
            if (item != null)
            {
                if (items.Find(s => s._book.StockQuantity >= newQuantity) != null)
                    item.quanTity = newQuantity;
                else item.quanTity = 1;
            }    
        }
        public void ReMoveItem(string ID)
        {
            items.RemoveAll(s=>s._book.ID==ID);
        }
        public void clearCart()
        { items.Clear(); }
    }
}