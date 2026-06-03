function Stats({ tasks }) {
  const total     = tasks.length;
  const done      = tasks.filter(t => t.done).length;
  const active    = tasks.filter(t => !t.done).length;
  const percent   = total > 0 ? Math.round((done / total) * 100) : 0;

  return (
    <div className="container">
      <h1 className="page-title">📊 Статистика</h1>

      {total === 0 ? (
        <p className="empty">Ще немає задач. Додайте їх на головній сторінці!</p>
      ) : (
        <>
          {/* Великі картки */}
          <div className="stats-grid">
            <div className="big-stat-card">
              <div className="big-number">{total}</div>
              <div className="big-label">Всього задач</div>
            </div>
            <div className="big-stat-card green">
              <div className="big-number">{done}</div>
              <div className="big-label">Виконано</div>
            </div>
            <div className="big-stat-card blue">
              <div className="big-number">{active}</div>
              <div className="big-label">Активних</div>
            </div>
          </div>

          {/* Прогрес бар */}
          <div className="progress-section">
            <div className="progress-header">
              <span>Прогрес виконання</span>
              <span>{percent}%</span>
            </div>
            <div className="progress-bar-bg">
              <div
                className="progress-bar-fill"
                style={{ width: `${percent}%` }}
              />
            </div>
          </div>

          {/* Список всіх задач */}
          <div className="stats-list">
            <h2 className="stats-subtitle">Всі задачі:</h2>
            {tasks.map(t => (
              <div key={t.id} className={`stats-item ${t.done ? "done" : ""}`}>
                <span className="stats-dot">{t.done ? "✅" : "⏳"}</span>
                <span className="stats-text">{t.text}</span>
                <span className="stats-status">{t.done ? "Виконано" : "Активна"}</span>
              </div>
            ))}
          </div>
        </>
      )}
    </div>
  );
}

export default Stats;