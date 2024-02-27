import { useContext } from "react";
import avatarImg from "../../assets/avatar.png";
import { Link } from "react-router-dom";

import { AuthContext } from "../../contexts/auth";
import { FiUsers, FiList, FiLogOut } from "react-icons/fi";
import './header.css'

export default function Header() {
  const { user, logout } = useContext(AuthContext);
  function handleLogout() {
    logout();
  }
  return (
    <div className="sidebar">
      <div>
        <img src={avatarImg} alt="User's avatar" />
        <label>{user.email}</label>
      </div>

      <Link to="/dashboard" >
        <FiUsers color="#FFF" size={24} />
        <span>Patients</span>
      </Link>
      <Link to="/drugs">
        <FiList color="#FFF" size={24} />
        <span>Drugs</span> 
      </Link>
      <Link onClick={handleLogout}>
        <FiLogOut color="#FFF" size={24} />
        <span>Logout</span> 
      </Link>
    </div>
  );
}
