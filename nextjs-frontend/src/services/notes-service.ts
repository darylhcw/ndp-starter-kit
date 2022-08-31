import Api from './api';

// Fake throttle api to be slower for testing/debugging.
const useFakeThrottle = false;
const fakeThrottleTime = 2500;

interface Note {
  id: number;
  text: string;
}

const DefaultExport = {
  fetchAllNotes: fetchAllNotes,
  addNote: addNote,
  deleteNote: deleteNote
}


async function fetchAllNotes() {
  if (useFakeThrottle) {
    await new Promise(_ => setTimeout(_, fakeThrottleTime));
  }

  try {
    const res = await Api().get<Note[]>("notes");
    return res.data;
  } catch (err) {
    throw err;
  }
}

async function addNote(newNote: Note) {
  if (useFakeThrottle) {
    await new Promise(_ => setTimeout(_, fakeThrottleTime));
  }

  try {
    const res = await Api().post("notes/add", newNote);
    return res.data;
  } catch (err) {
    throw err;
  }
}

async function deleteNote(id: number) {
  if (useFakeThrottle) {
    await new Promise(_ => setTimeout(_, fakeThrottleTime));
  }

  try {
    await Api().delete("notes/" + id.toString() );
    return true
  } catch (err) {
    throw err;
  }
}

export {
  DefaultExport as default,

  fetchAllNotes, addNote, deleteNote,

  type Note
}
