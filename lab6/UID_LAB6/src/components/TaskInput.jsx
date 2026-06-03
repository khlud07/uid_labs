import { useState } from "react";

function TaskInput({ onAdd }) {
  const [text, setText] = useState("");

  const handleSubmit = () => {
    if (text.trim() === "") return;
    onAdd(text.trim());
    setText("");
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter") handleSubmit();
  };

  return (
    <div className="task-input">
      <input
        type="text"
        value={text}
        onChange={(e) => setText(e.target.value)}
        onKeyDown={handleKeyDown}
        placeholder="Введіть нову задачу..."
        className="input"
      />
      <button onClick={handleSubmit} className="add-btn">
        Додати
      </button>
    </div>
  );
}

export default TaskInput;