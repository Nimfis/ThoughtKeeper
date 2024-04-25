using System;
using System.Windows;
using System.Windows.Controls;
using ThoughtKeeper.DTO;
using ThoughtKeeper.Service;

namespace ThoughtKeeper
{
    public partial class MainWindow : Window
    {
        public string Username { get; }
        public UserDTO User { get; }

        private readonly IUserService _userService;
        private readonly INoteService _noteService;
        private readonly int _userId;

        public MainWindow(int userId, UserDTO user, INoteService noteService, IUserService userService)
        {
            InitializeComponent();
            User = user;
            DataContext = this;
            _userId = userId;
            _userService = userService;
            _noteService = noteService;

            Username = user.Username;

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
                var notes = _noteService.GetAllNotes(_userId);
                NoteList.ItemsSource = notes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading notes: " + ex.Message);
            }
        }

        private void AddNoteButton_Click(object sender, RoutedEventArgs e)
        {
            var newNote = new NoteDTO { UserId = _userId };
            var newNoteWindow = new AddEditNoteWindow(newNote, _noteService, _userId);

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
                var editNoteWindow = new AddEditNoteWindow(selectedNote, _noteService, _userId);
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
            if (selectedNote != null && selectedNote.UserId == _userId)
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

        private void NoteList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (NoteList.SelectedItem is NoteDTO selectedNote)
            {

                MessageBox.Show($"Tytuł: {selectedNote.Title}\nTreść: {selectedNote.Content}\nData Utworzenia: {selectedNote.DateCreated}");
            }
        }

    }
}
