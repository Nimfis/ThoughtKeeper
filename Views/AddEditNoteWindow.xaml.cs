using System;
using System.Windows;
using ThoughtKeeper.DTO;



namespace ThoughtKeeper
{
    public partial class AddEditNoteWindow : Window
    {
        private readonly INoteService _noteService;
        private NoteDTO _currentNote;
        private readonly int _userId;

        public AddEditNoteWindow(NoteDTO noteToEdit, INoteService noteService, int userId)
        {
            InitializeComponent();
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            _currentNote = noteToEdit ?? throw new ArgumentNullException(nameof(noteToEdit));
            _userId = userId;
            titleTextBox.Text = _currentNote.Title; 
            contentTextBox.Text = _currentNote.Content; 
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentNote.Title = titleTextBox.Text; 
            _currentNote.Content = contentTextBox.Text; 

            if (_currentNote.Id == 0)
            {
                _noteService.AddNote(_currentNote);
            }
            else
            {
                // Wywołanie metody EditNote z NoteService
                _noteService.EditNote(_currentNote, _userId);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}