using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.model.structures;
using hsr_museum.src.main.model.structures.items.museum_event;

namespace hsr_museum.src.main.model.utilities
{
    public static class IdentificationSearch
    {

        static ObjectFileDAO<Employee>? employeesFileDAO;
        static ObjectFileDAO<Exhibition>? exhibitionsFileDAO;
        static ObjectFileDAO<Events>? eventsFileDAO;

        /// <summary>
        /// initializes this static class
        /// </summary>
        /// <param name="seedsDAO"> The DAO file associated with seeds </param>
        /// <param name="plantsDAO"> The DAO file associated with plants </param>
        public static void init(ObjectFileDAO<Employee> employeesDAO, ObjectFileDAO<Exhibition> exhibitionsDAO,
                                    ObjectFileDAO<Events> eventsDAO) {
            employeesFileDAO = employeesDAO;
            exhibitionsFileDAO = exhibitionsDAO;
            eventsFileDAO = eventsDAO;
        }

        private static Employee employeeSearch(uint id) {
            return employeesFileDAO.getObject(id);
        }

        private static Exhibition exhibitionSearch(uint id) {
            return exhibitionsFileDAO.getObject(id);
        }

    }
}