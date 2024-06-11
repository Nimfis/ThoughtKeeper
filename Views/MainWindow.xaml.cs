using System;
using System.Windows;
using System.Windows.Controls;
using ThoughtKeeper.DTO;
using ThoughtKeeper.Interfaces;
using ThoughtKeeper.Security;

namespace ThoughtKeeper
{
    public partial class MainWindow : Window
    {
        private UserDTO User { get; }

        private readonly IUserService _userService;
        private readonly INoteService _noteService;
        private readonly ICategoryService _categoryService;
        private readonly IPasswordManager _passwordManager;

        public MainWindow(UserDTO user, INoteService noteService, IUserService userService, ICategoryService categoryService, IPasswordManager passwordManager)
        {
            User = user;

            InitializeComponent();
            DataContext = this;
            _userService = userService;
            _noteService = noteService;
            _categoryService = categoryService;
            _passwordManager = passwordManager;

            userTextBlock.DataContext = User;

            try
            {
                LoadNotes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the main window: " + ex.Message);
            }
        }

        private void LoadNotes()
        {
            try
            {
                var notes = _noteService.GetAllNotes(User.UserId);
                NoteList.ItemsSource = notes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading notes: " + ex.Message);
            }
        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var newNote = new NoteDTO { UserId = User.UserId };
            var newNoteWindow = new AddEditNoteWindow(newNote, _noteService, _categoryService, User.UserId);

            var result = newNoteWindow.ShowDialog();

            if (result == true)
            {
                LoadNotes();
            }
        }

        private void EditNoteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NoteList.SelectedItem is NoteDTO selectedNote)
            {
                var editNoteWindow = new AddEditNoteWindow(selectedNote, _noteService, _categoryService, User.UserId);
                var result = editNoteWindow.ShowDialog();

                if (result == true)
                {
                    LoadNotes();
                }
            }
            else
            {
                MessageBox.Show("Wybierz notatkę, którą chcesz edytować.");
            }
        }

        private void RemoveNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = NoteList.SelectedItem as NoteDTO;
            if (selectedNote != null && selectedNote.UserId == User.UserId)
            {
                _noteService.DeleteNote(selectedNote.Id);
                LoadNotes();
            }
            else
            {
                MessageBox.Show("Wybierz notatkę, którą chcesz usunąć.");
            }
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = NoteList.SelectedItem as NoteDTO;
            if (selectedNote != null)
            {
                var detailsWindow = new NoteDetailWindow(selectedNote);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz notatkę, aby wyświeltić szczegóły.");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(_userService, _noteService, _categoryService, _passwordManager);
            loginWindow.Show();
            this.Close();
        }


        private void NoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check note
        }
    }
}
