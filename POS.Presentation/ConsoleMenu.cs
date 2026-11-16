using System.Globalization;
using POS.Models;
using POS.UseCases.Criteria;
using POS.UseCases.Students;

namespace POS.Presentation;

public sealed class ConsoleMenu(POSFacade facade, DemoScenario demo)
{
    private const string DefaultExportPath = "grades.csv";

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("========== ПОС ==========");
            Console.WriteLine("1. Студенты");
            Console.WriteLine("2. Критерии");
            Console.WriteLine("3. Оценки");
            Console.WriteLine("4. Итог студента");
            Console.WriteLine("5. Экспорт итогов в CSV");
            Console.WriteLine("6. Демонстрационный сценарий");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();
            if (choice is null)
                return;

            try
            {
                switch (choice.Trim())
                {
                    case "1": StudentsMenu(); break;
                    case "2": CriteriaMenu(); break;
                    case "3": EvaluationsMenu(); break;
                    case "4": ShowGrade(); break;
                    case "5": ExportGrades(); break;
                    case "6": demo.Run(); break;
                    case "0":
                        Console.WriteLine("До свидания!");
                        return;
                    default: Console.WriteLine("Неизвестная команда."); break;
                }
            }
            catch (Exception ex) when (ex is DomainException or ConsoleInputException)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                Console.WriteLine($"Ошибка работы с файлом: {ex.Message}");
            }
        }
    }

    private void StudentsMenu()
    {
        Console.WriteLine("\n--- Студенты ---");
        Console.WriteLine("1. Добавить студента");
        Console.WriteLine("2. Показать студентов");
        Console.WriteLine("3. Удалить студента");

        switch (ReadChoice())
        {
            case "1":
                Console.Write("ФИО студента: ");
                var student = facade.AddStudent(Console.ReadLine() ?? "");
                Console.WriteLine($"Добавлен студент: {student.Name}");
                break;
            case "2":
                PrintStudents();
                break;
            case "3":
                facade.RemoveStudent(ChooseStudent().Id);
                Console.WriteLine("Студент удалён.");
                break;
            default: Console.WriteLine("Неизвестная команда."); break;
        }
    }

    private void CriteriaMenu()
    {
        Console.WriteLine("\n--- Критерии ---");
        Console.WriteLine("1. Добавить критерий");
        Console.WriteLine("2. Показать рубрикатор");
        Console.WriteLine("3. Удалить критерий");

        switch (ReadChoice())
        {
            case "1":
                Console.Write("Название критерия: ");
                var name = Console.ReadLine() ?? "";
                var maxScore = ReadDecimal("Максимальный балл: ");
                var weight = ReadDecimal("Вес критерия: ");
                var criterion = facade.AddCriterion(name, maxScore, weight);
                Console.WriteLine($"Добавлен критерий: {criterion.Name}");
                break;
            case "2":
                PrintCriteria();
                break;
            case "3":
                facade.RemoveCriterion(ChooseCriterion().Id);
                Console.WriteLine("Критерий удалён.");
                break;
            default: Console.WriteLine("Неизвестная команда."); break;
        }
    }

    private void EvaluationsMenu()
    {
        Console.WriteLine("\n--- Оценки ---");
        Console.WriteLine("1. Выставить оценку");
        Console.WriteLine("2. Исправить оценку");
        Console.WriteLine("3. Отменить оценку");
        Console.WriteLine("4. Посмотреть оценки студента");

        switch (ReadChoice())
        {
            case "1": SetEvaluation(false); break;
            case "2": SetEvaluation(true); break;
            case "3":
                var student = ChooseStudent();
                var criterion = ChooseCriterion();
                facade.RemoveEvaluation(student.Id, criterion.Id);
                Console.WriteLine("Оценка отменена.");
                break;
            case "4":
                var selected = ChooseStudent();
                var evaluations = facade.GetEvaluations(selected.Id);
                if (evaluations.Count == 0)
                    Console.WriteLine("Оценок пока нет.");
                foreach (var item in evaluations)
                {
                    var comment = item.Comment is null ? "" : $" ({item.Comment})";
                    Console.WriteLine($"{item.CriterionName}: {item.Score}/{item.MaxScore}{comment}");
                }
                break;
            default: Console.WriteLine("Неизвестная команда."); break;
        }
    }

    private void SetEvaluation(bool update)
    {
        var student = ChooseStudent();
        var criterion = ChooseCriterion();
        var score = ReadDecimal("Выставленный балл: ");
        Console.Write("Комментарий (необязательно): ");
        var comment = Console.ReadLine();

        var result = update
            ? facade.UpdateEvaluation(student.Id, criterion.Id, score, comment)
            : facade.SetEvaluation(student.Id, criterion.Id, score, comment);

        Console.WriteLine($"Оценка сохранена: {result.CriterionName} {result.Score}/{result.MaxScore}");
    }

    private void ShowGrade()
    {
        Console.WriteLine("\n--- Итог студента ---");
        var modeKey = ChooseMode();
        var student = ChooseStudent();
        var result = facade.CalculateGrade(student.Id, modeKey);
        Console.WriteLine($"Итог студента {result.StudentName}: {result.Grade}/100");
    }

    private void ExportGrades()
    {
        var modeKey = ChooseMode();
        Console.Write($"Имя CSV-файла (Enter — {DefaultExportPath}): ");
        var path = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(path))
            path = DefaultExportPath;
        facade.ExportGrades(path.Trim(), modeKey);
        Console.WriteLine($"Итоги экспортированы в {path.Trim()}");
    }

    private void PrintStudents()
    {
        var students = facade.GetStudents();
        if (students.Count == 0)
            Console.WriteLine("Студентов пока нет.");
        for (var i = 0; i < students.Count; i++)
            Console.WriteLine($"{i + 1}. {students[i].Name}");
    }

    private void PrintCriteria()
    {
        var criteria = facade.GetCriteria();
        if (criteria.Count == 0)
            Console.WriteLine("Рубрикатор пуст.");
        for (var i = 0; i < criteria.Count; i++)
            Console.WriteLine($"{i + 1}. {criteria[i].Name} | максимум: {criteria[i].MaxScore} | вес: {criteria[i].Weight}");
    }

    private StudentDto ChooseStudent()
    {
        var students = facade.GetStudents();
        if (students.Count == 0)
            throw new ConsoleInputException("Список студентов пуст.");
        PrintStudents();
        return students[ReadIndex("Номер студента: ", students.Count)];
    }

    private CriterionDto ChooseCriterion()
    {
        var criteria = facade.GetCriteria();
        if (criteria.Count == 0)
            throw new ConsoleInputException("Рубрикатор пуст.");
        PrintCriteria();
        return criteria[ReadIndex("Номер критерия: ", criteria.Count)];
    }

    private string ChooseMode()
    {
        var modes = facade.GetGradeModes();
        for (var i = 0; i < modes.Count; i++)
            Console.WriteLine($"{i + 1}. {modes[i].Description}");
        return modes[ReadIndex("Режим расчёта: ", modes.Count)].Key;
    }

    private static string ReadChoice()
    {
        Console.Write("Ваш выбор: ");
        return (Console.ReadLine() ?? "").Trim();
    }

    private static int ReadIndex(string message, int count)
    {
        Console.Write(message);
        if (!int.TryParse(Console.ReadLine(), out var number) || number < 1 || number > count)
            throw new ConsoleInputException("Некорректный номер.");
        return number - 1;
    }

    private static decimal ReadDecimal(string message)
    {
        Console.Write(message);
        var text = (Console.ReadLine() ?? "").Trim().Replace(',', '.');
        if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
            throw new ConsoleInputException("Некорректное число.");
        return value;
    }
}
