﻿using System.Windows.Input;

namespace Pip.Command;

public class DelegateCommand : ICommand
{
    private readonly Func<object?, bool>? _canExecute;
    private readonly Action<object?> _execute;

    public DelegateCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute(parameter);
    }

    public event EventHandler? CanExecuteChanged;

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
