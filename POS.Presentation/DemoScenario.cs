using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.Presentation;

public sealed class DemoScenario(POSFacade facade)
{
    private const string ExportPath = "demo-grades.csv";

    public void Run()
    {
        var students = new List<StudentDto>();
        var criteria = new List<CriterionDto>();

        try
        {
            Section("1. Студенты и критерии");
            var ivan = AddStudent(students, "[демо] Иванов Иван");
            var anna = AddStudent(students, "[демо] Петрова Анна");
            var practice = AddCriterion(criteria, "[демо] Практика", 10, 2);
            var test = AddCriterion(criteria, "[демо] Контрольная", 20, 1);
            var exam = AddCriterion(criteria, "[демо] Экзамен", 10, 3);

            Section("2. Выставление оценок");
            Evaluate(ivan, practice, 8, "Хорошая работа");
            Evaluate(ivan, test, 15, null);
            Evaluate(anna, practice, 10, null);
            Evaluate(anna, test, 18, null);
            Evaluate(anna, exam, 9, "Отличный ответ");

            Section("3. Защита инвариантов");
            Attempt("Балл 12 при максимуме 10",
                () => facade.SetEvaluation(anna.Id, exam.Id, 12, null));
            Attempt("Отрицательный балл",
                () => facade.SetEvaluation(ivan.Id, exam.Id, -1, null));
            Attempt("Критерий, которого нет в рубрикаторе",
                () => facade.SetEvaluation(ivan.Id, Guid.NewGuid(), 5, null));
            Attempt("Повторная оценка по тому же критерию",
                () => facade.SetEvaluation(ivan.Id, practice.Id, 9, null));

            Section("4. Исправление и отмена оценок");
            facade.UpdateEvaluation(ivan.Id, practice.Id, 9, "Пересмотрено");
            Console.WriteLine("Оценка за практику исправлена: 9/10");
            facade.RemoveEvaluation(ivan.Id, test.Id);
            Console.WriteLine("Оценка за контрольную отменена");
            Evaluate(ivan, exam, 6, null);
            PrintEvaluations(ivan);
            PrintEvaluations(anna);

            Section("5. Итоги в разных режимах расчёта");
            var modes = facade.GetGradeModes();
            foreach (var mode in modes)
            {
                Console.WriteLine(mode.Description + ":");
                foreach (var student in students)
                {
                    var grade = facade.CalculateGrade(student.Id, mode.Key);
                    Console.WriteLine($"  {grade.StudentName}: {grade.Grade}/100");
                }
            }

            Section("6. Экспорт итогов");
            facade.ExportGrades(ExportPath, modes[0].Key);
            Console.WriteLine($"Файл {ExportPath}:");
            foreach (var line in File.ReadAllLines(ExportPath))
                Console.WriteLine("  " + line);
        }
        finally
        {
            foreach (var student in students)
                facade.RemoveStudent(student.Id);
            foreach (var criterion in criteria)
                facade.RemoveCriterion(criterion.Id);
            Section("Демо-данные удалены, файл экспорта сохранён");
        }
    }

    private StudentDto AddStudent(List<StudentDto> created, string name)
    {
        var student = facade.AddStudent(name);
        created.Add(student);
        Console.WriteLine($"Студент: {student.Name}");
        return student;
    }

    private CriterionDto AddCriterion(List<CriterionDto> created, string name, decimal maxScore, decimal weight)
    {
        var criterion = facade.AddCriterion(name, maxScore, weight);
        created.Add(criterion);
        Console.WriteLine($"Критерий: {criterion.Name}, максимум {criterion.MaxScore}, вес {criterion.Weight}");
        return criterion;
    }

    private void Evaluate(StudentDto student, CriterionDto criterion, decimal score, string? comment)
    {
        var result = facade.SetEvaluation(student.Id, criterion.Id, score, comment);
        Console.WriteLine($"{student.Name} — {result.CriterionName}: {result.Score}/{result.MaxScore}");
    }

    private void PrintEvaluations(StudentDto student)
    {
        Console.WriteLine($"Оценки: {student.Name}");
        foreach (var item in facade.GetEvaluations(student.Id))
            Console.WriteLine($"  {item.CriterionName}: {item.Score}/{item.MaxScore}");
    }

    private static void Attempt(string title, Action action)
    {
        try
        {
            action();
            Console.WriteLine($"{title}: принято");
        }
        catch (DomainException ex)
        {
            Console.WriteLine($"{title}: отклонено — {ex.Message}");
        }
    }

    private static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine($"--- {title} ---");
    }
}
