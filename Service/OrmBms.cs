using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Reflection;

namespace Service
{
    public class OrmBms
    {
        private MySqlConnection connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        public OrmBms()
        {

        }

        public bool Initialize(string server, string database, string uid, string password)
        {
            _server = server;
            _database = database;
            _uid = uid;
            _password = password;
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "PORT=3306;" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            return true;
        }

        // open connection
        private bool OpenConnection()
        {
            try
            {
                System.Console.Error.WriteLine("Trying to open connection...");
                connection.Open();
                System.Console.Error.WriteLine("Connection Open !");
                return true;
            }
            catch (MySqlException ex)
            {
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        System.Console.Error.WriteLine("Cannot connect to server. Contact administrator");
                        break;

                    case 1045:
                        System.Console.Error.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        // close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                System.Console.Error.WriteLine("Connection Closed !");
                return true;
            }
            catch (MySqlException ex)
            {
                System.Console.Error.WriteLine(ex.Message);
                return false;
            }
        }

        // Fait un select et popule une collection d'objet
        // La ligne suivante recupere toute les lignes de la table Contact et les store dans une collection d'objet Contact:
        // IEnumerable<Contact> contacts = Orm.ObjectQuery<Contact>("select * from Contact");
        // La classe Contact est definie comme ceci :
        //      class Contact
        //      {
        //          public int Id { get; set; }
        //          public string FirstName { get; set; }
        //          public string LastName { get; set; }
        //          public DateTime? DateOfBirth { get; set; }
        //          public string PhoneNumber { get; set; }
        //      }
        // Les properties de la class Contact doivent avoir exactement les meme noms que les champs en table. La class Contact est la representation de la table Contact
        // Ces classes peuvent etre défini directement dans les modules.
        public IEnumerable<T> ObjectQuery<T>(string query, object queryParameter = null, bool letConnectionOpen = false)
        {
            IEnumerable<T> res;

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return null;
            try
            {
                if (queryParameter != null)
                    res = this.connection.Query<T>(query, queryParameter);
                else
                    res = this.connection.Query<T>(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return null;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        public IEnumerable<dynamic> Query(string query, object queryParameter = null, bool letConnectionOpen = false)
        {
            IEnumerable<dynamic> res;

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return null;

            try
            {
                if (queryParameter != null)
                    res = this.connection.Query(query, queryParameter);
                else
                    res = this.connection.Query(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return null;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        public int InsertObject<T>(T toAdd, bool letConnectionOpen = false)
        {
            string query = "insert into ";
            PropertyInfo[] propertyInfos;
            int res;

            if (toAdd == null)
                return 0;

            string[] names = typeof(T).ToString().Split('.');
            query += names[names.Length - 1].ToLower();
            query += "(";
            propertyInfos = typeof(T).GetProperties();

            foreach (PropertyInfo propertyInfo in propertyInfos)
                query += propertyInfo.Name + ", ";
            query = query.Remove(query.Length - 2);
            query += ") values (";
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(string))
                    query += "'";
                query += propertyInfo.GetValue(toAdd);
                if (propertyInfo.PropertyType == typeof(string))
                    query += "'";
                query += ", ";
            }
            query = query.Remove(query.Length - 2);
            query += ")";

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return 0;

            System.Console.Error.WriteLine(query);
            try
            {
                res = this.connection.Execute(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return 0;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        public int Insert(string query, object parameter = null, bool letConnectionOpen = false)
        {
            int res;

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return 0;

            try
            {
                if (parameter != null)
                    res = this.connection.Execute(query, parameter);
                else
                    res = this.connection.Execute(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return 0;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        // Exemple de comment appeler cette methode :
        // 
        //  Contact contact = ...;
        //   
        //  ...
        //
        //  UpdateObject<Contact>(
        //  @"update Contact
        //  set FirstName = @Firstname,
        //      LastName = @LastName,
        //      PhoneNumber = @PhoneNumber,
        //  where Id = @Id", contact); 
        //
        // Sachant que l'objet contact est tel quel :
        //  class Contact
        //      {
        //          public int Id { get; set; }
        //          public string FirstName { get; set; }
        //          public string LastName { get; set; }
        //          public string PhoneNumber { get; set; }
        //      }
        // TODO : check qui update quoi
        public int UpdateObject<T>(string query, T toUpdate, bool letConnectionOpen = false)
        {
            int res;

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return 0;
            try
            {
                res = this.connection.Execute(query, toUpdate);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return 0;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        // TODO : check qui update quoi
        public int Update(string query, object parameter, bool letConnectionOpen = false)
        {
            int res;

            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return 0;

            try
            {
                if (parameter != null)
                    res = this.connection.Execute(query, parameter);
                else
                    res = this.connection.Execute(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return 0;
            }
            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

        // TODO : Check qui essaye de supprimer quoi.
        public int Delete(string query, object parameter = null, bool letConnectionOpen = false)
        {
            int res;


            if (connection.State == ConnectionState.Closed)
                if (!this.OpenConnection())
                    return 0;

            try
            {
                if (parameter != null)
                    res = this.connection.Execute(query, parameter);
                else
                    res = this.connection.Execute(query);
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine(e.Message);
                return 0;
            }

            if (connection.State == ConnectionState.Open && !letConnectionOpen)
                this.CloseConnection();
            return res;
        }

    }
}
