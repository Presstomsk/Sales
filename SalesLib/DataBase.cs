using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.IO;
using System;

namespace SalesLib
{
    public class DataBase
    {
        private const string CONN_STR = "Server=mysql60.hostland.ru;Database=host1323541_sbd17;Uid=host1323541_itstep;Pwd=269f43dc;";
        private MySqlConnection db;
        private MySqlCommand command;

        public DataBase()
        {
            db = new MySqlConnection(CONN_STR);
            command = new MySqlCommand { Connection = db };
        }

        //public void Open() => db.Open();
        public void Open()
        {
            db.Open();
        }

        public void Close() => db.Close();

        public List<Product> GetProducts()
        {
            Open();
            var list = new List<Product>();

            var sql = "SELECT id, name, price FROM tab_products;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = res.GetString("name");
                var price = res.GetUInt32("price");
                list.Add(new Product { Id = id, Name = name, Price = price });
            }

            Close();
            return list;
        }

        public uint GetProductCount(uint id)
        {
            Open();

            var sql = @$"SELECT count
                        FROM tab_products_stock
                        JOIN tab_products 
                            ON tab_products_stock.product_id = tab_products.id
                        WHERE product_id = {id};";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return 0;

            res.Read();
            var count = res.GetUInt32("count");

            Close();

            return count;
        }

        public List<Buyer> GetBuyers()
        {
            var list = new List<Buyer>();

            Open();

            var sql = @"SELECT tab_buyers.id, first_name, last_name, discount
                        FROM tab_buyers
                        JOIN tab_people 
                            ON tab_buyers.people_id = tab_people.id
                        JOIN tab_discounts 
                            ON tab_buyers.discount_id = tab_discounts.id;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var name = $"{res.GetString("first_name")} {res.GetString("last_name")}";
                var discount = res.GetUInt32("discount");

                list.Add(new Buyer { Id = id, Name = name, Discount = discount });
            }
            Close();
            return list;
        }

        public List<Orders> GetOrders()
        {
            Open();
            var list = new List<Orders>();

            var sql = "SELECT id, buyer_id, seller_id, date, product_id, amount, total_price FROM tab_orders;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var buyer_id = res.GetUInt32("buyer_id");
                var seller_id = res.GetUInt32("seller_id");
                var date = res.GetString("date");
                var product_id = res.GetUInt32("product_id");
                var amount = res.GetUInt32("amount");
                var total_price = res.GetUInt32("total_price");

                list.Add(new Orders { Id = id, BuyerId = buyer_id, SellerId = seller_id, Date = date, ProductId = product_id, Amount = amount, TotalPrice = total_price });
            }

            Close();
            return list;
        }

        public List<People> GetPeople()
        {
            Open();
            var list = new List<People>();

            var sql = "SELECT id, first_name, last_name, phone;";
            command.CommandText = sql;
            var res = command.ExecuteReader();
            if (!res.HasRows) return null;

            while (res.Read())
            {
                var id = res.GetUInt32("id");
                var first_name = res.GetString("first_name");
                var last_name = res.GetString("last_name");
                var phone = res.GetString("phone");

                list.Add(new People { Id = id, FirstName = first_name, LastName = last_name, Phone = phone });
            }

            Close();
            return list;
        }

        public void AddOrders(Orders nextOrder)
        {
            Open();

            var sql = @$"INSERT INTO tab_orders (buyer_id, seller_id, date, product_id, amount, total_price) 
                        VALUES ({nextOrder.BuyerId}, {nextOrder.SellerId}, {nextOrder.Date}, {nextOrder.ProductId}, {nextOrder.Amount}, {nextOrder.TotalPrice});";
            command.CommandText = sql;
            command.ExecuteReader();

            Close();
        }

        public void OrdersExport(List<Orders> orders, string ordersExportPath)
        {
            using (StreamWriter file = new StreamWriter (ordersExportPath, false))
            {
                foreach (var order in orders)
                {
                    file.WriteLine($"{order.BuyerId}|{order.SellerId}|{order.BuyerId}|{order.Date}|{order.ProductId}|{order.Amount}|{order.TotalPrice};");
                }
            }       
        }

        public void PeopleImport()
        {

        }
        public void PeopleExport(List<People> people, string peopleExportPath)
        {
            using (StreamWriter file = new StreamWriter(peopleExportPath, false))
            {
                foreach (var person in people)
                {
                    file.WriteLine($"{person.Id}|{person.FirstName}|{person.LastName}|{person.Phone};");
                }
            }
        }
    }
}