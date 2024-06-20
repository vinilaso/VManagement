using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VManagement.Commons.Interfaces
{
    public interface IEntity
    {
        #region [ Campos ]
        /// <summary>
        /// Identificador. Chave primária no banco de dados.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Nome do objeto no banco de dados. Exemplo: SYS_PARAMETROS
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// KeyValuePair onde são guardados os valores dos campos da entidade.
        /// </summary>
        public Dictionary<string, object?> Fields { get; }

        #endregion

        #region [ Métodos ]
        /// <summary>
        /// Persiste a entidade no banco de dados.
        /// </summary>
        /// <returns>Um objeto do tipo IEntity com as informações persistidas</returns>
        public IEntity Save();
        /// <summary>
        /// Remove a entidade do banco de dados.
        /// </summary>
        /// <param name="whereClause">Cláusula Where da consulta.</param>
        public void Delete();
        /// <summary>
        /// Atualiza a entidade no banco de dados.
        /// </summary>
        /// <param name="whereClause">Cláusula Where da consulta</param>
        /// <returns>Um objeto do tipo IEntity com as informações persistidas</returns>
        public IEntity Update(string whereClause);
        /// <summary>
        /// Retorna as entidades de acordo com os parâmetros informados.
        /// </summary>
        /// <param name="whereClause">Cláusula Where da consulta</param>
        /// <param name="sortClause">Cláusula Order By da consulta</param>
        /// <returns>Um objeto do tipo IEnumerable(Entity) com os registros no banco correspondentes aos parâmetros informados</returns>
        public IEnumerable<IEntity> GetAll(string whereClause, string sortClause);
        /// <summary>
        /// Resgata do banco uma entidade com os parâmetros correspondentes.
        /// </summary>
        /// <param name="whereClause">Cláusula Where da consulta</param>
        /// <returns>Um objeto com as informações resgatadas</returns>
        public IEntity GetOne(string whereClause);
        #endregion
    }
}
