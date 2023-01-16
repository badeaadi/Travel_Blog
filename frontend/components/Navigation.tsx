import axios from "axios";
import { signOut, useSession } from "next-auth/react";
import Link from "next/link";
import useSWR from "swr";
import { AUTH_BASE_URL } from "../types";

const fetcher = (url: string) => axios.get(url, {
    
}).then((res) => res.data)

export default function Navigation() {
    const { data: session, status } = useSession()

    const { data, error } = useSWR(
        `/api/User/${session?.user?.name}`,
        fetcher
    );

    return (
        <>
            <div className="navbar bg-base-200">
                <div className="navbar-start">
                    <div className="dropdown">
                        <label tabIndex={0} className="btn btn-ghost lg:hidden">
                            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h8m-8 6h16" /></svg>
                        </label>
                        <ul tabIndex={0} className="menu menu-compact dropdown-content mt-3 p-2 shadow bg-base-100 rounded-box w-52">
                            <li><Link href="/">Home</Link></li>
                            <li><Link href="/feed">Feed</Link></li>
                            {status === 'unauthenticated' && <li><Link href="/auth/register">Register</Link></li>}
                        </ul>
                    </div>
                    <a className="btn btn-ghost normal-case text-xl">TravelBlog</a>
                </div>
                <div className="navbar-center hidden lg:flex">
                    <ul className="menu menu-horizontal px-1">
                        <li><Link href="/">Home</Link></li>
                        <li><Link href="/feed">Feed</Link></li>
                        {status === 'unauthenticated' && <li><Link href="/auth/register">Register</Link></li>}
                    </ul>
                </div>
                <div className="navbar-end">
                    {status === 'authenticated' && <div className="dropdown dropdown-end">
                        <label tabIndex={0} className="btn btn-ghost btn-circle avatar">
                            <div className="w-10 rounded-full">
                                <img src={data?.thumbnailUrl} alt="Profile" />
                            </div>
                        </label>
                        <ul tabIndex={0} className="menu menu-compact dropdown-content mt-3 p-2 shadow bg-base-100 rounded-box w-52">
                            <li><Link href="/profile">Profile</Link></li>
                            <li onClick={() => signOut()}><a>Logout</a></li>
                        </ul>
                    </div>}
                    {status === 'unauthenticated' && <Link href="/auth/login"><button className="btn btn-primary">Login</button></Link>}
                </div>
            </div>
        </>
    );
}