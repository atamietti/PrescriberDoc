import { useContext } from "react";
import { AuthContext } from "../../contexts/auth";

export default function Dashboard() {
  const { logout } = useContext(AuthContext);
  function handleLogout() {
    logout();
  }

  return (
    <div>
      <h1> Dashboard </h1>
      <button onClick={handleLogout}>Logout</button>
    </div>
  );
}
