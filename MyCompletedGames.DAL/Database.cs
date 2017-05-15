using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace MyCompletedGames.DAL
{
    /// <summary>
    /// Helper class to issue raw commands to the DB like INSERT and SELECT.
    /// </summary>
    public class Database
    {
        static string CONNECTION_STRING;

        /// <summary>
        /// Inits the Database
        /// </summary>
        /// <param name="connString"></param>
        public static void Init(string connString)
        {
            CONNECTION_STRING = connString;
        }

        public static bool StoreProcedureExists(string sprocName)
        {
            return ExecuteScalar("SELECT 1 FROM sys.procedures WHERE Name = '" + sprocName + "'") != null;
        }

        public static byte[] ReadBinaryData(string query)
        {

            int bufferSize = 100;
            byte[] outByte = new byte[bufferSize];
            long retval = 0;
            long startIndex = 0;

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            while (reader.Read())
                            {
                                retval = reader.GetBytes(0, startIndex, outByte, 0, bufferSize);

                                // Continue while there are bytes beyond the size of the buffer.  
                                while (retval == bufferSize)
                                {
                                    ms.Write(outByte, 0, (int)retval);
                                    startIndex += bufferSize;
                                    retval = reader.GetBytes(0, startIndex, outByte, 0, bufferSize);
                                }
                            }

                            ms.Write(outByte, 0, (int)retval);
                            ms.Position = 0;
                            return ms.ToArray();
                        }
                    }
                }
            }
        }



        public static IEnumerable<SqlDataReader> ExecuteReader(string query)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }
                    }
                }
            }

        }

        public static object ExecuteScalar(string query)
        {

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        public static int ExecuteNonQuery(string query)
        {

            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteStoredProcedureScalar(string sproc, params string[] parameters)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(sproc, conn) { CommandType = CommandType.StoredProcedure })
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p.Split('=')[0], SqlDbType.VarChar).Value = p.Split('=')[1];
                    }
                    conn.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        public static int ExecuteStoredProcedureNonQuery(string sproc, byte[] image, params string[] parameters)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(sproc, conn) { CommandType = CommandType.StoredProcedure })
                {
                    if (image != null)
                    {
                        command.Parameters.Add("@Image", SqlDbType.VarBinary, image.Length).Value = image;
                    }

                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p.Split('=')[0], SqlDbType.VarChar).Value = p.Split('=')[1];
                    }


                    conn.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public static IEnumerable<SqlDataReader> ExecuteStoredProcedure(string sproc, params string[] parameters)
        {
            using (var conn = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(sproc, conn) { CommandType = CommandType.StoredProcedure })
                {


                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p.Split('=')[0], SqlDbType.VarChar).Value = p.Split('=')[1];
                    }
                    conn.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }

                    }


                }
            }
        }



    }
}