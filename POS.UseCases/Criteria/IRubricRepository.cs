using POS.Models;

namespace POS.UseCases.Criteria;

public interface IRubricRepository
{
    Rubric Get();
    void Save(Rubric rubric);
}
