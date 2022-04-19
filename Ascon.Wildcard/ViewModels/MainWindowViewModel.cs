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

        #region VIEW PROPS
        public string MainWndHeader => "Тестовое задание АСКОН";
        public string WordsColumn => "Слова";
        public string WordsRules => "Управление";
        public string Rule_Pattern => "Паттерн";
        public string Rule_WithRegister => "Учитывать регистр";
        public string Button_SelectFile => "Выбрать файл";
        public string Button_SearchWords => "Найти слова";
        #endregion

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

        private bool _isReady = false;
        public bool IsReady
        {
            get => _isReady;
            set 
            { 
                _isReady = value;
                RaisePropertyChanged(nameof(IsReady));
            }
        }

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

        private async void GetWordsByPattern()
        {            
            _wildcardSearcher.SetIsWithRegister(WithRegister);
            ResultWords = await Task.Run(() =>  _wildcardSearcher.SearchWords(UserPattern)?.ToList());
        }

        private void SelectFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                _wildcardSearcher.Reset();

                FilePath = dlg.FileName;
                
                var encoding = Encoding.Default;

                Stream fs = new FileStream(FilePath, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs, true)) encoding = sr.CurrentEncoding;

                StartText = GetTextFromFile(encoding, FilePath, _wildcardSearcher);

                if (StartText.Contains("�"))
                {
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    var enc1251 = Encoding.GetEncoding(1251);
                    StartText = GetTextFromFile(enc1251, FilePath, _wildcardSearcher);
                    IsReady = _wildcardSearcher.IsReady;
                }
                ResultWords?.Clear();
            }
        }

        private string GetTextFromFile(Encoding encoding, string filePath, WildcardSearcher wildcardSearcher, int charCount = 100)
        {
            using (StreamReader sr = new StreamReader(filePath, encoding))
            {
                wildcardSearcher.SetText(sr.ReadToEnd());
                return new string(wildcardSearcher.Text.Take(charCount).ToArray()) + "...";
            }
        }

        #endregion
    }
}
