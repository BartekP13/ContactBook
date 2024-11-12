using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactBook.Core.ViewModel.Controls
{
    public class BasePearson : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StreetName { get; set; }
        public int HouseNumber { get; set; }
        public int ApartmentNumber { get; set; }
        public string PostalCode { get; set; }
        public string Town { get; set; }
        public int PhoneNumber { get; set; }
        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value.Date;
                OnPropertyChanged(nameof(DateOfBirth));
                OnPropertyChanged(nameof(Age));
            }
        }
        public int Age
        {
            get {
                int age = DateTime.Today.Year - _dateOfBirth.Year;

                if (DateTime.Today.Month < _dateOfBirth.Month || (DateTime.Today.Month == _dateOfBirth.Month && DateTime.Today.Day < _dateOfBirth.Day))
                {
                    age--;
                }

                return age;
            }
            set { }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged(nameof(IsEditing));
                }
            }
        }

        private bool _errorDataInfo;
        public bool ErrorDataInfo
        {
            get { return _errorDataInfo; }
            set
            {
                if (_errorDataInfo != value)
                {
                    _errorDataInfo = value;
                    OnPropertyChanged(nameof(ErrorDataInfo));
                }
            }
        }

        private bool _synchronizeData;
        public bool SynchronizeData
        {
            get { return _synchronizeData; }
            set
            {
                if (_synchronizeData != value)
                {
                    _synchronizeData = value;
                    OnPropertyChanged(nameof(SynchronizeData));
                }
            }
        }

        public BasePearson()
        {
            _dateOfBirth = DateTime.Today;
        }

        public event PropertyChangedEventHandler? PropertyChanged = (s, e) => { };

        protected void OnPropertyChanged(string name) => PropertyChanged(this, new PropertyChangedEventArgs(name));
    }
}
