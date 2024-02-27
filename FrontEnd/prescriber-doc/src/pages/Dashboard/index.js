import { useContext } from "react";
import { AuthContext } from "../../contexts/auth";
import Header from "../../components/Header";
import Title from "../../components/Title";
import { FiUsers } from "react-icons/fi";

export default function Dashboard() {
  return (
    <div>
      <Header />
      <div className="content">
        <Title name="Patients">
          <FiUsers size={25} />
        </Title>
      </div>
    </div>
  );
}
