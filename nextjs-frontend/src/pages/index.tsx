import styles from './index.module.scss'
import { useState } from 'react'
import { useQueryClient, useQuery, useMutation } from '@tanstack/react-query';
import NotesApi from '@services/notes-service'
import { Note } from '@services/notes-service'

// Need id for UI. Real id is gotten from db auto-increment.
// '-' ensures we never get clashes.
let TempId = -1;

export default function Home() {
  const QueryClient = useQueryClient();

  const { isLoading, isError, isFetching, data: Notes, error}
    = useQuery<Note[], Error>(["notes"], () => NotesApi.fetchAllNotes());

  const addNoteMutation = useMutation(NotesApi.addNote, {
    // Optimistically set new note, saving previous for fallback if we fail.
    onMutate: async newNote => {
      await QueryClient.cancelQueries(["notes"])
      const previousTodos = QueryClient.getQueryData(["notes"])
      QueryClient.setQueryData(["notes"], (old : Note[] ) => [...old, newNote]);

      setText("");

      return { previousTodos };
    },

    onError: (error, variable, context) => {
      console.log("Add Note failed for", variable, "reverting...");
      console.log("Error:", error);
      QueryClient.setQueryData(["notes"], context.previousTodos);
    },

    onSettled: () => {
      QueryClient.invalidateQueries(["notes"]);
    }
  });

  const deleteNoteMutation = useMutation(NotesApi.deleteNote, {
    // Optimistically delete note, saving previous for fallback if we fail.
    onMutate: async id => {
      await QueryClient.cancelQueries(["notes"])
      const previousTodos = QueryClient.getQueryData(["notes"])
      QueryClient.setQueryData(["notes"], (old : Note[] ) => old.filter(note => note.id != id));

      return { previousTodos };
    },

    onError: (error, variable, context) => {
      console.log("Delete note failed for", variable, "reverting...");
      console.log("Error:", error);
      QueryClient.setQueryData(["notes"], context.previousTodos);
    },

    onSettled: () => {
      QueryClient.invalidateQueries(["notes"]);
    }
  })

  const [Text, setText] = useState("");

  function textChanged(event: React.ChangeEvent<HTMLTextAreaElement>) {
    setText(event.target.value);
  }

  function addNote() {
    if (Text.length == 0) return;
    const newNote = {
      id: TempId--,
      text: Text
    }
    addNoteMutation.mutate(newNote);
  }

  function deleteNote(id: number) {
    return () => {
      deleteNoteMutation.mutate(id);
    }
  }

  function notesListUI() {
    if (isLoading ) return <h2>LOADING...</h2>
    if (isError) return <h2>ERROR: {error.message}</h2>
    return (
      <ul>
        {Notes.map((note, index) => {
          return (
            <li key={note.id} className={styles.note}>
              <p>{index+1} : {note.text}</p>
              <button onClick={deleteNote(note.id)}>Delete</button>
            </li>
          )
        })}
      </ul>
    )
  }

  return (
    <div id={styles.container}>
      {notesListUI()}
      <label id={styles.enter} htmlFor="textInput">Enter note: </label>
      <textarea
        id="textInput"
        value={Text}
        className={styles["text-input"]}
        onChange={textChanged}/>
      <button onClick={addNote}>Add Note</button>
    </div>
  )
}
