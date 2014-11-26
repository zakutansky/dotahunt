using System;

namespace MushiDb.Services
{
    public class DisposableService : IDisposable
    {
        /// <summary>
        /// The _entities
        /// </summary>
        private HireAProEntities _entities = new HireAProEntities();

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        /// <exception cref="System.Exception">Service is already disposed.</exception>
        protected HireAProEntities Entities
        {
            get
            {
                if (_entities == null)
                {
                    throw new Exception("Service is already disposed.");
                }
                return _entities;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _entities.Dispose();
            _entities = null;
        }
    }
}