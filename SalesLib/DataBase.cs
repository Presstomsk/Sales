using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace SalesLib
{
    public class DataBase
    {
        private MySqlConnection db;
        private MySqlCommand command;

        public DataBase()
        {
            var connectionString = ConnectionString.Init(@"db_connect.ini");
            db = new MySqlConnection(connectionString);
            command = new MySqlCommand { Connection = db };
        }

        public void Open() => db.Open();

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
                list.Add(new Product {Id = id, Name = name, Price = price});
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
                
                list.Add(new Buyer {Id = id, Name = name, Discount = discount});
            }
            
            Close();

            return list;
        }

        public void ExportProductsToCSV(string path)
        {
            var products = GetProducts();

            using var file = new StreamWriter(path, append: false);
            foreach (var product in products)
            {
                file.WriteLine($"{product.Id}|{product.Name}|{product.Price}");
            }
        }

        public void ImportProductsFromCSV(string path)
        {
            var products_csv = new List<Product>();
            
            using var file = new StreamReader(path);
            
            var line = string.Empty;
            while ((line = file.ReadLine()) != null)
            {
                var temp = line.Split('|');
                var product = new Product
                {
                    Id = uint.Parse(temp[0]),
                    Name = temp[1],
                    Price = uint.Parse(temp[2])
                };
                products_csv.Add(product);
            }

            //TODO Переписать проверку на уникальность
            /*var products_db = GetProducts();
            var products = new List<Product>();
            uint i = 1; 
            foreach (var product_db in products_db)
            {
                foreach (var product_csv in products_csv)  
                {
                    if (product_db.Name == product_csv.Name) continue; 
                    
                    products.Add(new Product {Id = i, Name = product_csv.Name, Price = product_csv.Price});
                    i++;
                }
            }*/
            
            Open();
            foreach (var product in products_csv)
            {
                var sql = $"INSERT INTO host1323541_sbd10.tab_products (name, price) VALUES ('{product.Name}', {product.Price});";
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            Close();
        }
    }
}