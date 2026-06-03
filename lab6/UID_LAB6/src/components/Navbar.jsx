import { NavLink } from "react-router-dom";

function Navbar() {
  return (
    <nav className="navbar">
      <div className="nav-brand">📋 Менеджер задач</div>
      <div className="nav-links">
        <NavLink to="/"      className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
          🏠 Задачі
        </NavLink>
        <NavLink to="/stats" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
          📊 Статистика
        </NavLink>
        <NavLink to="/about" className={({ isActive }) => isActive ? "nav-link active" : "nav-link"}>
          ℹ️ Про застосунок
        </NavLink>
      </div>
    </nav>
  );
}

export default Navbar;