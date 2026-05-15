using System.Data;
using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SqlConnectionFactory _factory;

        public ProductRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public List<Product> GetAll()
        {
            List<Product> products = new();

            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_GetProducts",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        SKU = reader["SKU"].ToString(),
                        ProductName = reader["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        MinStock = Convert.ToInt32(reader["MinStock"]),
                        SupplierId = Convert.ToInt32(reader["SupplierId"])
                    });
                }
            }

            return products;
        }

        public void Add(Product product)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_AddProduct",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@SKU", product.SKU);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@MinStock", product.MinStock);
                cmd.Parameters.AddWithValue("@SupplierId", product.SupplierId);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Product product)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_UpdateProduct",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductId", product.ProductId);

                cmd.Parameters.AddWithValue("@ProductName", product.ProductName);
                cmd.Parameters.AddWithValue("@SKU", product.SKU);

                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);

                cmd.Parameters.AddWithValue("@Price", product.Price);

                cmd.Parameters.AddWithValue("@MinStock", product.MinStock);

                cmd.Parameters.AddWithValue("@SupplierId", product.SupplierId);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_DeleteProduct",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ProductId", id);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }
        public Product GetById(int id)
        {
            Product product = null;

            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Products WHERE ProductId = @ProductId",
                    conn
                );

                cmd.Parameters.AddWithValue("@ProductId", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),

                        SKU = reader["SKU"].ToString(),

                        ProductName = reader["ProductName"].ToString(),

                        Quantity = Convert.ToInt32(reader["Quantity"]),

                        Price = Convert.ToDecimal(reader["Price"]),

                        MinStock = Convert.ToInt32(reader["MinStock"]),

                        SupplierId = Convert.ToInt32(reader["SupplierId"])
                    };
                }
            }

            return product;
        }
    }
}