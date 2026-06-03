import { useState } from "react";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import Home from "./pages/Home";
import Stats from "./pages/Stats";
import About from "./pages/About";
import "./App.css";

function App() {
  // Стан задач тут — щоб Stats мав доступ
  const [tasks, setTasks] = useState([]);

  const addTask = (text) => {
    setTasks([...tasks, { id: Date.now(), text, done: false, createdAt: new Date() }]);
  };

  const toggleTask = (id) => {
    setTasks(tasks.map(t => t.id === id ? { ...t, done: !t.done } : t));
  };

  const deleteTask = (id) => {
    setTasks(tasks.filter(t => t.id !== id));
  };

  const clearDone = () => {
    setTasks(tasks.filter(t => !t.done));
  };

  return (
    <BrowserRouter>
      <div className="app">
        <Navbar />
        <main className="main-content">
          <Routes>
            <Route path="/" element={
              <Home
                tasks={tasks}
                onAdd={addTask}
                onToggle={toggleTask}
                onDelete={deleteTask}
                onClearDone={clearDone}
              />
            }/>
            <Route path="/stats"  element={<Stats tasks={tasks} />} />
            <Route path="/about"  element={<About />} />
          </Routes>
        </main>
      </div>
    </BrowserRouter>
  );
}

export default App;