using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VManagement.Commons.Interfaces;
using VManagement.Database;

namespace VManagement.Core.Entities
{
    public class Entity : IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; } = string.Empty;
        public virtual Dictionary<string, object?> Fields { get; } = new Dictionary<string, object?>();
        private EntityDAO _entityDAO => new EntityDAO(this);

        public void Delete(string whereClause)
        {
            _entityDAO.Delete();
        }

        public virtual IEntity Save()
        {
            _entityDAO.Insert();
            return this;
        }

        public virtual IEntity Update(string whereClause)
        {
            _entityDAO.Update();
            return this;
        }

        public virtual IEnumerable<IEntity> GetAll(string whereClause, string sortClause)
        {
            List<IEntity> entities = _entityDAO.GetAll(whereClause, sortClause).ToList();
            return entities;
        }

        public virtual IEntity GetOne(string whereClause)
        {
            IEntity result = _entityDAO.GetOne(whereClause);
            return result;
        }

        public static void Execute(string sqlCommand)
        {
            EntityDAO.Execute(sqlCommand);
        }
    }
}
