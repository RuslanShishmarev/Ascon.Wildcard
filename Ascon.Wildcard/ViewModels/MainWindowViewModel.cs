using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ascon.Wildcard.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        #region PROPS

        private string _filePath  = "Пуcто";
        public string FilePath
        {
            get => _filePath;
            set 
            { 
                _filePath = value;
                RaisePropertyChanged(nameof(FilePath));
            }
        }

        private string _userPattern;
        public string UserPattern
        {
            get => _userPattern;
            set
            {
                _userPattern = value;
                RaisePropertyChanged(nameof(UserPattern));
            }
        }

        private bool _withRegister;
        public bool WithRegister
        {
            get => _withRegister;
            set
            {
                _withRegister = value;
                RaisePropertyChanged(nameof(WithRegister));
            }
        }

        private List<string> _resultWords;
        public List<string> ResultWords
        {
            get => _resultWords;
            set
            {
                _resultWords = value;
                RaisePropertyChanged(nameof(ResultWords));
            }
        }

        #endregion

        #region COMMANDS

        public DelegateCommand SelectFileCommand { get; private set; }

        #endregion
        public MainWindowViewModel()
        {
            SelectFileCommand = new DelegateCommand(SelectFile);
        }

        #region METHODS

        private void GetWordsByPattern()
        {

        }

        private void SelectFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = ".txt";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                FilePath = dlg.FileName;
            }
        }

        #endregion
    }
}
