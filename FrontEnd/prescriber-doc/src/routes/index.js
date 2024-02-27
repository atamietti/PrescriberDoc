import { Routes, Route } from "react-router-dom";
import SignIn from "../pages/SignIn";
import SignUp from "../pages/SignUp";
import Dashboard from "../pages/Dashboard";
import Drugs from "../pages/Drugs";
import New from "../pages/New";


import Private from "./Private";

function RoutesApp() {
  return (
    <Routes>
      <Route path="/" element={<SignIn/>} />
      <Route path="/register" element={<SignUp/>}/>
      <Route path="/dashboard" element={<Private><Dashboard/></Private>}/>
      <Route path="/drugs" element={<Private><Drugs/></Private>}/>
      <Route path="/new" element={<Private><New/></Private>}/>


    </Routes>
  );
}
export default RoutesApp;
