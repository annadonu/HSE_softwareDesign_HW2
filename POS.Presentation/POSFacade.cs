using POS.UseCases.Criteria;
using POS.UseCases.Evaluations;
using POS.UseCases.Reports;
using POS.UseCases.Students;

namespace POS.Presentation;

public sealed class POSFacade(
    AddStudentHandler addStudent,
    GetStudentsHandler getStudents,
    RemoveStudentHandler removeStudent,
    AddCriterionHandler addCriterion,
    GetCriteriaHandler getCriteria,
    RemoveCriterionHandler removeCriterion,
    SetEvaluationHandler setEvaluation,
    UpdateEvaluationHandler updateEvaluation,
    RemoveEvaluationHandler removeEvaluation,
    GetEvaluationsHandler getEvaluations,
    GetGradeModesHandler getGradeModes,
    CalculateGradeHandler calculateGrade,
    ExportGradesHandler exportGrades)
{
    public StudentDto AddStudent(string name) =>
        addStudent.Handle(new AddStudentRequest(name));

    public IReadOnlyList<StudentDto> GetStudents() => getStudents.Handle();

    public void RemoveStudent(Guid id) => removeStudent.Handle(id);

    public CriterionDto AddCriterion(string name, decimal maxScore, decimal weight) =>
        addCriterion.Handle(new AddCriterionRequest(name, maxScore, weight));

    public IReadOnlyList<CriterionDto> GetCriteria() => getCriteria.Handle();

    public void RemoveCriterion(Guid id) => removeCriterion.Handle(id);

    public EvaluationDto SetEvaluation(Guid studentId, Guid criterionId, decimal score, string? comment) =>
        setEvaluation.Handle(new SetEvaluationRequest(studentId, criterionId, score, comment));

    public EvaluationDto UpdateEvaluation(Guid studentId, Guid criterionId, decimal score, string? comment) =>
        updateEvaluation.Handle(new SetEvaluationRequest(studentId, criterionId, score, comment));

    public void RemoveEvaluation(Guid studentId, Guid criterionId) =>
        removeEvaluation.Handle(studentId, criterionId);

    public IReadOnlyList<EvaluationDto> GetEvaluations(Guid studentId) =>
        getEvaluations.Handle(studentId);

    public IReadOnlyList<GradeModeDto> GetGradeModes() => getGradeModes.Handle();

    public GradeDto CalculateGrade(Guid studentId, string modeKey) =>
        calculateGrade.Handle(studentId, modeKey);

    public void ExportGrades(string filePath, string modeKey) =>
        exportGrades.Handle(filePath, modeKey);
}
