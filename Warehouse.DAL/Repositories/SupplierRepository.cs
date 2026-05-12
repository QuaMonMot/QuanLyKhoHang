using System.Data;
using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly SqlConnectionFactory _factory;

        public SupplierRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        // =========================
        // GET ALL
        // =========================
        public List<Supplier> GetAll()
        {
            List<Supplier> suppliers = new();

            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_GetSuppliers",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    suppliers.Add(new Supplier
                    {
                        SupplierId = Convert.ToInt32(reader["SupplierId"]),

                        SupplierCode = reader["SupplierCode"].ToString(),

                        SupplierName = reader["SupplierName"].ToString(),

                        Phone = reader["Phone"].ToString(),

                        Address = reader["Address"].ToString()
                    });
                }
            }

            return suppliers;
        }

        // =========================
        // GET BY ID
        // =========================
        public Supplier GetById(int id)
        {
            Supplier supplier = null;

            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Suppliers WHERE SupplierId = @SupplierId",
                    conn
                );

                cmd.Parameters.AddWithValue("@SupplierId", id);

                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    supplier = new Supplier
                    {
                        SupplierId = Convert.ToInt32(reader["SupplierId"]),

                        SupplierCode = reader["SupplierCode"].ToString(),

                        SupplierName = reader["SupplierName"].ToString(),

                        Phone = reader["Phone"].ToString(),

                        Address = reader["Address"].ToString()
                    };
                }
            }

            return supplier;
        }

        // =========================
        // ADD
        // =========================
        public void Add(Supplier supplier)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_AddSupplier",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@SupplierCode",
                    supplier.SupplierCode
                );

                cmd.Parameters.AddWithValue(
                    "@SupplierName",
                    supplier.SupplierName
                );

                cmd.Parameters.AddWithValue(
                    "@Phone",
                    supplier.Phone
                );

                cmd.Parameters.AddWithValue(
                    "@Address",
                    supplier.Address
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // UPDATE
        // =========================
        public void Update(Supplier supplier)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_UpdateSupplier",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@SupplierId",
                    supplier.SupplierId
                );

                cmd.Parameters.AddWithValue(
                    "@SupplierCode",
                    supplier.SupplierCode
                );

                cmd.Parameters.AddWithValue(
                    "@SupplierName",
                    supplier.SupplierName
                );

                cmd.Parameters.AddWithValue(
                    "@Phone",
                    supplier.Phone
                );

                cmd.Parameters.AddWithValue(
                    "@Address",
                    supplier.Address
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        // =========================
        // DELETE
        // =========================
        public void Delete(int id)
        {
            using (SqlConnection conn = _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "sp_DeleteSupplier",
                    conn
                );

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue(
                    "@SupplierId",
                    id
                );

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }
    }
}