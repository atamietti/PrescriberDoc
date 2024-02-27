import { useState, createContext, useEffect } from "react";

import {  useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export const AuthContext = createContext({});
const PRESCRIBER_API = "https://localhost:44315/api/";
function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loadingAuth, setLoadingAuth] = useState(null);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();

  useEffect(() => {
    async function loadUser() {
      const storageUser = localStorage.getItem("@prescriberdoc");
      if (storageUser) {
        setUser(JSON.parse(storageUser));
        setLoading(false);
      }
      setLoading(false);
    }
    loadUser();
  }, []);

  async function signIn(email, password) {
    setLoadingAuth(true);
    try {
      const response = await fetch(PRESCRIBER_API.concat("auth/login"), {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
        }),
      });
      const data = await response.json();

      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.success) {
        setUser(data);
        storageUser(data);
        toast.success("Welcome!");
        setLoadingAuth(true);
        navigate("/dashboard");
      }
    } catch (error) {
      toast.error("Ops! ".concat(error));
      setLoadingAuth(false);
    }
  }
  async function signUp(email, password, fullname) {
    setLoadingAuth(true);
    try {
      const response = await fetch(PRESCRIBER_API.concat("auth/register"), {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          username: email,
          email: email,
          fullname: fullname,
          password: password,
          confirmPassword: password,
        }),
      });

      const data = await response.json();
      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.success) signIn(email, password);

      setLoadingAuth(false);
    } catch (error) {
      setLoadingAuth(false);
      toast.error("Ops! ".concat(error));
    }
  }
  function storageUser(data) {
    localStorage.setItem("@prescriberdoc", JSON.stringify(data));
  }

  function logout() {
    localStorage.removeItem("@prescriberdoc");
    setUser(null);
  }

  async function createDrug(drugId, drugName, description, company, dosage) {
    setLoadingAuth(true);
    try {
      let options = {
        method: "POST",
        headers: {
          "Access-Control-Allow-Origin":"*",
          "Access-Control-Allow-Headers":"Origin, X-Requested-With, Content-Type, Accept",
          "Content-Type": "application/json",
          "Authorization": "Bearer " + user.accessToken,
        },
        body: JSON.stringify({
          IdentificationDrugID: drugId,
          name: drugName,
          description: description,
          company: company,
          dosage: dosage,
        }),
      };
      const response = await fetch(PRESCRIBER_API.concat("drug"), options);
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.id !=="" ) {
   
        toast.success(data.name.concat(" Saved!"));
        setLoadingAuth(true);
        return true;
      }
    } catch (error) {
      toast.error("Ops! ".concat(error));
      setLoadingAuth(false);
      console.error("Ops! ", error);
    }
    return false;
  }

  async function createPatient(IdentificationCard, name, drugs) {
 
    try {
      let options = {
        method: "POST",
        headers: {
          "Access-Control-Allow-Origin":"*",
          "Access-Control-Allow-Headers":"Origin, X-Requested-With, Content-Type, Accept",
          "Content-Type": "application/json",
          "Authorization": "Bearer " + user.accessToken,
        },
        body: JSON.stringify({
          IdentificationCard: IdentificationCard,
          name: name,
          doctorId: user.email,
          drugs: drugs,
        }),
      };
      const response = await fetch(PRESCRIBER_API.concat("patient"), options);
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.id !=="" ) {
   
        toast.success(data.name.concat(" Saved!"));
        return true;
      }
    } catch (error) {
      toast.error("Ops! ".concat(error));
      console.error("Ops! ", error);
    }
    return false;
  }
  async function getDrugs() {

    try {
      let options = {
        method: "GET",
        headers: {
          "Access-Control-Allow-Origin":"*",
          "Access-Control-Allow-Headers":"Origin, X-Requested-With, Content-Type, Accept",
          "Content-Type": "application/json",
          "Authorization": "Bearer " + user.accessToken,
        },
        
      };
      const response = await fetch(PRESCRIBER_API.concat("drugs"), options);
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.id !=="" ) {
        return data;
      }
    } catch (error) {
      toast.error("Ops! ".concat(error));
      setLoadingAuth(false);
      console.error("Ops! ", error);
    }
    return null;
  }

  async function getPatients() {

    try {
      let options = {
        method: "GET",
        headers: {
          "Access-Control-Allow-Origin":"*",
          "Access-Control-Allow-Headers":"Origin, X-Requested-With, Content-Type, Accept",
          "Content-Type": "application/json",
          "Authorization": "Bearer " + user.accessToken,
        },
        
      };
      const response = await fetch(PRESCRIBER_API.concat("patients"), options);
      const data = await response.json();
      if (!response.ok) {
        throw new Error(data?.detail);
      }

      if (data.id !=="" ) {
        return data;
      }
    } catch (error) {
      toast.error("Ops! ".concat(error));
      setLoadingAuth(false);
      console.error("Ops! ", error);
    }
    return null;
  }

  return (
    <AuthContext.Provider
      value={{
        signed: !!user,
        user,
        signIn,
        signUp,
        logout,
        loadingAuth,
        loading,
        createDrug,
        getDrugs,
        createPatient,
        getPatients
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export default AuthProvider;
