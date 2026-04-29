using System;

// Интерфейс команды
public interface ICommand
{
    void Execute();
}

// Получатель (Receiver)
public class RobotVacuum
{
    public void StartCleaning() => Console.WriteLine("Пылесос начал уборку");
    public void StopCleaning() => Console.WriteLine("Пылесос остановил уборку");
    public void ReturnToBase() => Console.WriteLine("Пылесос возвращается на базу");
}

// Конкретные команды
public class StartCleaningCommand : ICommand
{
    private RobotVacuum _robot;
    public StartCleaningCommand(RobotVacuum robot) { _robot = robot; }
    public void Execute() => _robot.StartCleaning();
}

public class StopCleaningCommand : ICommand
{
    private RobotVacuum _robot;
    public StopCleaningCommand(RobotVacuum robot) { _robot = robot; }
    public void Execute() => _robot.StopCleaning();
}

public class ReturnToBaseCommand : ICommand
{
    private RobotVacuum _robot;
    public ReturnToBaseCommand(RobotVacuum robot) { _robot = robot; }
    public void Execute() => _robot.ReturnToBase();
}

// Инициатор (Invoker)
public class RobotController
{
    private ICommand _command;
    public void SetCommand(ICommand command) { _command = command; }
    public void PressButton() { _command.Execute(); }
}

// Демонстрация
class Program
{
    static void Main()
    {
        RobotVacuum robot = new RobotVacuum();
        RobotController controller = new RobotController();

        controller.SetCommand(new StartCleaningCommand(robot));
        controller.PressButton();

        controller.SetCommand(new StopCleaningCommand(robot));
        controller.PressButton();

        controller.SetCommand(new ReturnToBaseCommand(robot));
        controller.PressButton();
    }
}