using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes
{
    public class Note
    {
        private String text;
        private String title;
        private int id;
        private static int CURR_ID = 1;

        public Note(String text, String title)
        {
            this.text = text;
            this.title = title;

            this.id = CURR_ID;
            CURR_ID++;
        }
        #region Accessors/Mutators
        public String getText()
        {
            return text;
        }
        public void setText(String text)
        {
            this.text = text;
        }

        public String getTitle()
        {
            return title;
        }
        public void setTitle(String title)
        {
            this.title = title;
        }

        public int getId()
        {
            return id;
        }
        #endregion
        //STATIC
        private static List<Note> notes = new List<Note>();

        public static bool Remove(int _id)
        {
            for (int i = 0; i < notes.Count; i++)
            {
                if (_id==notes.ElementAt(i).id)
                {
                    notes.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public static void Add(Note note)
        {
            notes.Add(note);
        }

        public static void Add(String text, String title)
        {
            notes.Add(new Note(text, title));
        }

        public static List<Note> Get()
        {
            return notes;
        }
        public static Note? FindByID(int id)
        {
            foreach (Note note in notes)
            {
                if (note.id == id) return note;
            }
            return null;
        }

    }
}
