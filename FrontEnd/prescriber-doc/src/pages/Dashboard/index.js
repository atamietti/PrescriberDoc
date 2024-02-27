import { useContext, useState, useEffect, useSyncExternalStore } from "react";
import { AuthContext } from "../../contexts/auth";
import Header from "../../components/Header";
import Title from "../../components/Title";
import { FiUsers, FiPlus, FiSearch, FiEdit2 } from "react-icons/fi";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";

import "./dashboard.css";

export default function Dashboard() {
  const { getPatients ,user} = useContext(AuthContext);

  const [patients, setPatients] = useState("");
  const [loading, setLoading] = useState("");

  useEffect(() => {
    async function loadPatients() {
      const querySnatpshot = await getPatients()
        .then((data) => {
          let list = [];
          data.forEach((d) => {
            list.push({
              identificationCard: d.identificationCard,
              name: d.name,
              drugs: d.drugs,
            });
          });
          if (data.size === 0) {
            setLoading(false);
            return;
          }
          setPatients(list);
          setLoading(false);
        })
        .catch((error) => {
          toast.error("Ops! ".concat(error));
          setLoading(false);
        });
    }
    loadPatients();
  }, []);

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

          {patients.length === 0 ? (
            <div className="container dashboard">
              <span>Patients not found</span>
            </div>
          ) : (
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
                  {patients.map((item,index)=>{
                    return(
                      <tr key={index}>
                              <td data-label="Patient">{item.name} </td>
                              <td data-label="Doctor">{user.email}</td>
                              <td data-label="Drugs">
                              {item.drugs.length==0 ? (
                                  <span className="badge" style={{ backgroundColor: "#999" }}>
                                  No Drugs prescribed
                                  </span>
                              ):(
                                  <span className="badge" style={{ backgroundColor: "orange" }} >
                                  Drugs prescribed
                                  </span>
                                )}
                              </td>

                              <td data-label="#">
                              <button
                                className="action"
                                style={{ backgroundColor: "#3583f6" }}
                              >
                                <FiSearch color="#fff" size={17} />
                              </button>
                              <button className="action">
                                <FiEdit2
                                color="#fff"
                                size={17}
                                style={{ backgroundColor: "#f6a935" }}
                                />
                              </button>
                              </td>
                            </tr>

                    )
                  })}

              </tbody>
            </table>
          )}
        </>
      </div>
    </div>
  );
}
