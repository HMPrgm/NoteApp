﻿using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int score = 0;
        private Note curNote;
        private bool collapsed = true;
        
        public MainWindow()
        {
            InitializeComponent();
            Note.InitializeNotes();
            RefreshApp();
            Collapse();
            
        }

        private void RefreshApp(Note? note = null)
        {
            if (note == null) { note = Note.GetFirst(); }
            if (note == null) { AddNewNote(); }
            RefreshNoteList();
            UpdateCurrentNote(note);
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OnClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveCurrentNote();
            Note.SaveNotes();
        }



        private void noteList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Code that gets item clicked (I don't really know how it works)
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            while ((dep != null) && !(dep is ListViewItem))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }
            if (dep == null){ return; }
            TextBlock item = (TextBlock)noteList.ItemContainerGenerator.ItemFromContainer(dep);
            UpdateCurrentNote(Note.FindByID((int)item.Tag));
        }

        private void RefreshNoteList()
        {
            noteList.Items.Clear();
            foreach (Note note in Note.Get())
            {
                TextBlock text = new TextBlock();
                text.Text = note.title == "" ? "Untitled Note" : note.title;
                text.Name = "Note_" + note.id;
                note.textBlock = text;
                text.Tag = note.id;
                noteList.Items.Add(text);
            }
        }

        private void UpdateCurrentNote(Note? note)
        {
            if (note == null) { return; }
            //Saves old note
            if (curNote != null) { SaveCurrentNote(); }
            //Sets New Note
            curNote = note;
            noteTitle.Text = note.title;
            noteBox.Text = note.text;
        }

        private void SaveCurrentNote()
        {
            if (curNote == null) { return; }
            curNote.text = noteBox.Text;
            curNote.title = noteTitle.Text;
            RefreshNoteList();
        }

        private void NoteChanged(object sender, TextChangedEventArgs e)
        {
            string display = noteTitle.Text.Length > 0 ? noteTitle.Text : "Untitled Note";
            mainWindow.Title = display;
            if (curNote != null && curNote.textBlock != null)
            { 
                curNote.textBlock.Text = display; 
            }
        }

        private void AddNewNote(object sender, RoutedEventArgs e) 
        {
            Note note = AddNewNote();
            RefreshApp(note);
        }
        private Note AddNewNote(string title = "", string text = "")
        {
            Note note = new Note(title: title, text: text);
            Note.Add(note);
            return note;
        }

        private void DeleteCurrentNote(object sender, RoutedEventArgs e)
        {
            if (curNote != null)
            { 
                Note.Remove(curNote); 
            }
            
            RefreshApp();
        }


        
        private void Collapse(object sender, RoutedEventArgs e) { Collapse(); }
        private void Collapse()
        {
            sideBar.Width = new GridLength(0);
            mainWindow.Width = (mainWindow.Width * 3) / 4;
            collapseBtn.Visibility = Visibility.Collapsed;
            menuBtn.Visibility = Visibility.Visible;
            collapsed = true;
        }

        private void Expand(object sender, RoutedEventArgs e) {  Expand(); }
        private void Expand()
        {
            sideBar.Width = new GridLength(mainWindow.Width / 3);
            mainWindow.Width = (mainWindow.Width * 4) / 3;
            collapseBtn.Visibility = Visibility.Visible;
            menuBtn.Visibility = Visibility.Collapsed;
            collapsed = false;  
        }
    }
}
