using hsr_museum.src.main.model.persistence;
using hsr_museum.src.main.model.structures.items;

namespace hsr_museum.src.main.model.utilities
{
    public static class IdentificationSearch
    {

        static SeedsFileDAO? seedsFileDAO;
        static PlantsFileDAO? plantsFileDAO;

        /// <summary>
        /// initializes this static class
        /// </summary>
        /// <param name="seedsDAO"> The DAO file associated with seeds </param>
        /// <param name="plantsDAO"> The DAO file associated with plants </param>
        public static void init(SeedsFileDAO seedsDAO, PlantsFileDAO plantsDAO) {
            seedsFileDAO = seedsDAO;
            plantsFileDAO = plantsDAO;
        }

        /// <summary>
        /// When passed in an item, we search to find the item
        /// This is typically used when the item object that was passed in was incomplete
        /// For example: only the id and amonut is filled out
        /// This also helps convert between parent and children classes due to json serialization 
        /// that can remove all information of children classes
        /// </summary>
        /// <param name="item"> The item to find </param>
        /// <returns> An item that was founded, possibly null </returns>
        public static Item? idSearch(Item item) {
            if(item is null) {
                return null;
            }

            uint id = item.id;
            uint itemType = id >> 16;
            Item result;

            switch(itemType) {
                case 1:
                    result = seedSearch(id);
                    break;
                case 2:
                    result = plantSearch(id);
                    break;
                case 3:
                    result = new PlantPot();
                    break;
                default:
                    return null;
            }

            result.amount = item.amount;
            return result;

        }

        /// <summary>
        /// Searches and finds a seed item based on the id that was passed in
        /// </summary>
        /// <param name="id"> The id to be found </param>
        /// <returns> a copy of the seed item that was founded </returns>
        private static Seeds seedSearch(uint id) {
            return seedsFileDAO.getSeedsAmonut(id, 1);
        }

        /// <summary>
        /// Searches and finds a plant item based on the id that was passed in
        /// </summary>
        /// <param name="id"> The id to be found </param>
        /// <returns> a copy of the plant item that was founded </returns>
        private static Plant plantSearch(uint id) {
            return plantsFileDAO.getPlantAmount(id, 1);
        }

    }
}