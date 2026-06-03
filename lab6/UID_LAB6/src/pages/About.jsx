function About() {
  return (
    <div className="container">
      <h1 className="page-title">ℹ️ Про застосунок</h1>

      <div className="about-card">
        <h2 className="about-section">📌 Опис</h2>
        <p className="about-text">
          Менеджер задач — це веб-застосунок розроблений у рамках лабораторної
          роботи з дисципліни «Розробка інтерфейсів користувача». Застосунок
          реалізує парадигму Single Page Application (SPA).
        </p>
      </div>

      <div className="about-card">
        <h2 className="about-section">🛠 Технології</h2>
        <div className="tech-list">
          {[
            { name: "React 18",         desc: "бібліотека для побудови інтерфейсу" },
            { name: "React Router v6",  desc: "клієнтська маршрутизація (SPA)" },
            { name: "JavaScript ES6+",  desc: "мова програмування" },
            { name: "CSS3",             desc: "стилізація та анімації" },
          ].map(t => (
            <div key={t.name} className="tech-item">
              <span className="tech-name">{t.name}</span>
              <span className="tech-desc">{t.desc}</span>
            </div>
          ))}
        </div>
      </div>

      <div className="about-card">
        <h2 className="about-section">⚡ Функціонал</h2>
        <ul className="feature-list">
          <li>Додавання, виконання та видалення задач</li>
          <li>Фільтрація задач за статусом</li>
          <li>Статистика та прогрес виконання</li>
          <li>Навігація між сторінками без перезавантаження</li>
        </ul>
      </div>

    </div>
  );
}

export default About;