using System.Data;
using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly SqlConnectionFactory _factory;

        public StockRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // =========================
        // IMPORT STOCK
        // =========================
        public void ImportStock(
            int productId,
            int quantity,
            string note
        )
        {
            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_ImportStock",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@ProductId",
                    productId
                );

                cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                );

                cmd.Parameters.AddWithValue(
                    "@Note",
                    note
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // EXPORT STOCK
        // =========================
        public void ExportStock(
            int productId,
            int quantity,
            string note
        )
        {
            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_ExportStock",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@ProductId",
                    productId
                );

                cmd.Parameters.AddWithValue(
                    "@Quantity",
                    quantity
                );

                cmd.Parameters.AddWithValue(
                    "@Note",
                    note
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // INVENTORY
        // =========================
        public List<Product> GetInventory()
        {
            List<Product> products = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_GetProducts",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId =
                            Convert.ToInt32(
                                reader["ProductId"]
                            ),

                        SKU =
                            reader["SKU"].ToString(),

                        ProductName =
                            reader["ProductName"].ToString(),

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]
                            ),

                        Price =
                            Convert.ToDecimal(
                                reader["Price"]
                            ),

                        MinStock =
                            Convert.ToInt32(
                                reader["MinStock"]
                            ),
                        SupplierId = 
                            Convert.ToInt32(
                                reader["SupplierId"]
                             )
                    });
                }
            }

            return products;
        }

        // =========================
        // LOW STOCK
        // =========================
        public List<Product> GetLowStock()
        {
            List<Product> products = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_LowStockReport",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId =
                            Convert.ToInt32(
                                reader["ProductId"]
                            ),

                        SKU =
                            reader["SKU"].ToString(),

                        ProductName =
                            reader["ProductName"].ToString(),

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]
                            ),

                        MinStock =
                            Convert.ToInt32(
                                reader["MinStock"]
                            ),
                            SupplierId =
                            Convert.ToInt32(
                                reader["SupplierId"]
                             )
                    });
                }
            }

            return products;
        }

        // =========================
        // HISTORY
        // =========================
        public List<StockLog> GetHistory()
        {
            List<StockLog> logs = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_InventoryHistory",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(new StockLog
                    {
                        LogId =
                            Convert.ToInt32(
                                reader["LogId"]
                            ),

                        ProductName =
                            reader["ProductName"]
                            .ToString(),

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]
                            ),

                        Type =
                            reader["Type"].ToString(),

                        Note =
                            reader["Note"].ToString(),

                        CreatedAt =
                            Convert.ToDateTime(
                                reader["CreatedAt"]
                            )
                    });
                }
            }

            return logs;
        }
        // =========================
        // DASHBOARD
        // =========================
        public StockDashboard GetDashboard()
        {
            StockDashboard dashboard =
                new StockDashboard();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_StockDashboard",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    dashboard.TotalProducts =
                        Convert.ToInt32(
                            reader["TotalProducts"]
                        );

                    dashboard.TotalStock =
                        Convert.ToInt32(
                            reader["TotalStock"]
                        );

                    dashboard.LowStockProducts =
                        Convert.ToInt32(
                            reader["LowStockProducts"]
                        );

                }
            }

            return dashboard;
        }
        // =========================
        // sEARCH
        // =========================
        public List<Product> Search(string keyword)
        {
            List<Product> products = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_SearchProducts",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Keyword",
                    keyword
                );

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId =
                            Convert.ToInt32(
                                reader["ProductId"]
                            ),

                        SKU =
                            reader["SKU"].ToString(),

                        ProductName =
                            reader["ProductName"]
                            .ToString(),

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]
                            ),

                        Price =
                            Convert.ToDecimal(
                                reader["Price"]
                            ),

                        MinStock =
                            Convert.ToInt32(
                                reader["MinStock"]
                            ),
                        SupplierId =
                            Convert.ToInt32(
                                reader["SupplierId"]
                             )
                    });
                }
            }

            return products;
        }
        // =========================
        // PAGE
        // =========================
        public List<Product> Paging( int page,int pageSize)
        {
            List<Product> products = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_GetProductsPaging",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@Page",
                    page
                );

                cmd.Parameters.AddWithValue(
                    "@PageSize",
                    pageSize
                );

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId =
                            Convert.ToInt32(
                                reader["ProductId"]
                            ),

                        SKU =
                            reader["SKU"].ToString(),

                        ProductName =
                            reader["ProductName"]
                            .ToString(),

                        Quantity =
                            Convert.ToInt32(
                                reader["Quantity"]
                            ),

                        Price =
                            Convert.ToDecimal(
                                reader["Price"]
                            )
                    });
                }
            }

            return products;
        }
        // =========================
        // REPORT
        // =========================
        public object GetReport()
        {
            List<object> reports = new();

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_StockReport",
                    conn
                );

                cmd.CommandType =
                    CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    reports.Add(new
                    {
                        ProductName =
                            reader["ProductName"]
                            .ToString(),

                        TotalImport =
                            Convert.ToInt32(
                                reader["TotalImport"]
                            ),

                        TotalExport =
                            Convert.ToInt32(
                                reader["TotalExport"]
                            )
                    });
                }
            }

            return reports;
        }
    }
}
