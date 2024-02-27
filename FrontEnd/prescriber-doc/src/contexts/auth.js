import { useState, createContext, useEffect } from "react";

import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export const AuthContext = createContext({});

function AuthProvider({ children }) {
  const [user, setUser] = useState(null);
  const [loadingAuth, setLoadingAuth] = useState(null);
  const[loading, setLoading]=useState(true);

  const navigate = useNavigate();

  useEffect(()=>{
    async function loadUser(){
        const storageUser = localStorage.getItem('@prescriberdoc');
        if(storageUser){
            setUser(JSON.parse(storageUser));
            setLoading(false);
        }
        setLoading(false)
    }
    loadUser();
  },[])

  async function signIn(email, password) {
    setLoadingAuth(true);
    try {
      const response = await fetch("https://localhost:44315/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: email,
          password: password,
        }),
      });

      if (!response.ok) {
        throw new Error("Sign In Error!");
      }

      const data = await response.json();
      if (data.success) {
        setUser(data);
        storageUser(data);
        toast.success("Welcome!");
        setLoadingAuth(true);
        navigate("/dashboard");
      }
    } catch (error) {
      toast.error("Ops! Sign In Error!");
      setLoadingAuth(false);
      console.error("Sign Up Error!", error);
    }
  }
  async function signUp(email, password, fullname) {
    setLoadingAuth(true);
    try {
      const response = await fetch(
        "https://localhost:44315/api/auth/register",
        {
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
        }
      );

      if (!response.ok) {
        throw new Error("Sign Up Error!");
      }

      const data = await response.json();
      if (data.success) signIn(email, password);

      setLoadingAuth(false);
    } catch (error) {
      setLoadingAuth(false);
      toast.error("Ops! Sign Up Error!", error);
    }
  }
function storageUser(data){
    localStorage.setItem('@prescriberdoc', JSON.stringify(data));
}

function logout(){
    localStorage.removeItem('@prescriberdoc');
    setUser(null);
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
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export default AuthProvider;
