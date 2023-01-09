import { signIn } from "next-auth/react";
import { useState } from "react";

export default function Login() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    return (
        <div className="m-4 p-4 grid grid-cols-1 gap-4 mx-auto">
            <h1 className="text-4xl font-semibold text-center">Login</h1>
            <form>
                <div className="form-control w-full max-w-xs mx-auto">
                    <label className="label">
                        <span className="label-text">Email</span>
                    </label>
                    <input type="text" placeholder="Email" className="input input-bordered w-full max-w-xs" value={email} onChange={(e) => setEmail(e.target.value)} />
                </div>
                <div className="form-control w-full max-w-xs mx-auto">
                    <label className="label">
                        <span className="label-text">Password</span>
                    </label>
                    <input type="password" placeholder="Password" className="input input-bordered w-full max-w-xs" value={password} onChange={(e) => setPassword(e.target.value)} />
                </div>
                <div className="form-control w-full max-w-xs mx-auto mt-4">
                    <button className="btn btn-primary" onClick={(e) => {
                        e.preventDefault();
                        signIn("credentials", {
                            email,
                            password
                        })
                    }
                    }>Login</button>
                </div>
            </form>

        </div>
    );
}