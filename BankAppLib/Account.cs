using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace BankAppLib
{
    public class Account
    {
        public Guid AccountGuid { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Guid CustomerGuid { get; set; }
        public static async Task CreateAccount(string name, int amount, Guid customer)
        {
            try
            {
                Guid? customerGuid = null;
                using (MySqlCommand cmd = new MySqlCommand("select * from customers where Guid = @guid;", new MySqlConnection("server=localhost; port=3306; database=bankapp; uid=root;")))
                {
                    cmd.Parameters.AddWithValue("@guid", customer.ToByteArray());
                    using (MySqlDataReader dr = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (dr.HasRows)
                        {
                            customerGuid = dr.GetGuid("Guid");
                        }
                        else throw new Exception("Customer not found");
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand("insert into accounts values(@guid, @name, @customerguid, @amount);"))
                {
                    cmd.Parameters.AddWithValue("@guid", "guid");
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@customerguid", customerGuid);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while processong account creation: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
            }
        }
        public Account(Guid guid)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from accounts where Guid = @guid", new MySqlConnection("server=localhost; port=3306; database=scoutgest; uid=root;")))
                {
                    cmd.Parameters.AddWithValue("@guid", guid.ToByteArray());
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            AccountGuid = new Guid((byte[])dr["Guid"]);
                            Name = dr["Name"].ToString();
                            Amount = decimal.Parse(dr["Amount"].ToString());
                            CustomerGuid = new Guid((byte[])dr["Guid"]);
                        }
                        else throw new Exception("No account found");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while processing account creation: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
            }
        }
        public async Task Deposit(int amount)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("call transfer(@guid, @amount, @type)", new MySqlConnection("server=localhost; port=3306; database=scoutgest; uid=root;")))
                {
                    cmd.Parameters.AddWithValue("@guid", AccountGuid);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@type", "Withdrawal");
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred while depositing: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
            }
        }
        public async Task Withdraw(int amount)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("call transfer(@guid, @amount, @type)", new MySqlConnection("server=localhost; port=3306; database=scoutgest; uid=root;")))
                {
                    cmd.Parameters.AddWithValue("@guid", AccountGuid);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@type", "Withdrawal");
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"An error occurred while depositing: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
            }
        }
    }
    public class Customer
    {
        public Guid CustomerGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber{ get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public static async Task CreateCustomer(string name, string email, string phone, string address, string zip, string district, string country)
        {
            using (MySqlCommand cmd = new MySqlCommand("insert into customers values(@guid, @name, @email, @phone, @address, @zip, @district, @country)", new MySqlConnection("server=localhost; port=3306; database=scoutgest; uid=root;")))
            {
                cmd.Parameters.AddWithValue("@guid", Guid.NewGuid().ToByteArray());
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@zip", zip);
                cmd.Parameters.AddWithValue("@district", district);
                cmd.Parameters.AddWithValue("@country", country);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        public Customer(Guid guid)
        {
            try
            {
                using (MySqlCommand cmd = new MySqlCommand("select * from customers where Guid = @guid", new MySqlConnection("server=localhost; port=3306; database=scoutgest; uid=root;")))
                {
                    cmd.Parameters.AddWithValue("@guid", guid.ToByteArray());
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            CustomerGuid = new Guid((byte[])dr["Guid"]);
                            Name = dr["Name"].ToString();
                            Email = dr["Email"].ToString();
                            PhoneNumber = dr["PhoneNumber"].ToString();
                            Address = dr["Address"].ToString();
                            ZipCode = dr["ZipCode"].ToString();
                            District = dr["Email"].ToString();
                            Country = dr["District"].ToString();
                        }
                        else throw new Exception("No customer found");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while processing customer creation: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
            }
        }
    }
}