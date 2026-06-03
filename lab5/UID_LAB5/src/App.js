import { useState } from "react";
import TaskInput from "./components/TaskInput";
import TaskList from "./components/TaskList";
import "./App.css";

function App() {
  const [tasks, setTasks] = useState([]);
  const [filter, setFilter] = useState("all"); // all | active | done

  // Додати задачу
  const addTask = (text) => {
    const newTask = {
      id: Date.now(),
      text,
      done: false,
    };
    setTasks([...tasks, newTask]);
  };

  // Позначити виконаною
  const toggleTask = (id) => {
    setTasks(tasks.map(task =>
      task.id === id ? { ...task, done: !task.done } : task
    ));
  };

  // Видалити задачу
  const deleteTask = (id) => {
    setTasks(tasks.filter(task => task.id !== id));
  };

  // Очистити всі виконані
  const clearDone = () => {
    setTasks(tasks.filter(task => !task.done));
  };

  // Фільтрація
  const filteredTasks = tasks.filter(task => {
    if (filter === "active") return !task.done;
    if (filter === "done")   return task.done;
    return true;
  });

  const doneCount   = tasks.filter(t => t.done).length;
  const activeCount = tasks.filter(t => !t.done).length;

  return (
    <div className="app">
      <div className="container">
        <h1 className="title">📋 Менеджер задач</h1>

        <TaskInput onAdd={addTask} />
        {/* Статистика */}
<div className="stats">
  <div className="stat-card">
    <div className="stat-number">{tasks.length}</div>
    <div className="stat-label">Всього</div>
  </div>
  <div className="stat-card">
    <div className="stat-number">{activeCount}</div>
    <div className="stat-label">Активних</div>
  </div>
  <div className="stat-card">
    <div className="stat-number">{doneCount}</div>
    <div className="stat-label">Виконано</div>
  </div>
</div>

        {/* Фільтри */}
        <div className="filters">
          <button
            className={filter === "all" ? "filter-btn active" : "filter-btn"}
            onClick={() => setFilter("all")}
          >
            Всі ({tasks.length})
          </button>
          <button
            className={filter === "active" ? "filter-btn active" : "filter-btn"}
            onClick={() => setFilter("active")}
          >
            Активні ({activeCount})
          </button>
          <button
            className={filter === "done" ? "filter-btn active" : "filter-btn"}
            onClick={() => setFilter("done")}
          >
            Виконані ({doneCount})
          </button>
        </div>

        <TaskList
          tasks={filteredTasks}
          onToggle={toggleTask}
          onDelete={deleteTask}
        />

        {doneCount > 0 && (
          <button className="clear-btn" onClick={clearDone}>
            🗑 Очистити виконані
          </button>
        )}

        {tasks.length === 0 && (
          <p className="empty">Задач немає. Додайте першу! </p>
        )}
      </div>
    </div>
  );
}

export default App;