using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Input;

namespace ShutdownControl
{
    public class RelayCommand : ICommand
    {
        readonly Func<bool> _canExecute;
        readonly Action _execute;
        // 建構子(多型)
        public RelayCommand(Action execute) :
            this(execute, null)
        {
        }
        // 建構子(傳入參數)
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            // 簡化寫法 if(execute == null) throw new ArgumentNullException("execute");
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        // 當_canExecute發生變更時，加入或是移除Action觸發事件
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null) CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null) CommandManager.RequerySuggested += value;
            }
        }

        // 下面兩個方法是提供給 View 使用的
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
