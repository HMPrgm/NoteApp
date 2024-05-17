using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Notes
{
    public class Note
    {
        private string text;
        private string title;
        private int id;
        private static int CURR_ID = 1;

        public Note(string text, string title)
        {
            this.text = text;
            this.title = title;

            this.id = CURR_ID;
            CURR_ID++;
        }

        #region Accessors/Mutators
        public string getText()
        {
            return text;
        }
        public void setText(string text)
        {
            this.text = text;
        }

        public string getTitle()
        {
            return title;
        }
        public void setTitle(string title)
        {
            this.title = title;
        }

        public int getId()
        {
            return id;
        }
        public string ToCSV()
        {
            return $"{this.id},{this.title},{this.text}";
        }
        #endregion
        /********** NOTE STORAGE **********
         * Notes are stored in a csv file "notes.csv"
         * The csv order is [id,title,text]
         * Ids are overridden each time
         */
        private static List<Note> notes = new List<Note>();
        private static string filename = "notes.csv";
        private static int ITEMS_VALIDATION =3;
        
        public static void InitializeNotes()
        {
            try {
                List<string> contents = File.ReadAllLines(filename).ToList();
                foreach (string line in contents)
                {
                    if (line.Count(f => f == ',') != ITEMS_VALIDATION - 1) { continue; }

                    string[] lsplit = line.Split(",");
                    notes.Add(new Note(lsplit[2], lsplit[1]));
                }
            }
            catch (FileNotFoundException)
            {
                File.Create(filename);
            }
        }
        public static void SaveNotes()
        {
            //"{note.id},{note.title},{note.text}";
            string[] contents = new string[notes.Count()];
            for (int i = 0; i < notes.Count(); i++)
            {
                contents[i] = notes[i].ToCSV();
            }
            File.WriteAllLines(filename, contents);

        }
        public static bool Remove(Note note)
        {
            return notes.Remove(note);
        }

        public static void Add(Note note)
        {
            notes.Add(note);
        }

        public static void Add(string text, string title)
        {
            Add(new Note(text, title));
        }

        public static List<Note> Get()
        {
            return notes;
        }
        public static Note? GetFirst()
        {
            return notes.Count() > 0? notes[0]:null;
        }
        public static Note? FindByID(int id)
        {
            foreach (Note note in notes)
            {
                if (note.id == id) return note;
            }
            return null;
        }

        public static bool Update(int id, string title = "", string text = "")
        {
            Note? note = FindByID(id);
            if (note == null) return false;
            if (title != "") { note.title = title; }
            if (text != "") { note.text = text; }

            return true;
        }

    }
}
