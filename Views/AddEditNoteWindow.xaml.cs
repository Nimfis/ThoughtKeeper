﻿using System;
using System.Collections.Generic;
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

        public AddEditNoteWindow(NoteDTO noteToEdit, INoteService noteService, ICategoryService categoryService,  int userId)
        {
            InitializeComponent();
            _noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
            _currentNote = noteToEdit ?? throw new ArgumentNullException(nameof(noteToEdit));
            _userId = userId;

            titleTextBox.Text = _currentNote.Title; 
            contentTextBox.Text = _currentNote.Content;

            categoryComboBox.ItemsSource = categoryService.GetAll();
            categoryComboBox.DisplayMemberPath = "Name";
            categoryComboBox.SelectedValuePath = "Id";
            categoryComboBox.SelectedValue = _currentNote.CategoryId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentNote.Title = titleTextBox.Text; 
            _currentNote.Content = contentTextBox.Text;
            _currentNote.CategoryId = (int)categoryComboBox.SelectedValue;


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