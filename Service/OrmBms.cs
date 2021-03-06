﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Dapper;
using System.Data;
using System.Reflection;
using Service.Model;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SQLite;

namespace Service
{
    public class OrmBms
    {
        private DbConnection connection;
        public string Server { get; private set; }
        public string Database { get; private set; }
        public int Port { get; private set; }
        public string Uid { get; private set; }
        public string Password { get; private set; }
        public string ConnectionString { get; private set; }
        public BDDType BddType { get; private set; }

        public OrmBms()
        {

        }

        // Appelé par l'API a l'initialisation, cette methode n'est pas censé etre appelé ailleurs
        public bool Initialize(string server, string database, int port, string uid, string password, BDDType bddType)
        {
            Server = server;
            Database = database;
            Uid = uid;
            Password = password;
            Port = port;
            ConnectionString = "SERVER=" + server + ";" + (bddType.Type == BDDTypeEnum.MySQL ? "PORT=" + port + ";" : "") + (bddType.Type != BDDTypeEnum.Oracle ? "DATABASE=" +
            database + ";" : "") + "UID=" + uid + ";" + "PASSWORD=" + password + ";" + (bddType.Type == BDDTypeEnum.OLEDB ?  "Provider = SQLOLEDB;" : "");
            BddType = bddType;

            switch (bddType.Type)
            {
                case BDDTypeEnum.MySQL:
                    this.connection = new MySqlConnection(ConnectionString);
                    break;
                case BDDTypeEnum.MicrosoftSQL:
                    this.connection = new SqlConnection(ConnectionString);
                    break;
                case BDDTypeEnum.ODBC:
                    this.connection = new OdbcConnection(ConnectionString);
                    break;
                case BDDTypeEnum.OLEDB:
                    this.connection = new OleDbConnection(ConnectionString);
                    break;
                case BDDTypeEnum.SQLite:
                    this.connection = new SQLiteConnection(ConnectionString);
                    break;
                case BDDTypeEnum.Oracle:
                    this.connection = new OracleConnection(ConnectionString);
                    break;
            }
           

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
                System.Console.Error.WriteLine(ex.Message);
                

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


        // Pour chaque fonction de l'orm il y a un parametre optionnel (bool) qui par defaut est a false.
        // Le mettre a true permet de laisser la connection ouverte : /!\ A n'utiliser que si vous plusieurs requetes tres rapidements (genre dans un while) /!\


        // Fait un select et popule une collection d'objet
        // La ligne suivante recupere toute les lignes de la table Contact et les store dans une collection d'objet Contact:
        // IEnumerable<Contact> contacts = Orm.ObjectQuery<Contact>("select * from Contact");
        // La ligne suivante recupere toute les lignes de la table Contact ou le champ LastName = "Doe"
        // IEnumerable<Contact> contacts = Orm.ObjectQuery<Contact>("select * from Contact where FirstName = @firstName", new { firstName = "doe" });
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

        // Sert a faire des selects paramétré 
        // On utilisera cette methode principalement dans le cas ou on veut recuperer des champs en particulier et pas toute la table
        // (contrairement a la methode ObjectQuery qui va recuperer tout les champs de l'objet)
        // Exemple d'utilisation :
        // var res = api.Orm.Query("SELECT LOGIN, PWD FROM user WHERE LOGIN = @login AND PWD = @pwd", new {login = "plaintext", pwd = PwdInVariable});
        // int count = 0;
        // if (caca != null)
        // {
        //     foreach (dynamic toto in caca)
        //     {
        //         count++;
        //     }
        //     if (count > 0)
        //     {
        //          // Y a des resultats !
        //          foreach (dynamic row in res)
        //          {
        //               // On les affiches !
        //               System.Console.Error.Writeline("Login recuperé = " + row.LOGIN);
        //               System.Console.Error.Writeline("Pwd recupere = " + row.PWD);
        //          }
        //     }
        //     else
        //     {
        //          // Il n'y a pas de resultat
        //     }
        // }
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
                if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(DateTime))
                    query += "'";
                query += propertyInfo.GetValue(toAdd);
                if (propertyInfo.PropertyType == typeof(string) || propertyInfo.PropertyType == typeof(DateTime))
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

        public int Execute(string query, object parameter = null, bool letConnectionOpen = false)
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
