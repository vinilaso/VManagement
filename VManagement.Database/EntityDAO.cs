using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VManagement.Commons.Interfaces;

namespace VManagement.Database
{
    public class EntityDAO
    {
        private IEntity Entity;

        public EntityDAO(IEntity entity)
        {
            this.Entity = entity;
        }

        public void Insert()
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                SqlBuilder sqlBuilder = new SqlBuilder(Entity);

                conn.Open();
                var cmd = sqlBuilder.InsertClause;
                cmd.Connection = conn;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

        }

        public void Update()
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                SqlBuilder sqlBuilder = new SqlBuilder(Entity);

                conn.Open();
                var cmd = sqlBuilder.UpdateClause;
                cmd.Connection = conn;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public void Delete()
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                SqlBuilder sqlBuilder = new SqlBuilder(Entity);

                conn.Open();
                var cmd = sqlBuilder.DeleteClause;
                cmd.Connection = conn;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

        }

        public IEnumerable<IEntity> GetAll(string whereClause, string sortClause)
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                List<EntityMock> entities = new List<EntityMock>();

                SqlBuilder sqlBuilder = new SqlBuilder(Entity)
                {
                    WhereClause = whereClause,
                    SortClause = sortClause
                };


                conn.Open();
                var cmd = sqlBuilder.SelectClause;
                cmd.Connection = conn;

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    entities.Add(EntityMock.CreateFromReader(reader, Entity.Name));
                }

                return entities;
            }
            finally
            {
                conn.Close();
            }

        }

        public IEntity GetOne(string whereClause)
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                EntityMock mock = new EntityMock();

                SqlBuilder sqlBuilder = new SqlBuilder(Entity)
                {
                    WhereClause = whereClause
                };


                conn.Open();
                var cmd = sqlBuilder.SelectClause;
                cmd.Connection = conn;

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mock = EntityMock.CreateFromReader(reader, Entity.Name);
                }

                return mock;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void Execute(string command)
        {
            using var conn = Security.GetDatabaseConnection();

            try
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = command;

                cmd.ExecuteNonQuery();

            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Classe utilizada como mock no serviço de conexão ao banco de dados.
        /// </summary>
        private class EntityMock : IEntity
        {
            public long Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public Dictionary<string, object?> Fields { get; set; } = new Dictionary<string, object?>();

            public EntityMock() { }

            public static EntityMock CreateFromReader(SqlDataReader reader, string tableName)
            {
                EntityMock mock = new EntityMock();
                mock.Name = tableName;

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i) == "ID")
                        mock.Id = reader.GetInt32(i);
                    else
                        mock.Fields.Add(reader.GetName(i), reader.GetValue(i));
                }

                return mock;
            }

            public void Delete()
            {
                throw new NotImplementedException();
            }

            public IEntity Save()
            {
                throw new NotImplementedException();
            }

            public IEntity Update(string whereClause)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IEntity> GetAll(string whereClause, string sortClause)
            {
                throw new NotImplementedException();
            }

            public IEntity GetOne(string whereClause)
            {
                throw new NotImplementedException();
            }
        }
    }
}
