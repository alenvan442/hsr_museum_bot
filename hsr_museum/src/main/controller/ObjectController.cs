using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hsr_museum.src.main.model.persistence;

namespace hsr_museum_bot.hsr_museum.src.main.controller
{
    public class ObjectController<T>
    {
        ObjectFileDAO<T> objectFileDAO;

        public ObjectController(ObjectFileDAO<T> dao) {
            this.objectFileDAO = dao;
        }

        /// <summary>
        /// Retrieves a player from the database given an ID
        /// </summary>
        /// <param name="UID"> The ID of the player to look for </param>
        /// <returns> The retrieved player </returns>
        public T getObject(ulong UID)
        {
            return objectFileDAO.getObject(UID);
        }

        /// <summary>
        /// Retrieves all of the players in the database
        /// </summary>
        /// <returns> An array of all of the players </returns>
        public T[] getObjects()
        {
            return objectFileDAO.getObjects();
        }

        /// <summary>
        /// Adds a new player to the database
        /// </summary>
        /// <param name="member"> The discord member that will gain a new acconut </param>
        /// <returns> A boolean indicating whether or not the action was successful </returns>
        public void addObject(T obj, ulong id)
        {
            objectFileDAO.addObject(obj, id);
        }

        /// <summary>
        /// Deletes a user's data based on the given ID
        /// </summary>
        /// <param name="UID"> The ID of the player to delete </param>
        /// <returns> A boolean indicating whether or not the action was successful </returns>
        public Boolean deletePlayer(ulong UID)
        {
            return objectFileDAO.deleteObject(UID);
        }

    }
}