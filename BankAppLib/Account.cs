using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Renci.SshNet.Security.Cryptography;

namespace BankAppLib
{
    public class Account
    {
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
        public Guid AccountGuid { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public Guid CustomerGuid { get; set; }
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
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error while processong account creation: {(char.ToLower(e.Message[0]) + e.Message.Substring(1))}");
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
    public class Customers
    {
        /*public static bool CreateCustomer()
        {

        }*/
    }
}