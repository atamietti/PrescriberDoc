import { useContext, useState } from "react";
import logo from "../../assets/logo.png";
import { Link } from "react-router-dom";
import { AuthContext } from "../../contexts/auth";

export default function SignUp() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const { signUp, loadingAuth } = useContext(AuthContext);

  async  function handleSubmit(e) {
    e.preventDefault();
    if (name !== '' && email !== '' && password !== '') {
      await signUp(email,password,name)
    }
  }

  return (
    <div className="container-center">
      <div className="login">
        <div className="login-area">
          <img src={logo} alt="Prescriber Doc Logo" />
        </div>
        <form onSubmit={handleSubmit}>
          <h1> Sign Up </h1>
          <input
            type="text"
            placeholder="Full name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
          <input
            type="text"
            placeholder="email@email.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <input
            type="password"
            placeholder="**********"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button type="submit">
            {loadingAuth? 'Loading...': 'Register'}
            </button>
        </form>

        <Link to="/">Already haven an account?</Link>
      </div>
    </div>
  );
}
