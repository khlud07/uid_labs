function TaskItem({ task, onToggle, onDelete }) {
  return (
    <li className={task.done ? "task-item done" : "task-item"}>
      <input
        type="checkbox"
        checked={task.done}
        onChange={() => onToggle(task.id)}
        className="checkbox"
      />
      <span className="task-text">{task.text}</span>
      <button
        onClick={() => onDelete(task.id)}
        className="delete-btn"
      >
        ✕
      </button>
    </li>
  );
}

export default TaskItem;