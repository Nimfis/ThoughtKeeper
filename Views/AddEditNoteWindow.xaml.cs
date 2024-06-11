using System;
using System.Windows;
using ThoughtKeeper.DTO;
using ThoughtKeeper.Interfaces;

namespace ThoughtKeeper
{
    public partial class AddEditNoteWindow : Window
    {
        private readonly INoteService _noteService;
        private NoteDTO _currentNote;
        private readonly int _userId;

        public AddEditNoteWindow(NoteDTO noteToEdit, INoteService noteService, ICategoryService categoryService, int userId)
        {
            InitializeComponent();
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            _currentNote = noteToEdit ?? throw new ArgumentNullException(nameof(noteToEdit));
            _userId = userId;

            titleTextBox.Text = _currentNote.Title;
            contentTextBox.Text = _currentNote.Content;

            var categories = categoryService.GetAll();
            categoryComboBox.ItemsSource = categories;
            categoryComboBox.DisplayMemberPath = "Name";
            categoryComboBox.SelectedValuePath = "Id";
            categoryComboBox.SelectedValue = _currentNote.CategoryId;

            categoryComboBox.SelectionChanged += categoryComboBox_SelectionChanged;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentNote.Title = titleTextBox.Text;
            _currentNote.Content = contentTextBox.Text;

            if (categoryComboBox.SelectedValue != null)
            {
                _currentNote.CategoryId = (int)categoryComboBox.SelectedValue;
            }
            else
            {
                MessageBox.Show("Proszę wybrać kategorię.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_currentNote.Id == 0)
            {
                _currentNote.UserId = _userId;  // Make sure UserId is set for new notes
                _noteService.AddNote(_currentNote);
            }
            else
            {
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

        private void categoryComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (categoryComboBox.SelectedValue != null)
            {
                _currentNote.CategoryId = (int)categoryComboBox.SelectedValue;
            }
        }
    }
}
