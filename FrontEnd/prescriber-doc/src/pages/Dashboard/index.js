import { useContext } from "react";
import { AuthContext } from "../../contexts/auth";
import Header from "../../components/Header";
import Title from "../../components/Title";
import { FiUsers, FiPlus, FiSearch,  FiEdit2 } from "react-icons/fi";
import { Link } from "react-router-dom";

import "./dashboard.css";

export default function Dashboard() {
  return (
    <div>
      <Header />
      <div className="content">
        <Title name="Patients">
          <FiUsers size={25} />
        </Title>
        <>
          <Link to="/new" className="new">
            <FiPlus color="#fff" size={25} />
            New Patient
          </Link>

          <table>
            <thead>
              <tr>
                <th scope="col">Patient</th>
                <th scope="col">Doctor</th>
                <th scope="col">Drugs</th>
                <th scope="col">#</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td data-label="Patient">Andre Quintao </td>
                <td data-label="Doctor">Andre Tamietti</td>
                <td data-label="Drugs">
                  <span className="badge" style={{backgroundColor: '#999'}}>
                    Empty
                  </span>
                  </td>

                <td data-label="#">
                  <button className="action" style={{backgroundColor: '#3583f6'}}>
                    <FiSearch color="#fff" size={17} />
                  </button>
                  <button className="action">
                    <FiEdit2 color="#fff" size={17} style={{backgroundColor: '#f6a935'}} />
                  </button>
                </td>
              </tr>


              
              <tr>
                <td data-label="Patient">Andre Quintao </td>
                <td data-label="Doctor">Andre Tamietti</td>
                <td data-label="Drugs">
                  <span className="badge" style={{backgroundColor: '#999'}}>
                    Empty
                  </span>
                  </td>

                <td data-label="#">
                  <button className="action" style={{backgroundColor: '#3583f6'}}>
                    <FiSearch color="#fff" size={17} />
                  </button>
                  <button className="action">
                    <FiEdit2 color="#fff" size={17} style={{backgroundColor: '#f6a935'}} />
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </>
      </div>
      {/* 
      <div className="content">
        <Title name="Patients">
          <FiUsers size={25} />
        </Title>
        <Link to="/new">
          <FiPlus color="#fff" size={25} />
          New Patient
        </Link>
      </div>

      <div className="container">
        <h1>Teste</h1>
      </div> */}
    </div>
  );
}
