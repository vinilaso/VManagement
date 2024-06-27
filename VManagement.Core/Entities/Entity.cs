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
        private long _id { get; set; }
        public virtual long Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                var entity = _entityDAO.GetOne($"ID = {value}");

                if (entity != null) Fields = entity.Fields;
            }
        }
        public virtual string Name { get; } = string.Empty;
        public virtual Dictionary<string, object?> Fields { get; set; } = new Dictionary<string, object?>();
        private EntityDAO _entityDAO => new EntityDAO(this);

        public Entity() { }

        private Entity(IEntity mock)
        {
            Name = mock.Name;
            Id = mock.Id;
            Fields = mock.Fields;
        }

        public virtual void Delete()
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
            Entity result = new(_entityDAO.GetOne(whereClause));
            return result;
        }

        public static void Execute(string sqlCommand)
        {
            EntityDAO.Execute(sqlCommand);
        }
    }
}
