using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VManagement.Commons.Interfaces;

namespace VManagement.Database
{
    /// <summary>
    /// Classe utilizada para construir consultas SQL
    /// </summary>
    internal class SqlBuilder
    {
        /// <summary>
        /// Construtor de texto
        /// </summary>
        private readonly StringBuilder _builder;
        /// <summary>
        /// Base da construção de consultas
        /// </summary>
        public IEntity? Entity { get; set; }

        /// <summary>
        /// Cláusula Where da consulta
        /// </summary>
        public string WhereClause { get; set; } = string.Empty;
        /// <summary>
        /// Cláusula Order By da consulta
        /// </summary>
        public string SortClause { get; set; } = string.Empty;
        /// <summary>
        /// Retorna um objeto do tipo Microsoft.Data.SqlClient.SqlCommand com o comando INSERT baseado no campo Entity
        /// </summary>
        public SqlCommand InsertClause => BuildInsertClause();
        /// <summary>
        /// Retorna um objeto do tipo Microsoft.Data.SqlClient.SqlCommand com o comando SELECT baseado no campo Entity
        /// </summary>
        public SqlCommand SelectClause => BuildSelectClause();
        /// <summary>
        /// Retorna um objeto do tipo Microsoft.Data.SqlClient.SqlCommand com o comando UPDATE baseado no campo Entity
        /// </summary>
        public SqlCommand UpdateClause => BuildUpdateClause();
        /// <summary>
        /// Retorna um objeto do tipo Microsoft.Data.SqlClient.SqlCommand com o comando DELETE baseado no campo Entity
        /// </summary>
        public SqlCommand DeleteClause => BuildDeleteClause();

        public SqlBuilder()
        {
            _builder = new StringBuilder();
        }

        public SqlBuilder(IEntity entity) : this()
        {
            Entity = entity;
        }

        /// <summary>
        /// Constrói um comando do tipo INSERT baseado no campo Entity
        /// </summary>
        /// <returns>INSERT</returns>
        private SqlCommand BuildInsertClause()
        {
            var result = new SqlCommand();

            _builder.Clear();

            if (this.Entity == null) return new SqlCommand();

            _builder.Append($"INSERT INTO {Entity.Name} ");

            _builder.Append('(');

            foreach (var field in Entity.Fields)
            {
                if (field.Value != null)
                    _builder.Append($"{field.Key}, ");
            }

            TrimBuilder();

            _builder.Append(')');

            _builder.Append(" VALUES ");

            _builder.Append('(');

            foreach (var field in Entity.Fields)
            {
                if (field.Value != null)
                {
                    _builder.Append($"@{field.Key}, ");
                    result.Parameters.AddWithValue($"@{field.Key}", field.Value);
                }
            }

            TrimBuilder();

            _builder.Append(')');

            result.CommandText = _builder.ToString();

            return result;
        }
        /// <summary>
        /// Constrói um comando do tipo SELECT baseado no campo Entity
        /// </summary>
        /// <returns>SELECT</returns>
        private SqlCommand BuildSelectClause()
        {
            if (this.Entity == null) return new SqlCommand();

            var result = new SqlCommand();

            _builder.Clear();

            _builder.Append("SELECT ID, ");

            foreach (var field in Entity.Fields)
            {
                _builder.Append($"{field.Key}, ");
            }

            TrimBuilder();

            _builder.Append($" FROM {Entity.Name}");

            if (!string.IsNullOrEmpty(WhereClause))
            {
                _builder.Append(" WHERE ");
                _builder.Append(WhereClause);
            }

            if (!string.IsNullOrEmpty(SortClause))
            {
                _builder.Append(" ORDER BY ");
                _builder.Append(SortClause);
            }

            result.CommandText = _builder.ToString();

            return result;
        }
        /// <summary>
        /// Constrói um comando do tipo UPDATE baseado no campo Entity
        /// </summary>
        /// <returns>UPDATE</returns>
        private SqlCommand BuildUpdateClause()
        {
            if (this.Entity == null) return new SqlCommand();

            var result = new SqlCommand();

            _builder.Clear();

            _builder.Append($"UPDATE {Entity.Name} SET ");

            foreach (var field in Entity.Fields)
            {
                if (field.Value != null)
                {
                    _builder.Append($"{field.Key} = @{field.Key}, ");
                    result.Parameters.AddWithValue($"@{field.Key}", field.Value);
                }
            }

            TrimBuilder();

            _builder.Append($" WHERE ID = {Entity.Id} ");

            if (!string.IsNullOrEmpty(WhereClause))
            {
                _builder.Append($"AND {WhereClause} ");
            }

            result.CommandText = _builder.ToString();

            return result;
        }
        /// <summary>
        /// Constrói um comando do tipo DELETE baseado no campo Entity
        /// </summary>
        /// <returns>DELETE</returns>
        private SqlCommand BuildDeleteClause()
        {
            if (this.Entity == null) return new SqlCommand();

            var result = new SqlCommand();

            _builder.Clear();

            _builder.Append($"DELETE FROM {Entity.Name} WHERE ID = {Entity.Id}");

            result.CommandText = _builder.ToString();

            return result;
        }

        private void TrimBuilder()
        {
            _builder.Remove(_builder.Length - 1, 1);
            _builder.Remove(_builder.Length - 1, 1);
        }
    }
}
