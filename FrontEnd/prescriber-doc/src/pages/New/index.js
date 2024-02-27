import { useState, useContext, useEffect } from "react";
import Header from "../../components/Header";
import Title from "../../components/Title";
import { FiPlusCircle } from "react-icons/fi";
import { AuthContext } from "../../contexts/auth";
import { toast } from "react-toastify";

import "./new.css";

export default function New() {
  const { getDrugs, createPatient, user } = useContext(AuthContext);
  const [drugs, setDrugs] = useState([]);
  const [loadDrugs, setLoadDrugs] = useState(true);

  const [patient, setPatient] = useState("");
  const [idCard, setIdCard] = useState("");

  const [doctor, setDoctor] = useState("");
  const [selectedDrugs, setSelectedDrugs] = useState([]);
  const [selectedDrug, setSelectedDrug] = useState("");

  function handleSelectedDrug(e) {
    setSelectedDrug(drugs[e.target.value]);
  }
  function handleRemoveSelectedDrug(e) {
    e.preventDefault();
    if (selectedDrug) {
      const updatedCustomerIds = selectedDrugs.filter(
        (customerId) => customerId !== selectedDrug
      );
      if (updatedCustomerIds.length < selectedDrugs.length) {
        toast.success(selectedDrug.name.concat(" Removed."));
        setSelectedDrugs(updatedCustomerIds);
      }
    }
  }
  function handleAddSelectedDrug(e) {
    e.preventDefault();
    if (selectedDrug) {
      if (!selectedDrugs.includes(selectedDrug)) {
        setSelectedDrugs([...selectedDrugs, selectedDrug]);
        toast.success(selectedDrug.name.concat(" Added."));
      } else {
        toast.warning(
          selectedDrug.name.concat(" already added to this patient")
        );
      }
    }
  }

  useEffect(() => {
    async function loadDrugs() {
      const querySnatpshot = await getDrugs()
        .then((data) => {
          let list = [];
          data.forEach((d) => {
            list.push({
              identificationDrugID: d.IdentificationDrugID,
              name: d.name,
            });
          });
          if (data.size === 0) {
            toast.warning("First create Drugs List");
            setLoadDrugs(false);
            return;
          }
          setDoctor(user.email);
          setDrugs(list);
          setLoadDrugs(false);
        })
        .catch((error) => {
          toast.error("Ops! ".concat(error));
          setLoadDrugs(false);
        });
    }
    loadDrugs();
  }, []);

  async function handleRegister(e) {
    e.preventDefault();
    const patientDrugs = selectedDrugs.map(c => c.IdentificationDrugID);
      if (idCard !== "" && patient !== "") {
        const response = await createPatient(
          idCard,
          patient,
          patientDrugs
        );
        if (response) {
          setIdCard("");
          setPatient("");
          setSelectedDrugs([]);
        }
      } else {
        toast.warning("Fill required fields");
      }
  }

  return (
    <div>
      <Header />
      <div className="content">
        <Title name="New Patient">
          <FiPlusCircle size={25} />
        </Title>
      </div>
      <div className="container">
        <form className="form-profile" onSubmit={handleRegister}>
          <label><strong>*</strong>Patient</label>
          <input
            type="text"
            placeholder="Patient"
            value={patient}
            onChange={(e) => setPatient(e.target.value)}
          ></input>
          <label><strong>*</strong>Identification Card</label>
          <input
            type="text"
            placeholder="Identification Card"
            value={idCard}
            onChange={(e) => setIdCard(e.target.value)}
          ></input>
          <label>Doctor</label>
          <input
            disabled={true}
            type="text"
            placeholder="Doctor"
            value={doctor}
            onChange={(e) => setDoctor(e.target.value)}
          ></input>

          <label> Drugs in use</label>
          {loadDrugs ? (
            <input type="text" disabled={true} value="Loading..." />
          ) : (
            <select multiple={true} disabled={true}>
              {selectedDrugs.map((item, index) => {
                return (
                  <option key={index} value={index}>
                    {item.name}
                  </option>
                );
              })}
            </select>
          )}

          <label> Drugs List</label>
          {loadDrugs ? (
            <input type="text" disabled={true} value="Loading..." />
          ) : (
            <select onChange={handleSelectedDrug}>
              <option key="" value="">
                Select
              </option>
              {drugs.map((item, index) => {
                return (
                  <option key={index} value={index}>
                    {item.name}
                  </option>
                );
              })}
            </select>
          )}
          <div>
            <button className="btnAdd" onClick={handleAddSelectedDrug}>
              Add Drug
            </button>
            <button className="btnRemove" onClick={handleRemoveSelectedDrug}>
              Remove Drug
            </button>
          </div>

          <button type="submit">Save</button>
        </form>
      </div>
    </div>
  );
}
