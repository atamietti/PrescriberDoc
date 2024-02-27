import { useState } from "react";
import { useContext } from "react";
import { AuthContext } from "../../contexts/auth";
import Header from "../../components/Header";
import Title from "../../components/Title";
import { FiList } from "react-icons/fi";
import { toast } from "react-toastify";

export default function Drugs() {
  const [drugId, setDrugId] = useState("");
  const [description, setDescription] = useState("");
  const [company, setCompany] = useState("");
  const [dosage, setDosage] = useState("");
  const [name, setName] = useState("");
  const { createDrug } = useContext(AuthContext);

  async function handleRegister(e) {
    e.preventDefault();
    if (drugId !== "" && name !== "") {
      const  response =await createDrug(drugId, name, description, company, dosage);
      if(response)
      {
        setDrugId("");
        setDescription("");
        setCompany("");
        setDosage("");
        setName("");
      }
    }else{
        toast.warning("Fill required fields")
    }

  }

  return (
    <div>
      <Header />
      <div className="content">
        <Title name="Drugs">
          <FiList size={25} />
        </Title>
      </div>
      <div className="container">
        <form className="form-profile" onSubmit={handleRegister}>
          <label htmlFor={drugId}><strong>*</strong>Identification Code:</label>
          <input
            type="text"
            placeholder="Identification Code"
            value={drugId}
            onChange={(e) => setDrugId(e.target.value)}
          />
          <label htmlFor={name}><strong>*</strong>Drug Name:</label>
          <input
            type="text"
            placeholder="Drug Name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <label htmlFor={description}>Description:</label>
          <input
            type="text"
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
          <label htmlFor={company}>Company:</label>
          <input
            type="text"
            placeholder="Company name"
            value={company}
            onChange={(e) => setCompany(e.target.value)}
          />
          <label htmlFor={dosage}>Dosage:</label>
          <input
            type="text"
            placeholder="Dosage"
            value={dosage}
            onChange={(e) => setDosage(e.target.value)}
          />

          <button type="submit">Save</button>
        </form>
      </div>
    </div>
  );
}
