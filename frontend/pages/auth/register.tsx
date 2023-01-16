import { useState } from "react";
import axios from "axios"
import { useRouter } from "next/router";

export default function Register() {
    const router = useRouter();

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [thumbnailUrl, setThumbnailUrl] = useState("");

    return ( 
        <div className="m-4 p-4 grid grid-cols-1 gap-4 mx-auto">
            <h1 className="text-4xl font-semibold text-center">Register</h1>
            <form>
                <div className="form-control w-full max-w-xs mx-auto">
                    <label className="label">
                        <span className="label-text">First name</span>
                    </label>
                    <input type="text" placeholder="First name" className="input input-bordered w-full max-w-xs" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
                </div>
                <div className="form-control w-full max-w-xs mx-auto">
                    <label className="label">
                        <span className="label-text">Last name</span>
                    </label>
                    <input type="text" placeholder="Last name" className="input input-bordered w-full max-w-xs" value={lastName} onChange={(e) => setLastName(e.target.value)} />
                </div>
                <div className="form-control w-full max-w-xs mx-auto">
                    <label className="label">
                        <span className="label-text">Thumbnail</span>
                    </label>
                    <input type="text" placeholder="Thumbnail Url" className="input input-bordered w-full max-w-xs" value={thumbnailUrl} onChange={(e) => setThumbnailUrl(e.target.value)} />
                </div>
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
                    <button className="btn btn-primary" onClick={async (e) => {
                        e.preventDefault();
                        await axios.post('/api/auth/register', {
                            email,
                            firstName,
                            lastName,
                            password,
                            thumbnailUrl
                        })

                        router.push('/auth/login');
                    }
                    }>Register</button>
                </div>
            </form>

        </div>
    );
}