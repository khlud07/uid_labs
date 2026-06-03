import { useState } from "react";
import TaskInput from "../components/TaskInput";
import TaskList from "../components/TaskList";

function Home({ tasks, onAdd, onToggle, onDelete, onClearDone }) {
  const [filter, setFilter] = useState("all");

  const filteredTasks = tasks.filter(t => {
    if (filter === "active") return !t.done;
    if (filter === "done")   return t.done;
    return true;
  });

  const doneCount   = tasks.filter(t => t.done).length;
  const activeCount = tasks.filter(t => !t.done).length;

  return (
    <div className="container">
      <h1 className="page-title">Мої задачі</h1>

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

      <TaskInput onAdd={onAdd} />

      {/* Фільтри */}
      <div className="filters">
        {["all","active","done"].map(f => (
          <button
            key={f}
            className={filter === f ? "filter-btn active" : "filter-btn"}
            onClick={() => setFilter(f)}
          >
            { f === "all" ? `Всі (${tasks.length})`
            : f === "active" ? `Активні (${activeCount})`
            : `Виконані (${doneCount})` }
          </button>
        ))}
      </div>

      <TaskList tasks={filteredTasks} onToggle={onToggle} onDelete={onDelete} />

      {doneCount > 0 && (
        <button className="clear-btn" onClick={onClearDone}>
          🗑 Очистити виконані
        </button>
      )}

      {tasks.length === 0 && (
        <p className="empty">Задач немає. Додайте першу! 🎯</p>
      )}
    </div>
  );
}

export default Home;