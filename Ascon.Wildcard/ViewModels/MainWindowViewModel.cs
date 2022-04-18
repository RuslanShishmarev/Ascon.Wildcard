using Ascon.Wildcard.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
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

        private string _startText = "Тут будет начало текста";
        public string StartText
        {
            get => _startText;
            set
            {
                _startText = value;
                RaisePropertyChanged(nameof(StartText));
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

        private WildcardSearcher _wildcardSearcher;

        #endregion

        #region COMMANDS

        public DelegateCommand SelectFileCommand { get; private set; }
        public DelegateCommand GetWordsByPatternCommand { get; private set; }

        #endregion

        public MainWindowViewModel()
        {
            SelectFileCommand = new DelegateCommand(SelectFile);
            GetWordsByPatternCommand = new DelegateCommand(GetWordsByPattern);
            _wildcardSearcher = new WildcardSearcher(_withRegister);
        }

        #region METHODS

        private void GetWordsByPattern()
        {
            _wildcardSearcher.SetIsWithRegister(WithRegister);
            ResultWords = _wildcardSearcher.SearchWords(UserPattern).ToList();
        }

        private void SelectFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                FilePath = dlg.FileName;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var enc1251 = Encoding.GetEncoding(1251);

                using (StreamReader sr = new StreamReader(FilePath, Encoding.Default))
                {
                    _wildcardSearcher.SetText(sr.ReadToEnd());
                    StartText = new string(_wildcardSearcher.Text.Take(100).ToArray()) + "...";
                }

                ResultWords?.Clear();
            }
        }

        #endregion
    }
}
