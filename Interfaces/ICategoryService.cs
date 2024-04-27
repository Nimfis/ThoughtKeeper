using System.Collections.Generic;
using ThoughtKeeper.DTO;

namespace ThoughtKeeper.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDTO> GetAll();
    }
}
