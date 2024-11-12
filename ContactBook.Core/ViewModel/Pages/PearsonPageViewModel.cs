using ContactBook.Core.Database;
using ContactBook.Core.Helpers;
using ContactBook.Core.ViewModel.Controls;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ContactBook.Core.ViewModel.Pages
{
    public class PearsonPageViewModel : BasePearson
    {
        public ObservableCollection<BasePearson> PearsonList { get; set; } = new ObservableCollection<BasePearson>();
        public ICommand AddPearsonCommand { get; set; }
        public ICommand UpdateDatabase {  get; set; }
        public ICommand CancelUpdates {  get; set; }
        public ICommand DeleteFromTable {  get; set; }
        public ICommand EditDataProvider { get; set; }
        public ICommand AcceptEditData { get; set; }

        public PearsonPageViewModel()
        {
            AddPearsonCommand = new RelayCommand(AddNewPearson);
            UpdateDatabase = new RelayCommand(UpdateData);
            CancelUpdates = new RelayCommand(LoadDataFromDatabase);
            DeleteFromTable = new RelayCommandWithParameter(DeletePearson);
            EditDataProvider = new RelayCommandWithParameter(EditData);
            AcceptEditData = new RelayCommandWithParameter(AcceptEdit);
        }

        private void AcceptEdit(object parameter)
        {
            if (parameter is BasePearson pearson &&
                pearson.FirstName != null && pearson.FirstName != String.Empty &&
                pearson.LastName != null && pearson.LastName != String.Empty &&
                pearson.StreetName != null && pearson.StreetName != String.Empty &&
                pearson.HouseNumber.ToString().Length > 0 &&
                pearson.PostalCode != null && pearson.PostalCode != String.Empty &&
                pearson.Town != null && pearson.Town != String.Empty &&
                pearson.PhoneNumber.ToString().Length == 9 &&
                pearson.Age.ToString().Length > 0)
            {
                pearson.IsEditing = false;
                OnPropertyChanged(nameof(IsEditing));
                SynchronizeData = true;
                ErrorDataInfo = false;
            }
            else
            {
                ErrorDataInfo = true;
            }
        }

        private void EditData(object parameter)
        {
            if ( parameter is BasePearson pearson )
            {
                pearson.IsEditing = true;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        private void DeletePearson(object parameter)
        {
            if (parameter is BasePearson pearsonToDelete)
            {
                PearsonList.Remove(pearsonToDelete);
                SynchronizeData = true;
            }
        }

        public void AddNewPearson()
        {
            if (FirstName != null && FirstName != String.Empty &&
                LastName != null && LastName != String.Empty &&
                StreetName != null && StreetName != String.Empty &&
                HouseNumber.ToString().Length > 0 &&
                PostalCode != null && PostalCode != String.Empty &&
                Town != null && Town != String.Empty &&
                PhoneNumber.ToString().Length == 9 &&
                Age.ToString().Length > 0)
            {
                SynchronizeData = true;
                ErrorDataInfo = false;
                using (var dbContext = new DatabaseConfig())
                {
                    int maxId = dbContext.Pearson.Max(p => (int?)p.Id) ?? 0;
                    Id = maxId + 1;

                    var NewPearson = new BasePearson
                    {
                        Id = Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        StreetName = StreetName,
                        HouseNumber = HouseNumber,
                        ApartmentNumber = ApartmentNumber,
                        PostalCode = PostalCode,
                        Town = Town,
                        PhoneNumber = PhoneNumber,
                        DateOfBirth = DateOfBirth.Date,
                        Age = Age,
                    };

                    PearsonList.Add(NewPearson);
                    ClearFields(nameof(FirstName), nameof(LastName), nameof(StreetName),
                    nameof(HouseNumber), nameof(ApartmentNumber), nameof(PostalCode),
                    nameof(Town), nameof(PhoneNumber), nameof(DateOfBirth)
                    );
                }
            }
            else 
            {
                ErrorDataInfo = true;
            }
        }

        public void UpdateData()
        {
            using (var dbContext = new DatabaseConfig())
            {
                var currentPearsonIds = PearsonList.Select(x => x.Id).ToList();
                var databasePearsons = dbContext.Pearson.Where(p => currentPearsonIds.Contains(p.Id)).ToList();

                foreach (var viewModel in PearsonList)
                {
                    var database = databasePearsons.FirstOrDefault(p => p.Id == viewModel.Id);

                    if (database != null)
                    {

                        if (viewModel.FirstName != database.FirstName)
                                database.FirstName = viewModel.FirstName;

                            if (viewModel.LastName != database.LastName)
                                 database.LastName = viewModel.LastName;

                            if (viewModel.StreetName != database.StreetName)
                                database.StreetName = viewModel.StreetName;

                            if (viewModel.HouseNumber != database.HouseNumber)
                                database.HouseNumber = viewModel.HouseNumber;

                            if (viewModel.ApartmentNumber != database.ApartmentNumber)
                                database.ApartmentNumber = viewModel.ApartmentNumber;

                            if (viewModel.PostalCode != database.PostalCode)
                                database.PostalCode = viewModel.PostalCode;

                            if (viewModel.Town != database.Town)
                                database.Town = viewModel.Town;

                            if (viewModel.PhoneNumber != database.PhoneNumber)
                                database.PhoneNumber = viewModel.PhoneNumber;

                        }
                        else
                        {
                            var newPearson = new PearsonViewModel
                            {
                                FirstName = viewModel.FirstName,
                                LastName = viewModel.LastName,
                                StreetName = viewModel.StreetName,
                                HouseNumber = viewModel.HouseNumber,
                                ApartmentNumber = viewModel.ApartmentNumber,
                                PostalCode = viewModel.PostalCode,
                                Town = viewModel.Town,
                                PhoneNumber = viewModel.PhoneNumber,
                                DateOfBirth = viewModel.DateOfBirth,
                                Age = viewModel.Age,
                            };

                            dbContext.Pearson.Add(newPearson);
                        }
                    }
                var CurrentPearsonList = PearsonList.Select(x=> x.Id).ToList();
                var DataToDeleteInDb = dbContext.Pearson.Where(p => !CurrentPearsonList.Contains(p.Id));
                dbContext.Pearson.RemoveRange(DataToDeleteInDb);
                SynchronizeData = false;
                dbContext.SaveChanges();
            }
        }

        public void LoadDataFromDatabase()
        {
            using (var dbContext = new DatabaseConfig())
            {
                PearsonList.Clear();

                foreach (var pearson in dbContext.Pearson)
                {
                    PearsonList.Add(new BasePearson
                    {
                        Id = pearson.Id,
                        FirstName = pearson.FirstName,
                        LastName = pearson.LastName,
                        StreetName = pearson.StreetName,
                        HouseNumber = pearson.HouseNumber,
                        ApartmentNumber = pearson.ApartmentNumber,
                        PostalCode = pearson.PostalCode,
                        Town = pearson.Town,
                        PhoneNumber = pearson.PhoneNumber,
                        DateOfBirth = pearson.DateOfBirth,
                        Age = pearson.Age,
                    });
                }
                SynchronizeData = false;
            }
        }


        private void ClearFields(params string[] propertyNames)
        {
            foreach(var propertyName in propertyNames)
            {
                var propertyInfo = GetType().GetProperty(propertyName);
                if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(this, string.Empty);
                }
                else if (propertyInfo.PropertyType == typeof(int))
                {
                    propertyInfo.SetValue(this, 0);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    propertyInfo.SetValue(this, DateTime.Today);
                }
                OnPropertyChanged(propertyName);
            }
        }
    }
}
