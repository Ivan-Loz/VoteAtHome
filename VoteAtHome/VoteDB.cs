using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using VoteAtHome.Models;

namespace VoteAtHome
{
    public class VoteDB
    {
        static string masterConn =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

        static string connString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VoteDB;Integrated Security=True";

        public static void CreateDatabase()
        {
            string createDbSql = "IF DB_ID('VoteDB') IS NULL CREATE DATABASE VoteDB;";
            using (var conn = new SqlConnection(masterConn))
            using (var cmd = new SqlCommand(createDbSql, conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("База данных создана");
        }

        public static void CreateTables()
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                conn.Open();
                cmd.CommandText = @" 
            IF OBJECT_ID('Vote') IS NULL 
            CREATE TABLE Vote ( 
                Id INT PRIMARY KEY IDENTITY(1,1), 
                FullName NVARCHAR(90),
                Age INT,
                Description NVARCHAR(180),
                Category NVARCHAR(90),
                VoteCount INT,
                Photo NVARCHAR(90)
            );  

            IF OBJECT_ID('Voice') IS NULL
            CREATE TABLE Voice(
                IdUser INT,
                PhoneNumber NVARCHAR(120),
                ifVoice BIT
            )";

                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Таблицы созданы");
        }

        public static int FillingVote(string FullName, int Age, string Description, string Category, int VoteCount, string Photo)
        {
           
            string sql = @"INSERT INTO Vote (FullName, Age, Description, Category, VoteCount, Photo) 
                   VALUES (@fullname, @age, @description, @category, @voteCount, @photo);
                   SELECT SCOPE_IDENTITY();";

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@fullname", FullName);
                cmd.Parameters.AddWithValue("@age", Age);
                cmd.Parameters.AddWithValue("@description", Description);
                cmd.Parameters.AddWithValue("@category", Category);
                cmd.Parameters.AddWithValue("@voteCount", VoteCount);
                cmd.Parameters.AddWithValue("@photo", Photo);

                conn.Open();
                
                int newId = Convert.ToInt32(cmd.ExecuteScalar());
                Console.WriteLine("Данные заполнены. Новый ID: " + newId);
                return newId;
            }
        }


        public static void FillingVoice(string PhoneNumber, bool ifVoice)
        {
            string sql = "INSERT INTO Voice (PhoneNumber, ifVoice) VALUES (@phonenumber, @ifVoice)";
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@phonenumber", PhoneNumber);
                cmd.Parameters.AddWithValue("@ifVoice", ifVoice);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Данные заполнены");
        }

        
        public static Vote GetVoteById(int id)
        {
            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("SELECT * FROM Vote WHERE Id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        
                        return new Vote
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Description = reader["Description"].ToString(),
                            Category = reader["Category"].ToString(),
                            VoteCount = Convert.ToInt32(reader["VoteCount"]),
                            photo = reader["Photo"].ToString()
                        };
                    }

                    Console.WriteLine("Запись не найдена.");
                    return null;
                }
            }
        }

        
        public static List<Vote> GetAllVotes()
        {
            var votesList = new List<Vote>();

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand("SELECT * FROM Vote", conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        votesList.Add(new Vote
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            FullName = reader["FullName"].ToString(),
                            Age = Convert.ToInt32(reader["Age"]),
                            Description = reader["Description"].ToString(),
                            Category = reader["Category"].ToString(),
                            VoteCount = Convert.ToInt32(reader["VoteCount"]),
                            photo = reader["Photo"].ToString()
                        });
                    }
                }
            }
            return votesList;
        }
        public static bool IncrementVoteCount(int id)
        {
            string sql = "UPDATE Vote SET VoteCount = ISNULL(VoteCount, 0) + 1 WHERE Id = @id";

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static void UpdateVote(Vote vote)
        {
            string sql = @"UPDATE Vote 
                   SET FullName = @fullname, 
                       Age = @age, 
                       Description = @description, 
                       Category = @category
                   WHERE Id = @id";

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", vote.Id);
                cmd.Parameters.AddWithValue("@fullname", vote.FullName);
                cmd.Parameters.AddWithValue("@age", vote.Age);
                cmd.Parameters.AddWithValue("@description", vote.Description);
                cmd.Parameters.AddWithValue("@category", vote.Category);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("Данные в таблице Vote успешно обновлены");
        }
        public static int DeleteVote(int id)
        {
            string sql = "DELETE FROM Vote WHERE Id = @id";

            using (var conn = new SqlConnection(connString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Запись с ID {id} успешно удалена из базы данных.");
                }
                else
                {
                    Console.WriteLine($"Запись с ID {id} не найдена для удаления.");
                }

                return rowsAffected;
            }
        }
    }
}